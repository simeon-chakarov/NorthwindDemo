using Northwind.Contracts.Login;
using System.Net;

namespace NorthwindDemo.Mvc.Services
{
    public sealed class AuthApiClient(HttpClient http) : IAuthApiClient
    {
        private readonly HttpClient _http = http;

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", request, ct);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: ct);
        }
    }
}
