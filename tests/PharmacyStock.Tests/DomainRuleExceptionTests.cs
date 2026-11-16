using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;
using PharmacyStock.Core.Storage;
using Xunit;

namespace PharmacyStock.Tests;

/// <summary>
/// Tests domain rule exceptions.
/// </summary>
public sealed class DomainRuleExceptionTests
{
    /// <summary>
    /// Checks that a missing prescription throws a domain exception.
    /// </summary>
    [Fact]
    public void GetByIdWhenMissingThrowsDomainRuleException()
    {
        InMemoryPrescriptionRepository repository = new();

        DomainRuleException error =
            Assert.Throws<DomainRuleException>(() =>
                repository.GetById(99));

        Assert.Equal("prescription.exists", error.Rule);
        Assert.Contains("99", error.Message);
    }

    /// <summary>
    /// Checks that an existing prescription is returned.
    /// </summary>
    [Fact]
    public void GetByIdWhenFoundReturnsPrescription()
    {
        InMemoryPrescriptionRepository repository = new();

        Prescription prescription =
            new(1, 1, DateTimeOffset.UtcNow);

        repository.Add(prescription);

        Prescription result = repository.GetById(1);

        Assert.Same(prescription, result);
    }
}
