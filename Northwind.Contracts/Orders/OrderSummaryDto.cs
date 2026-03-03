namespace Northwind.Contracts.Orders
{
    public sealed record OrderSummaryDto(
        int OrderId,
        DateTime? OrderDate,
        decimal Total,
        int ProductsCount,
        string? FulfillmentWarning
    );
}
