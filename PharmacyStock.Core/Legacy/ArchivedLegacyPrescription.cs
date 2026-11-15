namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Represents an archived prescription using incorrect inheritance.
/// </summary>
public class ArchivedLegacyPrescription : LegacyPrescription
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ArchivedLegacyPrescription"/> class.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    public ArchivedLegacyPrescription(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Rejects adding a medicine to the archived prescription.
    /// </summary>
    /// <param name="line">Medicine line.</param>
    public override void AddLine(LegacyPrescriptionLine line)
    {
        throw new NotSupportedException(
            "Архівний рецепт не можна змінювати.");
    }
}
