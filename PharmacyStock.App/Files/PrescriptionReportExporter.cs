using System.Globalization;
using System.Text;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.App.Files;

/// <summary>
/// Exports prescription information to a text file.
/// </summary>
public static class PrescriptionReportExporter
{
    /// <summary>
    /// Exports a prescription with its calculated total.
    /// </summary>
    /// <param name="path">Destination file path.</param>
    /// <param name="prescription">Prescription to export.</param>
    /// <param name="totalCalculator">Total calculation function.</param>
    public static void Export(
        string path,
        Prescription prescription,
        Func<Prescription, decimal> totalCalculator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(prescription);
        ArgumentNullException.ThrowIfNull(totalCalculator);

        string fullPath = Path.GetFullPath(path);
        string? directory = Path.GetDirectoryName(fullPath);

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        using StreamWriter writer = new(
            fullPath,
            append: false,
            encoding: new UTF8Encoding(false));

        writer.WriteLine(string.Format(
            CultureInfo.InvariantCulture,
            "Рецепт № {0}",
            prescription.Id));

        writer.WriteLine(string.Format(
            CultureInfo.InvariantCulture,
            "Стан: {0}",
            prescription.Status));

        decimal total = totalCalculator(prescription);

        writer.WriteLine(string.Format(
            CultureInfo.GetCultureInfo("uk-UA"),
            "Сума: {0:F2} грн",
            total));
    }
}
