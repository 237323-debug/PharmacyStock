using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Pricing;

/// <summary>
/// Provides VIP patient prices with a ten percent discount.
/// </summary>
public sealed class VipPricingPolicy : IPricingPolicy
{
    /// <summary>
    /// Calculates the VIP item price.
    /// </summary>
    /// <param name="item">Prescription item.</param>
    /// <returns>Discounted amount.</returns>
    public decimal PriceOf(PrescriptionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return Math.Round(item.Amount * 0.90m, 2);
    }
}
