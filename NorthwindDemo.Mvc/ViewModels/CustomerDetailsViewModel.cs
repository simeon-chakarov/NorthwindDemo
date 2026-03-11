using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;

namespace NorthwindDemo.Mvc.ViewModels
{
    /// <summary>
    /// View model for the customer detail view, combining customer info with their order history.
    /// </summary>
    public sealed class CustomerDetailsViewModel
    {
        /// <summary>Gets the full customer details.</summary>
        public CustomerDetailsDto Customer { get; init; } = default!;

        /// <summary>Gets the list of order summaries for the customer.</summary>
        public IReadOnlyList<OrderSummaryDto> Orders { get; init; } = [];
    }
}
