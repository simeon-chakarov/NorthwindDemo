namespace Northwind.Contracts.Customers
{
    public sealed record CustomerDetailsDto(
        string Id,
        string CompanyName,
        string? ContactName,
        string? ContactTitle,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? Phone,
        string? Fax
    );
}
