using System.Globalization;
using PharmacyStock.Core.Documents;

namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Processes prescriptions using the legacy algorithm.
/// </summary>
public static class LegacyPrescriptionProcessor
{
    /// <summary>
    /// Calculates the prescription amount and updates its status.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="patientName">Patient name.</param>
    /// <param name="patientEmail">Patient email.</param>
    /// <param name="isRegularPatient">Regular patient indicator.</param>
    /// <param name="lines">Medicine lines.</param>
    /// <param name="createdAt">Creation date.</param>
    /// <param name="couponCode">Discount coupon.</param>
    /// <param name="status">Current prescription status.</param>
    /// <param name="printToConsole">Console output indicator.</param>
    /// <param name="deliveryPrice">Delivery price.</param>
    /// <param name="newStatus">Resulting prescription status.</param>
    /// <returns>Calculated prescription amount.</returns>
    public static decimal Process(
        int prescriptionId,
        string patientName,
        string patientEmail,
        bool isRegularPatient,
        List<MedicineLine> lines,
        DateOnly createdAt,
        string couponCode,
        DocumentStatus status,
        bool printToConsole,
        decimal deliveryPrice,
        out DocumentStatus newStatus)
    {
        DocumentGuards.EnsureHeaderValid(
            prescriptionId, patientName, patientEmail);

        DocumentGuards.EnsureLinesValid(lines);

        ArgumentOutOfRangeException.ThrowIfNegative(
            deliveryPrice);

        newStatus = status;
        decimal total = 0m;

        for (int i = 0; i < lines.Count; i++)
        {
            decimal sum = lines[i].Quantity * lines[i].UnitPrice;

            if (lines[i].Quantity >= 10)
            {
                sum *= 0.95m;
            }

            total += sum;
        }

        if (isRegularPatient)
        {
            total *= 0.93m;
        }

        if (total > 5000)
        {
            total *= 0.9m;
        }
        else if (total > 2000)
        {
            total *= 0.95m;
        }

        if (couponCode == "SALE10")
        {
            total -= total * 0.1m;
        }
        else if (couponCode == "MINUS200" && total > 1000)
        {
            total -= 200;
        }

        if (createdAt.DayOfWeek == DayOfWeek.Sunday)
        {
            total *= 0.98m;
        }

        if (total < 1000)
        {
            total += deliveryPrice;
        }

        if (total < 0)
        {
            total = 0;
        }

        total = Math.Round(total, 2);

        if (status == DocumentStatus.New && total > 0)
        {
            newStatus = DocumentStatus.Approved;
        }
        else if (status == DocumentStatus.Approved)
        {
            newStatus = DocumentStatus.Dispensed;
        }
        else if (status == DocumentStatus.Cancelled)
        {
            newStatus = DocumentStatus.Cancelled;
            total = 0m;
        }

        if (printToConsole)
        {
            Console.WriteLine("Рецепт № " + prescriptionId);
            Console.WriteLine("Пацієнт: " + patientName);
            Console.WriteLine("Пошта: " + patientEmail);

            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(
                    lines[i].MedicineCode + " x " + lines[i].Quantity);
            }

            Console.WriteLine(
                "Разом: " + total.ToString(CultureInfo.CurrentCulture));

            Console.WriteLine("Стан: " + newStatus);
        }

        return total;
    }
}
