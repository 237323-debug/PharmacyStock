namespace PharmacyStock.Legacy;

/// <summary>
/// Calculates shipping charges using the legacy threshold.
/// </summary>
public sealed class ShippingPolicy
{
    /// <summary>
    /// Returns the shipping charge for the amount after discount.
    /// </summary>
    public decimal For(decimal payable)
    {
        decimal shipping = 0m;
        if (payable < PricingRules.FreeShipFrom)
        {
            shipping = PricingRules.ShipCost;
        }
        return shipping;
    }
}
