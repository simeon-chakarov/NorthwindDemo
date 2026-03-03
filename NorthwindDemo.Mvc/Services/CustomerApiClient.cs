using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using System.Net;

namespace NorthwindDemo.Mvc.Services
{
    public sealed class CustomerApiClient(HttpClient http) : ICustomerApiClient
    {
        private readonly HttpClient _http = http;

        public async Task<IReadOnlyList<CustomerListItemDto>> GetCustomersAsync(string? search, CancellationToken ct = default)
        {
            var url = string.IsNullOrWhiteSpace(search)
                ? "/api/customers"
                : $"/api/customers?search={Uri.EscapeDataString(search)}";

            var data = await _http.GetFromJsonAsync<List<CustomerListItemDto>>(url, ct);
            return data ?? [];
        }

        public async Task<CustomerDetailsDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"/api/customers/{Uri.EscapeDataString(id)}", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<CustomerDetailsDto>(cancellationToken: ct);
        }

        public async Task<CustomerOrdersResponseDto?> GetCustomerOrdersAsync(string id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"/api/customers/{Uri.EscapeDataString(id)}/orders", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<CustomerOrdersResponseDto>(cancellationToken: ct);
        }
    }
}
