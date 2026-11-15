using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Abstractions;

/// <summary>
/// Defines a medicine pricing policy.
/// </summary>
public interface IPricingPolicy
{
    /// <summary>
    /// Calculates the price of a prescription item.
    /// </summary>
    /// <param name="item">Prescription item.</param>
    /// <returns>Calculated price.</returns>
    decimal PriceOf(PrescriptionItem item);
}
