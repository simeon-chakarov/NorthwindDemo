using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using System.Net;

namespace NorthwindDemo.Mvc.Services
{
    public sealed class CustomerApiClient(HttpClient http) : ICustomerApiClient
    {
        private readonly HttpClient _http = http;

        public async Task<PagedResponseDto<CustomerListItemDto>> GetCustomersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var url = $"/api/customers?search={Uri.EscapeDataString(search ?? string.Empty)}&page={page}&pageSize={pageSize}";

            var data = await _http.GetFromJsonAsync<PagedResponseDto<CustomerListItemDto>>(url, ct);
            return data ?? new PagedResponseDto<CustomerListItemDto>([], page, pageSize, 0);
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
