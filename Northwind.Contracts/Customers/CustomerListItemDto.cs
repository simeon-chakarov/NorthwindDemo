namespace Northwind.Contracts.Customers
{
    /// <summary>
    /// Lightweight DTO representing a single customer in a paginated list.
    /// </summary>
    /// <param name="Id">The unique customer identifier.</param>
    /// <param name="CompanyName">The name of the customer's company.</param>
    /// <param name="OrdersCount">The total number of orders placed by the customer.</param>
    public sealed record CustomerListItemDto(
       string Id,
       string CompanyName,
       int OrdersCount
    );
}
