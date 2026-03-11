namespace NorthwindDemo.Api.Enums
{
    /// <summary>
    /// Specifies the field by which the customer list can be sorted.
    /// </summary>
    public enum CustomerSortField
    {
        /// <summary>Sort by company name (default).</summary>
        CompanyName,

        /// <summary>Sort by total number of orders placed.</summary>
        OrderCount
    }
}
