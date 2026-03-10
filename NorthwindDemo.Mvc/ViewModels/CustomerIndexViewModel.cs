using Northwind.Contracts.Customers;

namespace NorthwindDemo.Mvc.ViewModels
{
    public sealed class CustomerIndexViewModel
    {
        public string? Search { get; init; }
        public PagedResponseDto<CustomerListItemDto> Page { get; init; } = new([], 1, 20, 0);
    }
}
