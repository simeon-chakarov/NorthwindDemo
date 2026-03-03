using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Api.Services
{
    public interface ICustomerService
    {
        Task<IReadOnlyList<CustomerListItemDto>> GetCustomersAsync(string? search, CancellationToken ct = default);

        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);

        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string customerId, CancellationToken ct = default);
    }
}
