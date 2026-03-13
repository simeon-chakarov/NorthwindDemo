namespace Northwind.Contracts.Login
{
    public sealed record LoginRequestDto(
        string Username,
        string Password
    );
}
