using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Mvc.Services
{
    public interface ICustomerApiClient
    {
        Task<IReadOnlyList<CustomerListItemDto>> GetCustomersAsync(string? search, CancellationToken ct = default);
        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);
        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string id, CancellationToken ct = default);
    }
}
