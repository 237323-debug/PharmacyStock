using System.Globalization;
using System.Text;
using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Documents;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Notifications;
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

Dictionary<string, IPricingPolicy> policies = new()
{
    ["regular"] = new DiscountPricingPolicy(0.05m),
    ["vip"] = new VipPricingPolicy(),
    ["staff"] = new DiscountPricingPolicy(0.30m),
};

string patientType = "regular";

IPricingPolicy pricing =
    policies.GetValueOrDefault(patientType)
    ?? new RegularPricingPolicy();

InMemoryPrescriptionRepository repository = new();

INotifier notifier =
    new TextWriterNotifier(Console.Out);

PrescriptionService service =
    new(repository, pricing, notifier);

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

service.NotifyPatient(prescription, "i@ex.com");

ArchivedPrescription archived = new(
    prescription,
    DateOnly.FromDateTime(DateTime.Today));

Console.WriteLine(
    "Архівний рецепт № " + archived.Id.ToString(
        CultureInfo.InvariantCulture));

string csv = PrescriptionCsvReport.BuildCsv(
    repository.GetAll());

Console.WriteLine("CSV-звіт:");
Console.Write(csv);
