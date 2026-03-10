namespace Northwind.Contracts.Customers
{
    public sealed record PagedResponseDto<T>(
        IReadOnlyList<T> Items,
        int Page,
        int PageSize,
        int TotalCount
    )
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
