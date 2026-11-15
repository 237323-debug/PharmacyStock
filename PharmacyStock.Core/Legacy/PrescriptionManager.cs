using System.Globalization;
using System.Text;

namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Demonstrates a God class with SOLID violations.
/// </summary>
public sealed class PrescriptionManager
{
    private readonly FilePrescriptionStore _store = new();
    private readonly SmtpNotifier _notifier = new();

    /// <summary>
    /// Builds and saves a CSV report.
    /// </summary>
    /// <param name="prescriptions">Prescription list.</param>
    /// <returns>CSV report.</returns>
    public static string BuildCsvReport(
        IReadOnlyList<LegacyPrescription> prescriptions)
    {
        ArgumentNullException.ThrowIfNull(prescriptions);

        StringBuilder text = new();

        text.AppendLine("id;total;status");

        foreach (LegacyPrescription prescription in prescriptions)
        {
            text.AppendLine(string.Format(
                CultureInfo.InvariantCulture,
                "{0};{1};{2}",
                prescription.Id,
                prescription.Total,
                prescription.Status));
        }

        File.WriteAllText("report.csv", text.ToString());
        Console.WriteLine("Звіт збережено");

        return text.ToString();
    }

    /// <summary>
    /// Calculates, saves and announces a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to process.</param>
    /// <param name="patientType">Patient category.</param>
    /// <returns>Calculated total.</returns>
    public decimal Place(
        LegacyPrescription prescription, string patientType)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        decimal total = 0m;

        foreach (LegacyPrescriptionLine line in prescription.Lines)
        {
            total += line.UnitPrice * line.Quantity;
        }

        if (patientType == "regular")
        {
            total *= 0.95m;
        }
        else if (patientType == "vip")
        {
            total *= 0.90m;
        }
        else if (patientType == "staff")
        {
            total *= 0.70m;
        }

        if (total > 1000m)
        {
            total -= 50m;
        }

        prescription.Total = total;
        prescription.Status = 1;

        _store.SaveToFile(prescription, "prescriptions.json");

        string subject = "Рецепт " + prescription.Id.ToString(
            CultureInfo.InvariantCulture);

        string body = total.ToString(CultureInfo.InvariantCulture);

        _notifier.Send(prescription.Email, subject, body);

        return total;
    }

    /// <summary>
    /// Attempts to archive a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to archive.</param>
    public void Archive(LegacyPrescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        LegacyPrescription archived =
            new ArchivedLegacyPrescription(prescription.Id);

        foreach (LegacyPrescriptionLine line in prescription.Lines)
        {
            archived.AddLine(line);
        }

        _store.SaveToFile(archived, "archive.json");
    }
}
