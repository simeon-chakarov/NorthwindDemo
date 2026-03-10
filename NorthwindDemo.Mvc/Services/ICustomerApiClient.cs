using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Mvc.Services
{
    public interface ICustomerApiClient
    {
        Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default);
        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);
        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string id, CancellationToken ct = default);
    }
}
