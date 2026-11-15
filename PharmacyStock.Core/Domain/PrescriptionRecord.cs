using System.Globalization;

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

    private readonly List<string[]> _lines = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PrescriptionRecord"/> class.
    /// </summary>
    /// <param name="id">The prescription identifier.</param>
    /// <param name="patientName">The patient name.</param>
    public PrescriptionRecord(string id, string patientName)
    {
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

    /// <summary>Gets the prescription creation date.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Finds a prescription by its identifier.
    /// </summary>
    /// <param name="prescriptions">The prescription list.</param>
    /// <param name="prescriptionId">The identifier to find.</param>
    /// <returns>The prescription or null if not found.</returns>
    public static PrescriptionRecord? FindById(
        List<PrescriptionRecord> prescriptions, string prescriptionId)
    {
        for (int i = 0; i < prescriptions.Count; i++)
        {
            if (prescriptions[i].Id == prescriptionId)
            {
                return prescriptions[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a medicine to the prescription.
    /// </summary>
    /// <param name="medicineCode">The medicine code.</param>
    /// <param name="quantity">The medicine quantity.</param>
    /// <param name="unitPrice">The medicine unit price.</param>
    public void AddLine(string medicineCode, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];

        line[0] = medicineCode;
        line[1] = quantity.ToString(CultureInfo.CurrentCulture);
        line[2] = unitPrice.ToString(CultureInfo.CurrentCulture);

        _lines.Add(line);
    }

    /// <summary>
    /// Calculates the total prescription amount.
    /// </summary>
    /// <param name="isRegularPatient">Indicates a regular patient.</param>
    /// <returns>The total amount.</returns>
    public decimal CalculateTotal(bool isRegularPatient)
    {
        decimal total = 0m;
        int lineCount = 0;

        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(
                _lines[i][1], CultureInfo.CurrentCulture);

            decimal unitPrice = decimal.Parse(
                _lines[i][2], CultureInfo.CurrentCulture);

            total += quantity * unitPrice;
            lineCount++;
        }

        if (isRegularPatient && total > RegularDiscountThreshold)
        {
            total *= RegularDiscountFactor;
        }
        else if (total > LargeOrderThreshold)
        {
            total *= LargeOrderDiscountFactor;
        }

        if (lineCount > BulkLineCount)
        {
            total -= BulkDiscountAmount;
        }

        if (total < 0)
        {
            total = 0;
        }

        // VAT is calculated after all discounts.
        total += total * VatRate;

        return Math.Round(total, 2);
    }

    /// <summary>
    /// Changes the prescription status if the transition is allowed.
    /// </summary>
    /// <param name="newStatus">The new status.</param>
    /// <returns>
    /// True if the status changed; otherwise, false.
    /// </returns>
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
    /// <returns>True if the prescription is valid; otherwise, false.</returns>
    public bool IsValid()
    {
        if (Id != null && Id != string.Empty
            && PatientName != null
            && PatientName.Length > MinPatientNameLength
            && _lines.Count > 0
            && _lines.Count < MaxLineCount
            && Status >= StatusNew
            && Status <= StatusCancelled)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Builds a text report for the prescription.
    /// </summary>
    /// <returns>The prescription report.</returns>
    public string BuildReport()
    {
        string report = string.Empty;

        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(
                _lines[i][1], CultureInfo.CurrentCulture);

            decimal unitPrice = decimal.Parse(
                _lines[i][2], CultureInfo.CurrentCulture);

            report += "Ліки: " + _lines[i][0]
                + "; кількість: " + _lines[i][1]
                + "; ціна: " + _lines[i][2]
                + "; сума: " + (quantity * unitPrice) + "\n";
        }

        report += "Разом: " + CalculateTotal(false) + "\n";

        return report;
    }
}
