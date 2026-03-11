using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Mvc.Services;
using NorthwindDemo.Mvc.ViewModels;

namespace NorthwindDemo.Mvc.Controllers
{
    /// <summary>
    /// MVC controller for customer list and detail views.
    /// </summary>
    public class CustomersController(ICustomerApiClient api) : Controller
    {
        private readonly ICustomerApiClient _api = api;

        /// <summary>
        /// Displays a paginated, searchable list of customers.
        /// </summary>
        /// <param name="search">An optional prefix to filter customers by company name.</param>
        /// <param name="page">The 1-based page number to display. Defaults to 1.</param>
        /// <param name="pageSize">The number of customers per page. Defaults to 20.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer list view, or the ApiUnavailable view if the API cannot be reached.</returns>
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 20, CancellationToken ct = default)
        {
            try
            {
                var result = await _api.GetCustomersAsync(search, page, pageSize, ct);

                var vm = new CustomerIndexViewModel
                {
                    Search = search,
                    Page = result
                };

                return View(vm);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return new StatusCodeResult(499);
            }
            catch (TaskCanceledException)
            {
                return View("ApiUnavailable");
            }
            catch (HttpRequestException)
            {
                return View("ApiUnavailable");
            }
        }

        /// <summary>
        /// Displays full details and order history for a single customer.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The customer detail view; 400 if <paramref name="id"/> is blank; 404 if not found; or the ApiUnavailable view if the API cannot be reached.</returns>
        public async Task<IActionResult> Details(string id, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest();
                }

                var customer = await _api.GetCustomerByIdAsync(id, ct);
                if (customer is null)
                {
                    return NotFound();
                }

                var orders = await _api.GetCustomerOrdersAsync(id, ct);

                var vm = new CustomerDetailsViewModel
                {
                    Customer = customer,
                    Orders = orders?.Orders ?? []
                };

                return View(vm);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return new StatusCodeResult(499);
            }
            catch (TaskCanceledException)
            {
                return View("ApiUnavailable");
            }
            catch (HttpRequestException)
            {
                return View("ApiUnavailable");
            }
        }
    }
}
