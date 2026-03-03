using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Mvc.Services;
using NorthwindDemo.Mvc.ViewModels;

namespace NorthwindDemo.Mvc.Controllers
{
    public class CustomersController(ICustomerApiClient api) : Controller
    {
        private readonly ICustomerApiClient _api = api;

        public async Task<IActionResult> Index(string? search, CancellationToken ct)
        {
            try
            {
                var customers = await _api.GetCustomersAsync(search, ct);

                var vm = new CustomerIndexViewModel
                {
                    Search = search,
                    Customers = customers
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
