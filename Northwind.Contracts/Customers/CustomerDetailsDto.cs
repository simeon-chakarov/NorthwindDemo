namespace Northwind.Contracts.Customers
{
    /// <summary>
    /// Full customer details including contact information and address.
    /// </summary>
    /// <param name="Id">The unique customer identifier.</param>
    /// <param name="CompanyName">The name of the customer's company.</param>
    /// <param name="ContactName">The name of the primary contact person.</param>
    /// <param name="ContactTitle">The job title of the primary contact person.</param>
    /// <param name="Address">The street address of the customer.</param>
    /// <param name="City">The city where the customer is located.</param>
    /// <param name="Region">The region or state where the customer is located.</param>
    /// <param name="PostalCode">The postal or ZIP code of the customer's address.</param>
    /// <param name="Country">The country where the customer is located.</param>
    /// <param name="Phone">The customer's phone number.</param>
    /// <param name="Fax">The customer's fax number.</param>
    public sealed record CustomerDetailsDto(
        string Id,
        string CompanyName,
        string? ContactName,
        string? ContactTitle,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? Phone,
        string? Fax
    );
}
