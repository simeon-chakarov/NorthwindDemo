using Northwind.Contracts.Customers;

namespace NorthwindDemo.Mvc.ViewModels
{
    public sealed class CustomerIndexViewModel
    {
        public string? Search { get; init; }
        public IReadOnlyList<CustomerListItemDto> Customers { get; init; } = [];
    }
}
