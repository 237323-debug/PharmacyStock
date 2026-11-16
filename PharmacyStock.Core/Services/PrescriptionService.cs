using System.Globalization;
using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;

namespace PharmacyStock.Core.Services;

/// <summary>
/// Provides operations for pharmacy prescriptions.
/// </summary>
public sealed class PrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    private readonly IPricingPolicy _pricing;
    private readonly INotifier _notifier;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionService"/> class.
    /// </summary>
    /// <param name="repository">Prescription repository.</param>
    /// <param name="pricing">Pricing policy.</param>
    /// <param name="notifier">Notification service.</param>
    public PrescriptionService(
        IPrescriptionRepository repository,
        IPricingPolicy pricing,
        INotifier notifier)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        _pricing = pricing
            ?? throw new ArgumentNullException(nameof(pricing));

        _notifier = notifier
            ?? throw new ArgumentNullException(nameof(notifier));
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
    /// Adds a validated item to an existing prescription.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="item">Validated medicine item.</param>
    public void AddItem(int prescriptionId, PrescriptionItem item)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(prescriptionId);
        ArgumentNullException.ThrowIfNull(item);

        Prescription prescription = _repository.GetById(prescriptionId);

        if (prescription.Status != PrescriptionStatus.New)
        {
            throw new DomainRuleException(
                "prescription.editable",
                "Додавати препарати можна лише до нового рецепта.");
        }

        prescription.AddItem(item);
    }

    /// <summary>
    /// Calculates prescription total using the pricing policy.
    /// </summary>
    /// <param name="prescription">Prescription.</param>
    /// <returns>Calculated total.</returns>
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

    /// <summary>
    /// Notifies a patient about a prescription.
    /// </summary>
    /// <param name="prescription">Prescription.</param>
    /// <param name="recipient">Patient email.</param>
    public void NotifyPatient(
        Prescription prescription,
        string recipient)
    {
        ArgumentNullException.ThrowIfNull(prescription);
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);

        string message = string.Format(
            CultureInfo.InvariantCulture,
            "Рецепт № {0} збережено.",
            prescription.Id);

        _notifier.Notify(recipient, message);
    }
}
