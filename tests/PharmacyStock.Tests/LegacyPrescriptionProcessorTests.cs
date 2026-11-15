using PharmacyStock.Core.Legacy;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests the original prescription processing behavior.
/// </summary>
public sealed class LegacyPrescriptionProcessorTests
{
    /// <summary>
    /// Checks the amount for a regular patient.
    /// </summary>
    [Fact]
    public void Process_RegularPatient_Returns1292()
    {
        List<MedicineLine> lines = new()
        {
            new MedicineLine
            {
                MedicineCode = "MED-1",
                Quantity = 12,
                UnitPrice = 100m,
            },
            new MedicineLine
            {
                MedicineCode = "MED-2",
                Quantity = 1,
                UnitPrice = 250m,
            },
        };

        decimal total = LegacyPrescriptionProcessor.Process(
            1,
            "Іваненко",
            "i@ex.com",
            true,
            lines,
            new DateOnly(2026, 3, 10),
            string.Empty,
            "New",
            false,
            60m,
            out string nextStatus);

        Assert.Equal(1292.70m, total);
        Assert.Equal("Approved", nextStatus);
    }

    /// <summary>
    /// Checks that delivery is included for a small prescription.
    /// </summary>
    [Fact]
    public void Process_SmallPrescription_AddsDelivery()
    {
        List<MedicineLine> lines = new()
        {
            new MedicineLine
            {
                MedicineCode = "MED-1",
                Quantity = 2,
                UnitPrice = 200m,
            },
        };

        decimal total = LegacyPrescriptionProcessor.Process(
            2,
            "Петренко",
            "p@ex.com",
            false,
            lines,
            new DateOnly(2026, 3, 10),
            "SALE10",
            "Approved",
            false,
            60m,
            out string nextStatus);

        Assert.Equal(420m, total);
        Assert.Equal("Dispensed", nextStatus);
    }

    /// <summary>
    /// Checks the original behavior for an empty prescription.
    /// </summary>
    [Fact]
    public void Process_NoLines_ReturnsMinusOne()
    {
        decimal total = LegacyPrescriptionProcessor.Process(
            3,
            "Коваль",
            "k@ex.com",
            false,
            new List<MedicineLine>(),
            new DateOnly(2026, 3, 10),
            string.Empty,
            "New",
            false,
            60m,
            out string nextStatus);

        Assert.Equal(-1m, total);
        Assert.Equal("New", nextStatus);
    }
}