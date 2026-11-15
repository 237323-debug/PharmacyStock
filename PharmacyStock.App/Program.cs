using System.Globalization;
using System.Text;
using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Documents;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Pricing;
using PharmacyStock.Core.Reporting;
using PharmacyStock.Core.Services;
using PharmacyStock.Core.Storage;

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

string report = DocumentReport.TextOf(request, result);

DocumentReportPrinter printer = new(Console.Out);
printer.Print(report);

Console.WriteLine();

IPrescriptionRepository repository =
    new InMemoryPrescriptionRepository();

IPricingPolicy pricing =
    new DiscountPricingPolicy(0.05m);

PrescriptionService service =
    new(repository, pricing);

Prescription prescription =
    new(2, 1, DateTimeOffset.UtcNow);

prescription.AddItem(
    new PrescriptionItem("MED-3", 2, 250m));

service.Place(prescription);

decimal total = service.TotalOf(prescription);

Console.WriteLine(string.Format(
    CultureInfo.GetCultureInfo("uk-UA"),
    "Сума за рецептом: {0:F2}",
    total));
