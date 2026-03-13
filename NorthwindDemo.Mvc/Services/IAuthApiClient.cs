using Northwind.Contracts.Login;

namespace NorthwindDemo.Mvc.Services
{
    public interface IAuthApiClient
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
    }
}
