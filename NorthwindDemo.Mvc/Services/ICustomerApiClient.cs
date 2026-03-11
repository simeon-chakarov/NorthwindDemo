using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Mvc.Services
{
    /// <summary>
    /// Defines the HTTP client contract for communicating with the Northwind API.
    /// </summary>
    public interface ICustomerApiClient
    {
        /// <summary>
        /// Fetches a paginated list of customers from the API, optionally filtered by company name.
        /// </summary>
        /// <param name="search">An optional prefix to filter customers by company name.</param>
        /// <param name="page">The 1-based page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>A paged response containing matching customer list items.</returns>
        Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default);
        /// <summary>
        /// Fetches full details for a single customer by identifier.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer details, or <see langword="null"/> if not found.</returns>
        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Fetches the order history for a specific customer.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer's orders, or <see langword="null"/> if the customer does not exist.</returns>
        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string id, CancellationToken ct = default);
    }
}
