using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Notifications;
using PharmacyStock.Core.Pricing;
using PharmacyStock.Core.Services;
using PharmacyStock.Core.Storage;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests pharmacy domain invariants.
/// </summary>
public sealed class DomainInvariantTests
{
    /// <summary>
    /// Checks that zero medicine quantity is rejected.
    /// </summary>
    [Fact]
    public void PrescriptionItemWithZeroQuantityThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PrescriptionItem("MED-1", 0, 100m));
    }

    /// <summary>
    /// Checks that approved prescriptions cannot be modified.
    /// </summary>
    [Fact]
    public void PrescriptionAfterApprovalRejectsNewItem()
    {
        Prescription prescription =
            new(1, 1, DateTimeOffset.UtcNow);

        prescription.AddItem(
            new PrescriptionItem("MED-1", 1, 100m));

        prescription.Approve();

        Assert.Throws<InvalidOperationException>(() =>
            prescription.AddItem(
                new PrescriptionItem("MED-2", 1, 50m)));
    }

    /// <summary>
    /// Checks the total with an injected discount policy.
    /// </summary>
    [Fact]
    public void TotalOfWithTenPercentDiscountReturns180()
    {
        PrescriptionService service = new(
            new InMemoryPrescriptionRepository(),
            new DiscountPricingPolicy(0.10m),
            new TextWriterNotifier(TextWriter.Null));

        Prescription prescription =
            new(2, 1, DateTimeOffset.UtcNow);

        prescription.AddItem(
            new PrescriptionItem("MED-1", 2, 100m));

        service.Place(prescription);

        decimal total = service.TotalOf(prescription);

        Assert.Equal(180m, total);
    }
}
