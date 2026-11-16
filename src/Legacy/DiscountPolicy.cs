namespace PharmacyStock.Legacy;

/// <summary>
/// Calculates discounts using the legacy pricing rules.
/// </summary>
public sealed class DiscountPolicy
{
    /// <summary>
    /// Returns the legacy discount for a patient category.
    /// </summary>
    public decimal For(decimal subtotal, ClientKind kind, int clientDone)
    {
        decimal discount = 0m;
        if (kind == ClientKind.Vip)
        {
            discount = subtotal * PricingRules.VipRate;
            if (discount > PricingRules.MaxDiscount) discount = PricingRules.MaxDiscount;
        }
        else if (kind == ClientKind.Staff)
        {
            discount = subtotal * PricingRules.StaffRate;
            if (discount > PricingRules.MaxDiscount) discount = PricingRules.MaxDiscount;
        }
        else if (clientDone > PricingRules.LoyalOrders)
        {
            discount = subtotal * PricingRules.LoyalRate;
            if (discount > PricingRules.MaxDiscount) discount = PricingRules.MaxDiscount;
        }
        return discount;
    }
}
