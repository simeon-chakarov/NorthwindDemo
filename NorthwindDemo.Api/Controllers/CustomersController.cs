using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Services;

namespace NorthwindDemo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerService service) : ControllerBase
    {
        private readonly ICustomerService _service = service;

        // GET /api/customers?search=alf
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CustomerListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CustomerListItemDto>>> GetCustomers([FromQuery] string? search, CancellationToken ct)
        {
            //await Task.Delay(TimeSpan.FromSeconds(10), ct); //OperationCanceledException
            var customers = await _service.GetCustomersAsync(search, ct);
            return Ok(customers);
        }

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
