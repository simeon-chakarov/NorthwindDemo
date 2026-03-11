using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Enums;

namespace NorthwindDemo.Api.Services
{
    /// <summary>
    /// Defines the business logic operations for customer data.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Returns a paginated list of customers with optional search, filtering, and sorting.
        /// </summary>
        /// <param name="search">An optional prefix to filter customers by company name.</param>
        /// <param name="city">An optional exact city name to filter by.</param>
        /// <param name="country">An optional exact country name to filter by.</param>
        /// <param name="sortBy">The field to sort by. Defaults to <see cref="CustomerSortField.CompanyName"/>.</param>
        /// <param name="sortDirection">The direction to sort results. Defaults to <see cref="SortDirection.Ascending"/>.</param>
        /// <param name="page">The 1-based page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page (clamped to 1–100).</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>A paged response containing matching customer list items.</returns>
        Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            string? city,
            string? country,
            CustomerSortField sortBy,
            SortDirection sortDirection,
            int page,
            int pageSize,
            CancellationToken ct = default);

        /// <summary>
        /// Returns full details for a single customer by their identifier.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer details, or <see langword="null"/> if not found or <paramref name="id"/> is blank.</returns>
        Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Returns the order history for a specific customer.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer's orders, or <see langword="null"/> if the customer does not exist or <paramref name="customerId"/> is blank.</returns>
        Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string customerId, CancellationToken ct = default);
    }
}
