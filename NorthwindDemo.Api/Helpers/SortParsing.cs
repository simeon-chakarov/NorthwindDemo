using NorthwindDemo.Api.Enums;

namespace NorthwindDemo.Api.Helpers
{
    /// <summary>
    /// Provides case-insensitive parsing of sort-related query string parameters into enums.
    /// </summary>
    public static class SortParsing
    {
        /// <summary>
        /// Parses a sort field string into a <see cref="CustomerSortField"/>.
        /// Accepted values (case-insensitive): <c>ordercount</c>. Anything else defaults to <see cref="CustomerSortField.CompanyName"/>.
        /// </summary>
        /// <param name="value">The raw query string value.</param>
        /// <returns>The parsed <see cref="CustomerSortField"/>.</returns>
        public static CustomerSortField ParseSortField(string? value) =>
            value?.Trim().ToLowerInvariant() switch
            {
                "ordercount" => CustomerSortField.OrderCount,
                _            => CustomerSortField.CompanyName
            };

        /// <summary>
        /// Parses a sort direction string into a <see cref="SortDirection"/>.
        /// Accepted values (case-insensitive): <c>desc</c>, <c>descending</c>. Anything else defaults to <see cref="SortDirection.Ascending"/>.
        /// </summary>
        /// <param name="value">The raw query string value.</param>
        /// <returns>The parsed <see cref="SortDirection"/>.</returns>
        public static SortDirection ParseSortDirection(string? value) =>
            value?.Trim().ToLowerInvariant() switch
            {
                "desc" or "descending" => SortDirection.Descending,
                _                      => SortDirection.Ascending
            };
    }
}
