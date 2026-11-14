using System.Globalization;
using PharmacyStock.Core.Domain;

Prescription prescription = new()
{
    Id = 1,
    PatientId = 10,
    CreatedAt = DateTimeOffset.Now,
};

prescription.AddItem(new PrescriptionItem
{
    MedicineCode = "MED-001",
    Quantity = 2,
    UnitPrice = 120.50m,
});

prescription.AddItem(new PrescriptionItem
{
    MedicineCode = "MED-002",
    Quantity = 1,
    UnitPrice = 89.90m,
});

string total = prescription.Total()
    .ToString("F2", CultureInfo.InvariantCulture);

Console.WriteLine($"Рецепт №{prescription.Id}");
Console.WriteLine($"Стан: {prescription.Status}");
Console.WriteLine($"Препаратів: {prescription.Items.Count}");
Console.WriteLine($"Сума: {total}");
