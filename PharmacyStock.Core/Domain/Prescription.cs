namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a patient's prescription.
/// </summary>
public sealed class Prescription
{
    private readonly List<PrescriptionItem> _items = new();

    /// <summary>Gets the prescription identifier.</summary>
    public int Id { get; init; }

    /// <summary>Gets the patient identifier.</summary>
    public int PatientId { get; init; }

    /// <summary>Gets or sets the prescription status.</summary>
    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.New;

    /// <summary>Gets the prescription creation date.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Gets the prescription items.</summary>
    public IReadOnlyList<PrescriptionItem> Items => _items;

    /// <summary>
    /// Adds a medicine item to the prescription.
    /// </summary>
    /// <param name="item">The medicine item.</param>
    public void AddItem(PrescriptionItem item)
    {
        _items.Add(item);
    }

    /// <summary>
    /// Calculates the total prescription amount.
    /// </summary>
    /// <returns>The total amount.</returns>
    public decimal Total()
    {
        decimal sum = 0m;

        foreach (PrescriptionItem item in _items)
        {
            sum += item.Amount;
        }

        return sum;
    }
}
