using Northwind.Contracts.Customers;

namespace NorthwindDemo.Mvc.ViewModels
{
    /// <summary>
    /// View model for the customer list (Index) view.
    /// </summary>
    public sealed class CustomerIndexViewModel
    {
        /// <summary>Gets the current search string, or <see langword="null"/> if no filter is applied.</summary>
        public string? Search { get; init; }

        /// <summary>Gets the current page of customer results.</summary>
        public PagedResponseDto<CustomerListItemDto> Page { get; init; } = new([], 1, 20, 0);
    }
}
