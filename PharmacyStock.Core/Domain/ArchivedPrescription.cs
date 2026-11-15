namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents an archived prescription using composition.
/// </summary>
public sealed class ArchivedPrescription
{
    private readonly Prescription _prescription;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ArchivedPrescription"/> class.
    /// </summary>
    /// <param name="prescription">Original prescription.</param>
    /// <param name="archivedAt">Archive date.</param>
    public ArchivedPrescription(
        Prescription prescription,
        DateOnly archivedAt)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _prescription = prescription;
        ArchivedAt = archivedAt;
    }

    /// <summary>Gets the archive date.</summary>
    public DateOnly ArchivedAt { get; }

    /// <summary>Gets the prescription identifier.</summary>
    public int Id => _prescription.Id;

    /// <summary>Gets the prescription status.</summary>
    public PrescriptionStatus Status => _prescription.Status;

    /// <summary>Gets the prescription items.</summary>
    public IReadOnlyList<PrescriptionItem> Items => _prescription.Items;
}
