using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts.Login;
using NorthwindDemo.Mvc.Services;
using NorthwindDemo.Mvc.ViewModels;

namespace NorthwindDemo.Mvc.Controllers
{
    public class AccountController(IAuthApiClient authApiClient) : Controller
    {
        private readonly IAuthApiClient _authApiClient = authApiClient;

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var response = await _authApiClient.LoginAsync(new LoginRequestDto(vm.Username, vm.Password), ct);

            if (response is null)
            {
                vm.ErrorMessage = "Invalid username or password.";
                return View(vm);
            }

            HttpContext.Session.SetString("AccessToken", response.AccessToken);

            return RedirectToAction("Index", "Customers");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AccessToken");
            return RedirectToAction("Login");
        }
    }
}
