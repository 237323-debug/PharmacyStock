using System.Text;
using PharmacyStock.Core.Documents;

Console.OutputEncoding = Encoding.UTF8;

TotalRequest request = new(
    DocumentId: 1,
    Client: new ClientInfo(
        "Іваненко", "i@ex.com", true),
    Lines: new List<MedicineLine>
    {
        new()
        {
            MedicineCode = "MED-1",
            Quantity = 12,
            UnitPrice = 100m,
        },
        new()
        {
            MedicineCode = "MED-2",
            Quantity = 1,
            UnitPrice = 250m,
        },
    },
    CreatedAt: new DateOnly(2026, 3, 10),
    CouponCode: null,
    Status: DocumentStatus.New,
    DeliveryPrice: 60m);

TotalResult result =
    DocumentTotalCalculator.Calculate(request);

Console.WriteLine(result.Total);
Console.WriteLine(result.Status);
