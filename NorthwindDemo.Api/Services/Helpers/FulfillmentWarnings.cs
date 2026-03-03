namespace NorthwindDemo.Api.Services.Helpers
{
    internal static class FulfillmentWarnings
    {
        public const string Discontinued =
            "Order contains discontinued product(s).";

        public const string StockRisk =
            "Order contains product(s) with low stock vs units on order.";

        public const string Both =
            "Order contains discontinued product(s) and product(s) with low stock vs units on order.";

        public static string? Build(bool discontinued, bool stockRisk) =>
            (discontinued, stockRisk) switch
            {
                (true, true) => Both,
                (true, false) => Discontinued,
                (false, true) => StockRisk,
                _ => null
            };
    }
}
