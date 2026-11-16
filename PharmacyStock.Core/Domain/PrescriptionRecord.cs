using System.Globalization;
using System.Text;

namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a pharmacy prescription record.
/// </summary>
public class PrescriptionRecord
{
    private const decimal VatRate = 0.2m;
    private const decimal RegularDiscountThreshold = 1000m;
    private const decimal RegularDiscountFactor = 0.9m;
    private const decimal LargeOrderThreshold = 5000m;
    private const decimal LargeOrderDiscountFactor = 0.85m;
    private const int BulkLineCount = 10;
    private const decimal BulkDiscountAmount = 100m;
    private const int MinPatientNameLength = 2;
    private const int MaxLineCount = 100;
    private const int StatusNew = 0;
    private const int StatusApproved = 1;
    private const int StatusDispensed = 2;
    private const int StatusCancelled = 3;

    private readonly List<PrescriptionItem> _lines = new();

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionRecord"/> class.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <param name="patientName">Patient name.</param>
    public PrescriptionRecord(string id, string patientName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(patientName);

        Id = id;
        PatientName = patientName;
        CreatedAt = DateTime.Now;
    }

    /// <summary>Gets the prescription identifier.</summary>
    public string Id { get; private set; }

    /// <summary>Gets the patient name.</summary>
    public string PatientName { get; private set; }

    /// <summary>Gets the prescription status.</summary>
    public int Status { get; private set; }

    /// <summary>Gets the creation date.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Finds a prescription by identifier.
    /// </summary>
    /// <param name="prescriptions">Prescription list.</param>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <returns>Prescription or null if not found.</returns>
    public static PrescriptionRecord? FindById(
        List<PrescriptionRecord> prescriptions,
        string prescriptionId)
    {
        ArgumentNullException.ThrowIfNull(prescriptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(prescriptionId);

        foreach (PrescriptionRecord prescription in prescriptions)
        {
            if (prescription.Id == prescriptionId)
            {
                return prescription;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a validated medicine item.
    /// </summary>
    /// <param name="medicineCode">Medicine code.</param>
    /// <param name="quantity">Medicine quantity.</param>
    /// <param name="unitPrice">Medicine unit price.</param>
    public void AddLine(
        string medicineCode,
        int quantity,
        decimal unitPrice)
    {
        PrescriptionItem item = new(
            medicineCode,
            quantity,
            unitPrice);

        _lines.Add(item);
    }

    /// <summary>
    /// Calculates the total prescription amount.
    /// </summary>
    /// <param name="isRegularPatient">Regular patient indicator.</param>
    /// <returns>Calculated amount.</returns>
    public decimal CalculateTotal(bool isRegularPatient)
    {
        decimal total = 0m;

        foreach (PrescriptionItem item in _lines)
        {
            total += item.Amount;
        }

        if (isRegularPatient && total > RegularDiscountThreshold)
        {
            total *= RegularDiscountFactor;
        }
        else if (total > LargeOrderThreshold)
        {
            total *= LargeOrderDiscountFactor;
        }

        if (_lines.Count > BulkLineCount)
        {
            total -= BulkDiscountAmount;
        }

        if (total < 0m)
        {
            total = 0m;
        }

        total += total * VatRate;

        return Math.Round(total, 2);
    }

    /// <summary>
    /// Changes the prescription status when allowed.
    /// </summary>
    /// <param name="newStatus">New status.</param>
    /// <returns>True if changed; otherwise, false.</returns>
    public bool TryChangeStatus(int newStatus)
    {
        if (Status == StatusNew && newStatus == StatusApproved)
        {
            Status = StatusApproved;
            return true;
        }

        if (Status == StatusApproved && newStatus == StatusDispensed)
        {
            Status = StatusDispensed;
            return true;
        }

        if (Status == StatusNew && newStatus == StatusCancelled)
        {
            Status = StatusCancelled;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the prescription data is valid.
    /// </summary>
    /// <returns>True if valid; otherwise, false.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Id)
            && !string.IsNullOrWhiteSpace(PatientName)
            && PatientName.Length > MinPatientNameLength
            && _lines.Count > 0
            && _lines.Count < MaxLineCount
            && Status >= StatusNew
            && Status <= StatusCancelled;
    }

    /// <summary>
    /// Builds a text report for the prescription.
    /// </summary>
    /// <returns>Prescription report text.</returns>
    public string BuildReport()
    {
        StringBuilder report = new();

        foreach (PrescriptionItem item in _lines)
        {
            string row = string.Format(
                CultureInfo.CurrentCulture,
                "Ліки: {0}; кількість: {1}; ціна: {2}; сума: {3}",
                item.MedicineCode,
                item.Quantity,
                item.UnitPrice,
                item.Amount);

            report.AppendLine(row);
        }

        string totalLine = string.Format(
            CultureInfo.CurrentCulture,
            "Разом: {0}",
            CalculateTotal(false));

        report.AppendLine(totalLine);

        return report.ToString();
    }
}
