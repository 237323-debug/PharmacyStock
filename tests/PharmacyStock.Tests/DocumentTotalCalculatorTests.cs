using PharmacyStock.Core.Documents;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests prescription totals and input validation.
/// </summary>
public sealed class DocumentTotalCalculatorTests
{
    /// <summary>
    /// Checks the total for a regular patient.
    /// </summary>
    [Fact]
    public void Calculate_RegularPatient_Returns1292()
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
    public void Calculate_SmallPrescription_AddsDelivery()
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
    /// Checks that an empty prescription throws an exception.
    /// </summary>
    [Fact]
    public void Calculate_NoLines_ThrowsArgument()
    {
        TotalRequest request = BuildSampleRequest() with
        {
            Lines = Array.Empty<MedicineLine>(),
        };

        Assert.Throws<ArgumentException>(() =>
            DocumentTotalCalculator.Calculate(request));
    }

    /// <summary>
    /// Checks that zero quantity throws an exception.
    /// </summary>
    [Fact]
    public void Calculate_ZeroQuantity_ThrowsRange()
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
