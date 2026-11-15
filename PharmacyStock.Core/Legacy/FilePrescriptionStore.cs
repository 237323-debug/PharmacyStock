using System.Globalization;
using System.Text;

namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Provides legacy prescription storage and unrelated operations.
/// </summary>
public sealed class FilePrescriptionStore : ILegacyPrescriptionStore
{
    private readonly Dictionary<int, LegacyPrescription> _items = new();

    /// <summary>
    /// Adds a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to add.</param>
    public void Add(LegacyPrescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);
        _items.Add(prescription.Id, prescription);
    }

    /// <summary>
    /// Finds a prescription by identifier.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>The prescription or null.</returns>
    public LegacyPrescription? GetById(int id)
    {
        return _items.TryGetValue(id, out LegacyPrescription? item)
            ? item
            : null;
    }

    /// <summary>
    /// Saves a prescription to a file.
    /// </summary>
    /// <param name="prescription">Prescription to save.</param>
    /// <param name="path">File path.</param>
    public void SaveToFile(
        LegacyPrescription prescription, string path)
    {
        ArgumentNullException.ThrowIfNull(prescription);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string content = string.Format(
            CultureInfo.InvariantCulture,
            "Рецепт: {0}; сума: {1}",
            prescription.Id,
            prescription.Total);

        File.WriteAllText(path, content);
    }

    /// <summary>
    /// Simulates sending an email message.
    /// </summary>
    /// <param name="recipient">Email recipient.</param>
    /// <param name="body">Message body.</param>
    public void SendEmail(string recipient, string body)
    {
        Console.WriteLine("Лист для " + recipient + ": " + body);
    }

    /// <summary>
    /// Exports prescriptions as CSV text.
    /// </summary>
    /// <returns>CSV report.</returns>
    public string ExportCsv()
    {
        StringBuilder text = new();

        text.AppendLine("id;total;status");

        foreach (LegacyPrescription item in _items.Values)
        {
            text.AppendLine(string.Format(
                CultureInfo.InvariantCulture,
                "{0};{1};{2}",
                item.Id,
                item.Total,
                item.Status));
        }

        return text.ToString();
    }

    /// <summary>
    /// Creates a file backup.
    /// </summary>
    /// <param name="folder">Backup directory.</param>
    public void Backup(string folder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        Directory.CreateDirectory(folder);

        foreach (LegacyPrescription item in _items.Values)
        {
            string filename = string.Format(
                CultureInfo.InvariantCulture,
                "prescription-{0}.txt",
                item.Id);

            SaveToFile(item, Path.Combine(folder, filename));
        }
    }

    /// <summary>
    /// Prints a prescription invoice.
    /// </summary>
    /// <param name="prescription">Prescription.</param>
    public void PrintInvoice(LegacyPrescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        Console.WriteLine(string.Format(
            CultureInfo.InvariantCulture,
            "Рецепт {0}: {1}",
            prescription.Id,
            prescription.Total));
    }
}
