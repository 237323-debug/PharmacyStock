using System.Globalization;

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
        string status,
        bool printToConsole,
        decimal deliveryPrice,
        out string newStatus)
    {
        newStatus = status;
        decimal total = 0m;

        if (prescriptionId <= 0)
        {
            Console.WriteLine("Помилка: номер рецепта");
            return -1m;
        }

        if (patientName == null || patientName == string.Empty)
        {
            Console.WriteLine("Помилка: немає пацієнта");
            return -1m;
        }

        if (patientEmail == null || !patientEmail.Contains('@'))
        {
            Console.WriteLine("Помилка: пошта пацієнта");
            return -1m;
        }

        if (lines == null || lines.Count == 0)
        {
            Console.WriteLine("Помилка: немає препаратів");
            return -1m;
        }

        if (status != "New" && status != "Approved"
            && status != "Dispensed" && status != "Cancelled")
        {
            Console.WriteLine("Помилка: невідомий стан");
            return -1m;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Quantity <= 0)
            {
                Console.WriteLine("Помилка: кількість");
                return -1m;
            }

            if (lines[i].UnitPrice < 0)
            {
                Console.WriteLine("Помилка: ціна");
                return -1m;
            }

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

        if (status == "New" && total > 0)
        {
            newStatus = "Approved";
        }
        else if (status == "Approved")
        {
            newStatus = "Dispensed";
        }
        else if (status == "Cancelled")
        {
            newStatus = "Cancelled";
            total = 0;
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