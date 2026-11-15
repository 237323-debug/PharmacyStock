using System.Globalization;
using System.Text;
using PharmacyStock.Core.Documents;

namespace PharmacyStock.Core.Reporting;

/// <summary>
/// Builds a text report for a pharmacy prescription.
/// </summary>
public static class DocumentReport
{
    /// <summary>
    /// Creates a formatted prescription report.
    /// </summary>
    /// <param name="request">Prescription request.</param>
    /// <param name="result">Calculated prescription result.</param>
    /// <returns>Formatted report text.</returns>
    public static string TextOf(
        TotalRequest request, TotalResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(request.Client);
        ArgumentNullException.ThrowIfNull(request.Lines);

        CultureInfo culture = CultureInfo.InvariantCulture;
        StringBuilder text = new();

        text.AppendLine(string.Format(
            culture, "Рецепт № {0}", request.DocumentId));

        text.AppendLine("Пацієнт: " + request.Client.Name);
        text.AppendLine("Пошта: " + request.Client.Email);

        foreach (MedicineLine line in request.Lines)
        {
            text.AppendLine(string.Format(
                culture,
                "{0} x {1}",
                line.MedicineCode,
                line.Quantity));
        }

        text.AppendLine(string.Format(
            culture, "Разом: {0:F2}", result.Total));

        text.AppendLine("Стан: " + result.Status);

        return text.ToString();
    }
}
