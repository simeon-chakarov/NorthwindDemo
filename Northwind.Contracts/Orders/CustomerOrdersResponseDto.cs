namespace Northwind.Contracts.Orders
{
    public sealed record CustomerOrdersResponseDto(
        string CustomerId,
        IReadOnlyList<OrderSummaryDto> Orders
    );
}
