namespace Northwind.Contracts.Customers
{
    /// <summary>
    /// Generic wrapper for a paginated list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the page.</typeparam>
    /// <param name="Items">The items on the current page.</param>
    /// <param name="Page">The current page number (1-based).</param>
    /// <param name="PageSize">The maximum number of items per page.</param>
    /// <param name="TotalCount">The total number of items across all pages.</param>
    public sealed record PagedResponseDto<T>(
        IReadOnlyList<T> Items,
        int Page,
        int PageSize,
        int TotalCount
    )
    {
        /// <summary>Gets the total number of pages.</summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>Gets a value indicating whether a previous page exists.</summary>
        public bool HasPrevious => Page > 1;

        /// <summary>Gets a value indicating whether a next page exists.</summary>
        public bool HasNext => Page < TotalPages;
    }
}
