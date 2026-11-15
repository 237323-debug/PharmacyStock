namespace PharmacyStock.Core.Domain;

/// <summary>
/// Клас для роботи з рецептами в аптеці.
/// </summary>
public class PrescriptionRecord
{
    private readonly List<string[]> _lines = new();

    /// <summary>
    /// Створює новий рецепт.
    /// </summary>
    /// <param name="id">Номер рецепта.</param>
    /// <param name="patientName">Ім'я пацієнта.</param>
    public PrescriptionRecord(string id, string patientName)
    {
        Id = id;
        PatientName = patientName;
        CreatedAt = DateTime.Now;
    }

    /// <summary>Номер рецепта.</summary>
    public string Id { get; private set; }

    /// <summary>Ім'я пацієнта.</summary>
    public string PatientName { get; private set; }

    /// <summary>Поточний стан рецепта.</summary>
    public int Status { get; private set; }

    /// <summary>Дата створення рецепта.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Шукає рецепт за його номером.
    /// </summary>
    /// <param name="prescriptions">Список рецептів.</param>
    /// <param name="prescriptionId">Номер для пошуку.</param>
    /// <returns>Знайдений рецепт або null.</returns>
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
    /// Додає лікарський препарат до рецепта.
    /// </summary>
    /// <param name="medicineCode">Код препарату.</param>
    /// <param name="quantity">Кількість.</param>
    /// <param name="unitPrice">Ціна препарату.</param>
    public void AddLine(string medicineCode, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = medicineCode;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();

        _lines.Add(line);
    }

    /// <summary>
    /// Обчислює загальну вартість препаратів.
    /// </summary>
    /// <param name="isRegularPatient">Ознака постійного пацієнта.</param>
    /// <returns>Загальна сума до сплати.</returns>
    public decimal CalculateTotal(bool isRegularPatient)
    {
        decimal total = 0m;
        int lineCount = 0;

        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(_lines[i][1]);
            decimal unitPrice = decimal.Parse(_lines[i][2]);

            total += quantity * unitPrice;
            lineCount++;
        }

        if (isRegularPatient && total > 1000)
        {
            total *= 0.9m;
        }
        else if (total > 5000)
        {
            total *= 0.85m;
        }

        if (lineCount > 10)
        {
            total -= 100;
        }

        if (total < 0)
        {
            total = 0;
        }

        total += total * 0.2m;
        return Math.Round(total, 2);
    }

    /// <summary>
    /// Змінює стан рецепта.
    /// </summary>
    /// <param name="newStatus">Новий стан.</param>
    /// <returns>True, якщо стан змінено, інакше false.</returns>
    public bool TryChangeStatus(int newStatus)
    {
        if (Status == 0 && newStatus == 1)
        {
            Status = 1;
            return true;
        }

        if (Status == 1 && newStatus == 2)
        {
            Status = 2;
            return true;
        }

        if (Status == 0 && newStatus == 3)
        {
            Status = 3;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Перевіряє правильність даних рецепта.
    /// </summary>
    /// <returns>Результат перевірки рецепта.</returns>
    public bool IsValid()
    {
        if (Id != null && Id != string.Empty
            && PatientName != null && PatientName.Length > 2
            && _lines.Count > 0 && _lines.Count < 100
            && Status >= 0 && Status <= 3)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Формує текстовий звіт про рецепт.
    /// </summary>
    /// <returns>Текст звіту.</returns>
    public string BuildReport()
    {
        string report = string.Empty;

        for (int i = 0; i < _lines.Count; i++)
        {
            report += "Ліки: " + _lines[i][0]
                + "; кількість: " + _lines[i][1]
                + "; ціна: " + _lines[i][2]
                + "; сума: "
                + (int.Parse(_lines[i][1])
                * decimal.Parse(_lines[i][2])) + "\n";
        }

        report += "Разом: " + CalculateTotal(false) + "\n";
        return report;
    }
}