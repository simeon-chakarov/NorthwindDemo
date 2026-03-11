using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Data;
using NorthwindDemo.Api.Enums;
using NorthwindDemo.Api.Services.Helpers;

namespace NorthwindDemo.Api.Services
{
    /// <summary>
    /// Implements <see cref="ICustomerService"/> using Entity Framework Core and in-memory caching.
    /// </summary>
    public class CustomerService(NorthwindContext dbContext, IMemoryCache cache) : ICustomerService
    {
        private readonly NorthwindContext _dbContext = dbContext;
        private readonly IMemoryCache _cache = cache;

        /// <inheritdoc/>
        public async Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            string? city,
            string? country,
            CustomerSortField sortBy,
            SortDirection sortDirection,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

            var trimmedSearch = string.IsNullOrWhiteSpace(search) ? string.Empty : search.Trim();
            var trimmedCity = string.IsNullOrWhiteSpace(city) ? string.Empty : city.Trim();
            var trimmedCountry = string.IsNullOrWhiteSpace(country) ? string.Empty : country.Trim();

            var cacheKey =
                $"customers|search={trimmedSearch.ToUpperInvariant()}|city={trimmedCity.ToUpperInvariant()}|country={trimmedCountry.ToUpperInvariant()}|sortBy={sortBy}|sortDir={sortDirection}|page={page}|pageSize={pageSize}";

            if (_cache.TryGetValue(cacheKey, out PagedResponseDto<CustomerListItemDto>? cached) && cached is not null)
            {
                return cached;
            }

            var query = _dbContext.Customers.AsNoTracking();

            if (trimmedSearch.Length > 0)
            {
                query = query.Where(c => EF.Functions.Like(c.CompanyName, $"{trimmedSearch}%"));
            }

            if (trimmedCity.Length > 0)
            {
                query = query.Where(c => c.City == trimmedCity);
            }

            if (trimmedCountry.Length > 0)
            {
                query = query.Where(c => c.Country == trimmedCountry);
            }

            var projectedQuery = query.Select(c => new
            {
                c.CustomerId,
                CompanyName = c.CompanyName ?? string.Empty,
                OrderCount = c.Orders.Count()
            });

            var totalCount = await projectedQuery.CountAsync(ct);

            var orderedQuery = (sortBy, sortDirection) switch
            {
                (CustomerSortField.OrderCount, SortDirection.Ascending) =>
                    projectedQuery.OrderBy(c => c.OrderCount).ThenBy(c => c.CustomerId),

                (CustomerSortField.OrderCount, SortDirection.Descending) =>
                    projectedQuery.OrderByDescending(c => c.OrderCount).ThenByDescending(c => c.CustomerId),

                (_, SortDirection.Descending) =>
                    projectedQuery.OrderByDescending(c => c.CompanyName).ThenByDescending(c => c.CustomerId),

                _ =>
                    projectedQuery.OrderBy(c => c.CompanyName).ThenBy(c => c.CustomerId)
            };

            var items = await orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerListItemDto(
                    c.CustomerId,
                    c.CompanyName,
                    c.OrderCount
                ))
                .ToListAsync(ct);

            var result = new PagedResponseDto<CustomerListItemDto>(items, page, pageSize, totalCount);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return result;
        }

        /// <inheritdoc/>
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
                    c.CompanyName ?? string.Empty,
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

        /// <inheritdoc/>
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
