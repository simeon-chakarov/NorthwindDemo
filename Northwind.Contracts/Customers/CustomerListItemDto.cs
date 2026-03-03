namespace Northwind.Contracts.Customers
{
    public sealed record CustomerListItemDto(
       string Id,
       string CompanyName,
       int OrdersCount
    );
}
