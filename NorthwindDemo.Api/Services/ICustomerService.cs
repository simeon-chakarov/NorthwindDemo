using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Api.Services
{
    public interface ICustomerService
    {
        Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);

        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string customerId, CancellationToken ct = default);
    }
}
