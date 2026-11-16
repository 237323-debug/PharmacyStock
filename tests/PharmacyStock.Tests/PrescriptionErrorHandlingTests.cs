using PharmacyStock.App.Input;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;
using PharmacyStock.Core.Notifications;
using PharmacyStock.Core.Pricing;
using PharmacyStock.Core.Services;
using PharmacyStock.Core.Storage;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests corrected prescription error handling.
/// </summary>
public sealed class PrescriptionErrorHandlingTests
{
    /// <summary>
    /// Checks that missing prescriptions are not silently ignored.
    /// </summary>
    [Fact]
    public void LoadWhenPrescriptionMissingThrows()
    {
        PrescriptionService service = CreateService();

        DomainRuleException error =
            Assert.Throws<DomainRuleException>(() =>
                service.Load(99));

        Assert.Equal("prescription.exists", error.Rule);
    }

    /// <summary>
    /// Checks that an existing prescription can be cancelled.
    /// </summary>
    [Fact]
    public void CancelWhenPrescriptionNewChangesStatus()
    {
        PrescriptionService service = CreateService();

        Prescription prescription =
            new(5, 1, DateTimeOffset.UtcNow);

        service.Place(prescription);
        service.Cancel(5, "Помилка оформлення");

        Assert.Equal(
            PrescriptionStatus.Cancelled,
            prescription.Status);
    }

    /// <summary>
    /// Checks that invalid text quantities are ignored.
    /// </summary>
    [Fact]
    public void CountValidQuantitiesWithInvalidTextReturnsTwo()
    {
        string[] values = { "2", "дві", "0", "-1", "4" };

        int result = QuantityInputAnalyzer.CountValidQuantities(values);

        Assert.Equal(2, result);
    }

    private static PrescriptionService CreateService()
    {
        return new PrescriptionService(
            new InMemoryPrescriptionRepository(),
            new RegularPricingPolicy(),
            new TextWriterNotifier(TextWriter.Null));
    }
}
