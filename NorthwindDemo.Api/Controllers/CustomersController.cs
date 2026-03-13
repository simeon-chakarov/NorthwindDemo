using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Helpers;
using NorthwindDemo.Api.Services;

namespace NorthwindDemo.Api.Controllers
{
    /// <summary>
    /// API controller for customer-related endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerService service) : ControllerBase
    {
        private readonly ICustomerService _service = service;

        /// <summary>
        /// Returns a paginated list of customers with optional search, filtering, and sorting.
        /// </summary>
        /// <param name="search">An optional prefix to filter customers by company name.</param>
        /// <param name="city">An optional exact city name to filter by.</param>
        /// <param name="country">An optional exact country name to filter by.</param>
        /// <param name="sortBy">The field to sort by. Accepted values (case-insensitive): <c>CompanyName</c> (default), <c>OrderCount</c>.</param>
        /// <param name="sortDirection">The sort direction. Accepted values (case-insensitive): <c>asc</c> / <c>ascending</c> (default), <c>desc</c> / <c>descending</c>.</param>
        /// <param name="page">The 1-based page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 20.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>A 200 OK response with the paged customer list.</returns>
        // GET /api/customers?search=alf&city=London&country=UK&sortBy=OrderCount&sortDirection=desc&page=1&pageSize=20
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseDto<CustomerListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponseDto<CustomerListItemDto>>> GetCustomers(
            [FromQuery] string? search,
            [FromQuery] string? city,
            [FromQuery] string? country,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var parsedSortBy = SortParsing.ParseSortField(sortBy);
            var parsedSortDirection = SortParsing.ParseSortDirection(sortDirection);
            var result = await _service.GetCustomersAsync(search, city, country, parsedSortBy, parsedSortDirection, page, pageSize, ct);
            return Ok(result);
        }

        /// <summary>
        /// Returns full details for a single customer.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>200 OK with customer details; 400 if <paramref name="id"/> is blank; 404 if not found.</returns>
        // GET /api/customers/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDetailsDto>> GetCustomerById(string id, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var customer = await _service.GetCustomerByIdAsync(id, ct);
            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// Returns the order history for a specific customer.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>200 OK with the customer's orders; 400 if <paramref name="id"/> is blank; 404 if the customer does not exist.</returns>
        // GET /api/customers/{id}/orders
        [HttpGet("{id}/orders")]
        [ProducesResponseType(typeof(CustomerOrdersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerOrdersResponseDto>> GetCustomerOrders(string id, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var orders = await _service.GetCustomerOrdersAsync(id, ct);
            if (orders is null)
            {
                return NotFound();
            }

            return Ok(orders);
        }
    }
}
