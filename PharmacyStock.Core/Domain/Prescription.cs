namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a pharmacy prescription.
/// </summary>
public sealed class Prescription
{
    private readonly List<PrescriptionItem> _items = new();

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="Prescription"/> class.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <param name="patientId">Patient identifier.</param>
    /// <param name="createdAt">Prescription creation date.</param>
    public Prescription(
        int id,
        int patientId,
        DateTimeOffset createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(patientId);

        Id = id;
        PatientId = patientId;
        CreatedAt = createdAt;
        Status = PrescriptionStatus.New;
    }

    /// <summary>Gets the prescription identifier.</summary>
    public int Id { get; }

    /// <summary>Gets the patient identifier.</summary>
    public int PatientId { get; }

    /// <summary>Gets the prescription creation date.</summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>Gets the prescription status.</summary>
    public PrescriptionStatus Status { get; private set; }

    /// <summary>Gets the prescription items.</summary>
    public IReadOnlyList<PrescriptionItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Adds a medicine item to a new prescription.
    /// </summary>
    /// <param name="item">Medicine item to add.</param>
    public void AddItem(PrescriptionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Status != PrescriptionStatus.New)
        {
            throw new InvalidOperationException(
                "Додавати препарати можна лише до нового рецепта.");
        }

        _items.Add(item);
    }

    /// <summary>
    /// Approves a prescription that contains medicine items.
    /// </summary>
    public void Approve()
    {
        if (Status != PrescriptionStatus.New)
        {
            throw new InvalidOperationException(
                "Підтвердити можна лише новий рецепт.");
        }

        if (_items.Count == 0)
        {
            throw new InvalidOperationException(
                "Не можна підтвердити порожній рецепт.");
        }

        Status = PrescriptionStatus.Approved;
    }

    /// <summary>
    /// Marks an approved prescription as dispensed.
    /// </summary>
    public void Dispense()
    {
        if (Status != PrescriptionStatus.Approved)
        {
            throw new InvalidOperationException(
                "Видати препарати можна лише за підтвердженим рецептом.");
        }

        Status = PrescriptionStatus.Dispensed;
    }

    /// <summary>
    /// Cancels a new or approved prescription.
    /// </summary>
    public void Cancel()
    {
        if (Status != PrescriptionStatus.New
            && Status != PrescriptionStatus.Approved)
        {
            throw new InvalidOperationException(
                "Цей рецепт уже не можна скасувати.");
        }

        Status = PrescriptionStatus.Cancelled;
    }

    /// <summary>
    /// Calculates the total amount of the prescription.
    /// </summary>
    /// <returns>The total amount.</returns>
    public decimal Total()
    {
        decimal total = 0m;

        foreach (PrescriptionItem item in _items)
        {
            total += item.Amount;
        }

        return total;
    }
}
