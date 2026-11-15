using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Services;

/// <summary>
/// Provides operations for pharmacy prescriptions.
/// </summary>
public sealed class PrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    private readonly IPricingPolicy _pricing;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionService"/> class.
    /// </summary>
    /// <param name="repository">Prescription repository.</param>
    /// <param name="pricing">Medicine pricing policy.</param>
    public PrescriptionService(
        IPrescriptionRepository repository,
        IPricingPolicy pricing)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        _pricing = pricing
            ?? throw new ArgumentNullException(nameof(pricing));
    }

    /// <summary>
    /// Stores a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to store.</param>
    public void Place(Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _repository.Add(prescription);
    }

    /// <summary>
    /// Calculates the total prescription amount.
    /// </summary>
    /// <param name="prescription">Prescription to calculate.</param>
    /// <returns>Total amount.</returns>
    public decimal TotalOf(Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        decimal total = 0m;

        foreach (PrescriptionItem item in prescription.Items)
        {
            total += _pricing.PriceOf(item);
        }

        return total;
    }
}
