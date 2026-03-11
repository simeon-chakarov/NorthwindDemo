namespace Northwind.Contracts.Orders
{
    /// <summary>
    /// Summary of a single order, including totals and any fulfillment warnings.
    /// </summary>
    /// <param name="OrderId">The unique order identifier.</param>
    /// <param name="OrderDate">The date the order was placed, or <see langword="null"/> if not recorded.</param>
    /// <param name="Total">The calculated order total after discounts.</param>
    /// <param name="ProductsCount">The number of distinct products in the order.</param>
    /// <param name="FulfillmentWarning">A warning message if the order contains discontinued or low-stock products; otherwise <see langword="null"/>.</param>
    public sealed record OrderSummaryDto(
        int OrderId,
        DateTime? OrderDate,
        decimal Total,
        int ProductsCount,
        string? FulfillmentWarning
    );
}
