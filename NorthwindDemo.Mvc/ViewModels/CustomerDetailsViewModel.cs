using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Mvc.ViewModels
{
    public sealed class CustomerDetailsViewModel
    {
        public CustomerDetailsDto Customer { get; init; } = default!;
        public IReadOnlyList<OrderSummaryDto> Orders { get; init; } = [];
    }
}
