using PharmacyStock.Core.Documents;
using PharmacyStock.Core.Legacy;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests prescription processing and validation.
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
            DocumentStatus.New,
            false,
            60m,
            out DocumentStatus nextStatus);

        Assert.Equal(1292.70m, total);
        Assert.Equal(DocumentStatus.Approved, nextStatus);
    }

    /// <summary>
    /// Checks delivery for a small prescription.
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
            DocumentStatus.Approved,
            false,
            60m,
            out DocumentStatus nextStatus);

        Assert.Equal(420m, total);
        Assert.Equal(DocumentStatus.Dispensed, nextStatus);
    }

    /// <summary>
    /// Checks that an empty prescription throws an exception.
    /// </summary>
    [Fact]
    public void Process_NoLines_ThrowsArgument()
    {
        List<MedicineLine> lines = new();

        Assert.Throws<ArgumentException>(() =>
            DocumentGuards.EnsureLinesValid(lines));
    }

    /// <summary>
    /// Checks that zero quantity throws an exception.
    /// </summary>
    [Fact]
    public void Process_ZeroQuantity_ThrowsRange()
    {
        MedicineLine line = new()
        {
            MedicineCode = "MED-1",
            Quantity = 0,
            UnitPrice = 100m,
        };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            DocumentGuards.EnsureLineValid(line));
    }
}
