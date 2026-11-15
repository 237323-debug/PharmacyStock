using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Pricing;

/// <summary>
/// Calculates medicine prices without discounts.
/// </summary>
public sealed class RegularPricingPolicy : IPricingPolicy
{
    /// <summary>
    /// Returns the regular item amount.
    /// </summary>
    /// <param name="item">Prescription item.</param>
    /// <returns>Regular price.</returns>
    public decimal PriceOf(PrescriptionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item.Amount;
    }
}
