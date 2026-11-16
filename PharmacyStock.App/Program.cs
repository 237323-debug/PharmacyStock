using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using PharmacyStock.App.Input;
using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Documents;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;
using PharmacyStock.Core.Notifications;
using PharmacyStock.Core.Pricing;
using PharmacyStock.Core.Reporting;
using PharmacyStock.Core.Services;
using PharmacyStock.Core.Storage;

Console.OutputEncoding = Encoding.UTF8;

// Налаштування журналу.
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.TimestampFormat = "HH:mm:ss ";
        options.IncludeScopes = true;
        options.SingleLine = true;
    });

    builder.SetMinimumLevel(LogLevel.Debug);
});

ILogger<PrescriptionService> logger =
    loggerFactory.CreateLogger<PrescriptionService>();

// Приклад лабораторної № 3.
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

// Приклад лабораторної № 4.
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

INotifier notifier = new TextWriterNotifier(Console.Out);

PrescriptionService service = new(
    repository,
    pricing,
    notifier,
    logger);

Prescription prescription = new(
    2,
    1,
    DateTimeOffset.UtcNow);

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

// Лабораторна № 5. Введення даних.
Console.WriteLine();
Console.WriteLine("Додавання нового препарату до рецепта № 2.");

while (true)
{
    Console.Write("Введіть код препарату: ");
    string? medicineCode = Console.ReadLine();

    if (medicineCode is null)
    {
        Console.WriteLine("Введення завершено.");
        break;
    }

    Console.Write("Введіть кількість: ");
    string? quantityText = Console.ReadLine();

    if (quantityText is null)
    {
        Console.WriteLine("Введення завершено.");
        break;
    }

    Console.Write("Введіть ціну: ");
    string? priceText = Console.ReadLine();

    if (priceText is null)
    {
        Console.WriteLine("Введення завершено.");
        break;
    }

    Result<PrescriptionItem> parsed =
        PrescriptionItemParser.Parse(
            medicineCode,
            quantityText,
            priceText);

    if (!parsed.IsSuccess)
    {
        Console.WriteLine("Помилка вводу: " + parsed.Error);
        Console.WriteLine("Спробуйте ввести дані ще раз.");
        Console.WriteLine();

        continue;
    }

    if (parsed.Value is not PrescriptionItem item)
    {
        throw new InvalidOperationException(
            "Результат перевірки не містить препарату.");
    }

    service.AddItem(prescription.Id, item);

    Console.WriteLine("Препарат успішно додано до рецепта.");

    Console.WriteLine(string.Format(
        CultureInfo.GetCultureInfo("uk-UA"),
        "Нова сума рецепта: {0:F2}",
        service.TotalOf(prescription)));

    break;
}

// Демонстрація чотирьох рівнів логування.
Console.WriteLine();
Console.WriteLine("Демонстрація журналу подій:");

Prescription demo = new(
    3,
    1,
    DateTimeOffset.UtcNow);

demo.AddItem(
    new PrescriptionItem("MED-LOG", 1, 100m));

service.Place(demo);

service.AddItem(
    demo.Id,
    new PrescriptionItem("MED-LOG-2", 1, 50m));

demo.Approve();
demo.Dispense();

// Спроба змінити вже виданий рецепт.
try
{
    service.AddItem(
        demo.Id,
        new PrescriptionItem("MED-LOG-3", 1, 20m));
}
catch (DomainRuleException ex)
{
    Console.WriteLine("Правило: " + ex.Rule);
    Console.WriteLine(ex.Message);
}

// Спроба скасувати вже виданий рецепт.
try
{
    service.Cancel(demo.Id, "Демонстрація помилки");
}
catch (DomainRuleException ex)
{
    Console.WriteLine("Правило: " + ex.Rule);
    Console.WriteLine(ex.Message);
}
