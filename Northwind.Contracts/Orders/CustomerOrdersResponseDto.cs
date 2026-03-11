namespace Northwind.Contracts.Orders
{
    /// <summary>
    /// Response containing the full order history for a specific customer.
    /// </summary>
    /// <param name="CustomerId">The unique identifier of the customer.</param>
    /// <param name="Orders">The list of order summaries for the customer.</param>
    public sealed record CustomerOrdersResponseDto(
        string CustomerId,
        IReadOnlyList<OrderSummaryDto> Orders
    );
}
