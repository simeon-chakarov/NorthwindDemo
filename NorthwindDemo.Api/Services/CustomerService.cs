using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Data;
using NorthwindDemo.Api.Data.Entities;
using NorthwindDemo.Api.Services.Helpers;

namespace NorthwindDemo.Api.Services
{
    public class CustomerService(NorthwindContext dbContext, IMemoryCache cache) : ICustomerService
    {
        private readonly NorthwindContext _dbContext = dbContext;
        private readonly IMemoryCache _cache = cache;

        public async Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

            var trimmedSearch = search?.Trim();
            var cacheKey = $"customers|search={trimmedSearch}|page={page}|pageSize={pageSize}";

            if (_cache.TryGetValue(cacheKey, out PagedResponseDto<CustomerListItemDto>? cached) &&
                cached is not null)
            {
                return cached;
            }

            IQueryable<Customer> query = _dbContext.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(trimmedSearch))
            {
                query = query.Where(c =>
                    c.CompanyName != null &&
                    EF.Functions.Like(c.CompanyName, $"{trimmedSearch}%"));
            }

            var totalCount = await query.CountAsync(ct);

            var skip = (page - 1) * pageSize;

            var items = await query
                .OrderBy(c => c.CompanyName)
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new CustomerListItemDto(
                    c.CustomerId,
                    c.CompanyName ?? string.Empty,
                    c.Orders.Count()
                ))
                .ToListAsync(ct);

            var result = new PagedResponseDto<CustomerListItemDto>(items, page, pageSize, totalCount);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return result;
        }

        public async Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var customer = await _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerDetailsDto(
                    c.CustomerId,
                    c.CompanyName ?? "",
                    c.ContactName,
                    c.ContactTitle,
                    c.Address,
                    c.City,
                    c.Region,
                    c.PostalCode,
                    c.Country,
                    c.Phone,
                    c.Fax
                ))
                .SingleOrDefaultAsync(ct);

            return customer;
        }

        public async Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string customerId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return null;
            }

            var exists = await _dbContext.Customers
                .AsNoTracking()
                .AnyAsync(c => c.CustomerId == customerId, ct);

            if (!exists)
            {
                return null;
            }

            var rawOrders = await _dbContext.Orders
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    Total = o.OrderDetails
                        .Select(od => (decimal)od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                        .Sum(),
                    ProductsCount = o.OrderDetails.Count(),
                    HasDiscontinued = o.OrderDetails.Any(od =>
                        od.Product != null && od.Product.Discontinued),
                    HasStockIssue = o.OrderDetails.Any(od =>
                        od.Product != null &&
                        od.Product.UnitsInStock < od.Product.UnitsOnOrder)
                })
                .ToListAsync(ct);

            var orders = rawOrders
                .Select(x => new OrderSummaryDto(
                    x.OrderId,
                    x.OrderDate,
                    x.Total,
                    x.ProductsCount,
                    x.HasDiscontinued || x.HasStockIssue
                        ? FulfillmentWarnings.Build(x.HasDiscontinued, x.HasStockIssue)
                        : null
                ))
                .ToList();

            return new CustomerOrdersResponseDto(customerId, orders);
        }
    }
}
