namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Defines an overly large storage interface.
/// </summary>
public interface ILegacyPrescriptionStore
{
    /// <summary>
    /// Adds a prescription.
    /// </summary>
    /// <param name="prescription">Prescription to add.</param>
    void Add(LegacyPrescription prescription);

    /// <summary>
    /// Finds a prescription.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>The prescription or null.</returns>
    LegacyPrescription? GetById(int id);

    /// <summary>
    /// Saves a prescription to a file.
    /// </summary>
    /// <param name="prescription">Prescription to save.</param>
    /// <param name="path">File path.</param>
    void SaveToFile(LegacyPrescription prescription, string path);

    /// <summary>
    /// Sends an email message.
    /// </summary>
    /// <param name="recipient">Email recipient.</param>
    /// <param name="body">Message body.</param>
    void SendEmail(string recipient, string body);

    /// <summary>
    /// Exports prescriptions as CSV.
    /// </summary>
    /// <returns>CSV report.</returns>
    string ExportCsv();

    /// <summary>
    /// Creates a backup.
    /// </summary>
    /// <param name="folder">Backup directory.</param>
    void Backup(string folder);

    /// <summary>
    /// Prints an invoice.
    /// </summary>
    /// <param name="prescription">Prescription.</param>
    void PrintInvoice(LegacyPrescription prescription);
}
