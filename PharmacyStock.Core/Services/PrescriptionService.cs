using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;

namespace PharmacyStock.Core.Services;

/// <summary>
/// Provides operations for pharmacy prescriptions.
/// </summary>
public sealed partial class PrescriptionService
{
    private static readonly Func<ILogger, int, IDisposable?> PrescriptionScope =
        LoggerMessage.DefineScope<int>("Рецепт:{PrescriptionId}");

    private readonly IPrescriptionRepository _repository;
    private readonly IPricingPolicy _pricing;
    private readonly INotifier _notifier;
    private readonly ILogger<PrescriptionService> _logger;

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
        : this(
            repository,
            pricing,
            notifier,
            NullLogger<PrescriptionService>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionService"/> class.
    /// </summary>
    /// <param name="repository">Prescription repository.</param>
    /// <param name="pricing">Pricing policy.</param>
    /// <param name="notifier">Notification service.</param>
    /// <param name="logger">Application logger.</param>
    public PrescriptionService(
        IPrescriptionRepository repository,
        IPricingPolicy pricing,
        INotifier notifier,
        ILogger<PrescriptionService> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        _pricing = pricing
            ?? throw new ArgumentNullException(nameof(pricing));

        _notifier = notifier
            ?? throw new ArgumentNullException(nameof(notifier));

        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Stores a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to store.</param>
    public void Place(Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        using IDisposable? scope = PrescriptionScope(
            _logger,
            prescription.Id);

        LogPrescriptionCreated(
            _logger,
            prescription.Id);

        _repository.Add(prescription);
    }

    /// <summary>
    /// Loads a prescription by identifier.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <returns>Existing prescription.</returns>
    public Prescription Load(int prescriptionId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            prescriptionId);

        return _repository.GetById(prescriptionId);
    }

    /// <summary>
    /// Adds a validated item to a prescription.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="item">Validated medicine item.</param>
    public void AddItem(
        int prescriptionId,
        PrescriptionItem item)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            prescriptionId);

        ArgumentNullException.ThrowIfNull(item);

        using IDisposable? scope = PrescriptionScope(
            _logger,
            prescriptionId);

        LogAddStarted(
            _logger,
            prescriptionId);

        Prescription prescription = Load(prescriptionId);

        if (prescription.Status != PrescriptionStatus.New)
        {
            LogEditRejected(
                _logger,
                prescriptionId,
                prescription.Status);

            throw new DomainRuleException(
                "prescription.editable",
                "Додавати препарати можна лише до нового рецепта.");
        }

        prescription.AddItem(item);

        LogItemAdded(
            _logger,
            prescriptionId,
            item.MedicineCode,
            item.Quantity);
    }

    /// <summary>
    /// Cancels an existing prescription.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="reason">Cancellation reason.</param>
    public void Cancel(int prescriptionId, string reason)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            prescriptionId);

        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        using IDisposable? scope = PrescriptionScope(
            _logger,
            prescriptionId);

        Prescription prescription = Load(prescriptionId);

        try
        {
            prescription.Cancel();

            LogPrescriptionCancelled(
                _logger,
                prescriptionId);
        }
        catch (InvalidOperationException ex)
        {
            LogCancellationFailed(
                _logger,
                ex,
                prescriptionId);

            string message = string.Format(
                CultureInfo.InvariantCulture,
                "Рецепт № {0} не можна скасувати. Причина: {1}",
                prescriptionId,
                reason);

            throw new DomainRuleException(
                "prescription.cancellable",
                message,
                ex);
        }
    }

    /// <summary>
    /// Calculates the total of a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to calculate.</param>
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
    /// Sends a notification to a patient.
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

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Рецепт {PrescriptionId} створено")]
    private static partial void LogPrescriptionCreated(
        ILogger logger,
        int prescriptionId);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Debug,
        Message = "Початок додавання препарату до рецепта {PrescriptionId}")]
    private static partial void LogAddStarted(
        ILogger logger,
        int prescriptionId);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Information,
        Message = "До рецепта {PrescriptionId} додано {MedicineCode}, кількість {Quantity}")]
    private static partial void LogItemAdded(
        ILogger logger,
        int prescriptionId,
        string medicineCode,
        int quantity);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Warning,
        Message = "Редагування рецепта {PrescriptionId} відхилено, стан {Status}")]
    private static partial void LogEditRejected(
        ILogger logger,
        int prescriptionId,
        PrescriptionStatus status);

    [LoggerMessage(
        EventId = 1005,
        Level = LogLevel.Error,
        Message = "Не вдалося скасувати рецепт {PrescriptionId}")]
    private static partial void LogCancellationFailed(
        ILogger logger,
        Exception exception,
        int prescriptionId);

    [LoggerMessage(
        EventId = 1006,
        Level = LogLevel.Information,
        Message = "Рецепт {PrescriptionId} скасовано")]
    private static partial void LogPrescriptionCancelled(
        ILogger logger,
        int prescriptionId);
}
