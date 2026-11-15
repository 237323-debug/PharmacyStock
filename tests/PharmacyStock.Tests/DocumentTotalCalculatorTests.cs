using PharmacyStock.Core.Documents;
using PharmacyStock.Core.Reporting;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests prescription calculations and reports.
/// </summary>
public sealed class DocumentTotalCalculatorTests
{
    /// <summary>
    /// Checks the regular patient amount.
    /// </summary>
    [Fact]
    public void CalculateRegularPatientReturns1292()
    {
        TotalRequest request = BuildSampleRequest();

        TotalResult result =
            DocumentTotalCalculator.Calculate(request);

        Assert.Equal(1292.70m, result.Total);
        Assert.Equal(DocumentStatus.Approved, result.Status);
    }

    /// <summary>
    /// Checks delivery for a small prescription.
    /// </summary>
    [Fact]
    public void CalculateSmallPrescriptionAddsDelivery()
    {
        TotalRequest request = new(
            DocumentId: 2,
            Client: new ClientInfo(
                "Петренко", "p@ex.com", false),
            Lines: new List<MedicineLine>
            {
                new()
                {
                    MedicineCode = "MED-1",
                    Quantity = 2,
                    UnitPrice = 200m,
                },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: "SALE10",
            Status: DocumentStatus.Approved,
            DeliveryPrice: 60m);

        TotalResult result =
            DocumentTotalCalculator.Calculate(request);

        Assert.Equal(420m, result.Total);
        Assert.Equal(DocumentStatus.Dispensed, result.Status);
    }

    /// <summary>
    /// Checks that empty medicine lines are rejected.
    /// </summary>
    [Fact]
    public void CalculateNoLinesThrowsArgument()
    {
        TotalRequest request = BuildSampleRequest() with
        {
            Lines = Array.Empty<MedicineLine>(),
        };

        Assert.Throws<ArgumentException>(() =>
            DocumentTotalCalculator.Calculate(request));
    }

    /// <summary>
    /// Checks that zero quantity is rejected.
    /// </summary>
    [Fact]
    public void CalculateZeroQuantityThrowsRange()
    {
        TotalRequest request = BuildSampleRequest() with
        {
            Lines = new List<MedicineLine>
            {
                new()
                {
                    MedicineCode = "MED-1",
                    Quantity = 0,
                    UnitPrice = 100m,
                },
            },
        };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DocumentTotalCalculator.Calculate(request));
    }

    /// <summary>
    /// Checks that the report contains the calculated total.
    /// </summary>
    [Fact]
    public void TextOfContainsTotalLine()
    {
        TotalRequest request = BuildSampleRequest();

        TotalResult result =
            DocumentTotalCalculator.Calculate(request);

        string text = DocumentReport.TextOf(request, result);

        Assert.Contains("Разом: 1292.70", text);
    }

    /// <summary>
    /// Checks that the report contains patient data.
    /// </summary>
    [Fact]
    public void TextOfContainsPatientAndStatus()
    {
        TotalRequest request = BuildSampleRequest();

        TotalResult result =
            DocumentTotalCalculator.Calculate(request);

        string text = DocumentReport.TextOf(request, result);

        Assert.Contains("Пацієнт: Іваненко", text);
        Assert.Contains("Стан: Approved", text);
    }

    /// <summary>
    /// Checks that the printer writes a report.
    /// </summary>
    [Fact]
    public void PrintWritesReportToOutput()
    {
        StringWriter writer = new();
        DocumentReportPrinter printer = new(writer);

        printer.Print("Разом: 10.00");

        Assert.Contains("10.00", writer.ToString());
    }

    /// <summary>
    /// Checks that an empty report is rejected.
    /// </summary>
    [Fact]
    public void PrintEmptyReportThrowsArgument()
    {
        StringWriter writer = new();
        DocumentReportPrinter printer = new(writer);

        Assert.Throws<ArgumentException>(() =>
            printer.Print(string.Empty));
    }

    private static TotalRequest BuildSampleRequest()
    {
        return new TotalRequest(
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
    }
}
