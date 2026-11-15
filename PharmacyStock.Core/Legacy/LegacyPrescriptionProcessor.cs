using System.Globalization;
using PharmacyStock.Core.Documents;

namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Processes prescriptions using the legacy interface.
/// </summary>
public static class LegacyPrescriptionProcessor
{
    /// <summary>
    /// Calculates the prescription total using separate subprograms.
    /// </summary>
    /// <param name="prescriptionId">Prescription identifier.</param>
    /// <param name="patientName">Patient name.</param>
    /// <param name="patientEmail">Patient email.</param>
    /// <param name="isRegularPatient">Regular patient indicator.</param>
    /// <param name="lines">Medicine lines.</param>
    /// <param name="createdAt">Creation date.</param>
    /// <param name="couponCode">Discount coupon.</param>
    /// <param name="status">Current status.</param>
    /// <param name="printToConsole">Console output indicator.</param>
    /// <param name="deliveryPrice">Delivery price.</param>
    /// <param name="newStatus">Resulting status.</param>
    /// <returns>The calculated total.</returns>
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

        decimal total = DocumentTotalCalculator.SubtotalOf(lines);

        total -= DocumentTotalCalculator.ClientDiscountOf(
            total, isRegularPatient);

        total -= DocumentTotalCalculator.VolumeDiscountOf(total);

        total -= DocumentTotalCalculator.CouponDiscountOf(
            total, couponCode);

        total -= DocumentTotalCalculator.WeekendDiscountOf(
            total, createdAt);

        total += DocumentTotalCalculator.DeliveryFeeOf(
            total, deliveryPrice);

        total = status == DocumentStatus.Cancelled
            ? 0m
            : Math.Round(Math.Max(total, 0m), 2);

        newStatus = DocumentTotalCalculator.NextStatusOf(
            status, total);

        if (printToConsole)
        {
            Console.WriteLine("Рецепт № " + prescriptionId);
            Console.WriteLine("Пацієнт: " + patientName);
            Console.WriteLine("Пошта: " + patientEmail);

            foreach (MedicineLine line in lines)
            {
                Console.WriteLine(
                    line.MedicineCode + " x " + line.Quantity);
            }

            Console.WriteLine(
                "Разом: " + total.ToString(CultureInfo.CurrentCulture));

            Console.WriteLine("Стан: " + newStatus);
        }

        return total;
    }
}
