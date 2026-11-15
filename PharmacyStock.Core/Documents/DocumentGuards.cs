using PharmacyStock.Core.Legacy;

namespace PharmacyStock.Core.Documents;

/// <summary>
/// Validates pharmacy prescription input data.
/// </summary>
public static class DocumentGuards
{
    /// <summary>
    /// Validates the prescription header.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="patientName">Patient name.</param>
    /// <param name="patientEmail">Patient email.</param>
    public static void EnsureHeaderValid(
        int prescriptionId,
        string patientName,
        string patientEmail)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            prescriptionId);

        ArgumentException.ThrowIfNullOrWhiteSpace(patientName);
        ArgumentException.ThrowIfNullOrWhiteSpace(patientEmail);

        if (!patientEmail.Contains('@'))
        {
            throw new ArgumentException(
                "Пошта має містити символ @.",
                nameof(patientEmail));
        }
    }

    /// <summary>
    /// Validates a medicine line.
    /// </summary>
    /// <param name="line">Medicine line to validate.</param>
    public static void EnsureLineValid(MedicineLine line)
    {
        ArgumentNullException.ThrowIfNull(line);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            line.MedicineCode);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            line.Quantity, nameof(line.Quantity));

        ArgumentOutOfRangeException.ThrowIfNegative(
            line.UnitPrice, nameof(line.UnitPrice));
    }

    /// <summary>
    /// Validates all medicine lines.
    /// </summary>
    /// <param name="lines">Medicine lines to validate.</param>
    public static void EnsureLinesValid(
        IReadOnlyList<MedicineLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (lines.Count == 0)
        {
            throw new ArgumentException(
                "Рецепт не містить жодного препарату.",
                nameof(lines));
        }

        foreach (MedicineLine line in lines)
        {
            EnsureLineValid(line);
        }
    }
}
