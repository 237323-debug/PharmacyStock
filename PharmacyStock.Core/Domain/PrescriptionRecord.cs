namespace PharmacyStock.Core.Domain;

public class PrescriptionRecord
{
    private readonly List<string[]> _lines = new();

    public string Id { get; private set; }

    public string PatientName { get; private set; }

    public int Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public PrescriptionRecord(string id, string patientName)
    {
        Id = id;
        PatientName = patientName;
        CreatedAt = DateTime.Now;
    }

    public void AddLine(string medicineCode, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = medicineCode;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();

        _lines.Add(line);
    }

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

    public bool IsValid()
    {
        if (Id != null && Id != ""
            && PatientName != null && PatientName.Length > 2
            && _lines.Count > 0 && _lines.Count < 100
            && Status >= 0 && Status <= 3)
        {
            return true;
        }

        return false;
    }

    public string BuildReport()
    {
        string report = "";

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
}