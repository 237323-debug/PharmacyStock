using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Pricing;

/// <summary>
/// Calculates medicine prices with a discount.
/// </summary>
public sealed class DiscountPricingPolicy : IPricingPolicy
{
    private readonly decimal _discountRate;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DiscountPricingPolicy"/> class.
    /// </summary>
    /// <param name="discountRate">Discount rate.</param>
    public DiscountPricingPolicy(decimal discountRate)
    {
        if (discountRate < 0m || discountRate > 0.5m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(discountRate),
                "Знижка повинна бути від 0 до 50 відсотків.");
        }

        _discountRate = discountRate;
    }

    /// <summary>
    /// Calculates the discounted item amount.
    /// </summary>
    /// <param name="item">Prescription item.</param>
    /// <returns>Discounted price.</returns>
    public decimal PriceOf(PrescriptionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        decimal total = item.Amount * (1m - _discountRate);

        return Math.Round(total, 2);
    }
}
