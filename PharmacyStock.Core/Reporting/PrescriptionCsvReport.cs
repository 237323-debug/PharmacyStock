using System.Globalization;
using System.Text;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Reporting;

/// <summary>
/// Builds CSV reports about pharmacy prescriptions.
/// </summary>
public static class PrescriptionCsvReport
{
    /// <summary>
    /// Builds a CSV report without writing files.
    /// </summary>
    /// <param name="prescriptions">Prescription list.</param>
    /// <returns>CSV report text.</returns>
    public static string BuildCsv(
        IReadOnlyList<Prescription> prescriptions)
    {
        ArgumentNullException.ThrowIfNull(prescriptions);

        StringBuilder text = new();

        text.AppendLine("id;total;status");

        foreach (Prescription prescription in prescriptions)
        {
            string row = string.Format(
                CultureInfo.InvariantCulture,
                "{0};{1:F2};{2}",
                prescription.Id,
                prescription.Total(),
                prescription.Status);

            text.AppendLine(row);
        }

        return text.ToString();
    }
}
