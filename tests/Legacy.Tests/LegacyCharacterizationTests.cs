using System.Globalization;
using PharmacyStock.Legacy;
using Xunit;

namespace PharmacyStock.Legacy.Tests;

/// <summary>
/// Captures the existing behavior of the legacy pharmacy module.
/// </summary>
public sealed class LegacyCharacterizationTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyCharacterizationTests"/> class.
    /// </summary>
    public LegacyCharacterizationTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    /// <summary>Checks all four existing error messages.</summary>
    [Theory]
    [InlineData(null, "new", "a@b.c", "ERR: null")]
    [InlineData("empty", "new", "a@b.c", "ERR: empty")]
    [InlineData("two", "draft", "a@b.c", "ERR: state")]
    [InlineData("two", "new", "no-mail", "ERR: mail")]
    public void HandleBadInputReturnsErrorCode(
        string? kindOfItems,
        string state,
        string mail,
        string expected)
    {
        List<PrescriptionLine>? items = kindOfItems switch
        {
            "two" => TwoLines(),
            "empty" => new List<PrescriptionLine>(),
            _ => null,
        };

        LegacyPrescriptionProcessor sut = new();
        string actual = sut.Handle(CreateRequest(items, state, mail));

        Assert.Equal(expected, actual);
    }

    /// <summary>Checks an exact full-text report, including line endings.</summary>
    [Fact]
    public void HandleVipOrderReturnsExactReport()
    {
        LegacyPrescriptionProcessor sut = new();

        string actual = sut.Handle(CreateRequest(
            TwoLines(), "new", "a@b.c", "vip", 3));

        string expected =
            "Документ #1001\n" +
            "Клієнт: Іван\n" +
            "A-1 x2 = 300.00 UAH\n" +
            "B-2 x1 = 400.00 UAH\n" +
            "Знижка: 105.00 UAH\n" +
            "Доставка: 60.00 UAH\n" +
            "Разом: 655.00 UAH\n";

        Assert.Equal(expected, actual);
    }

    /// <summary>Checks every pricing category.</summary>
    [Theory]
    [InlineData("regular", 0, 760)]
    [InlineData("vip", 0, 655)]
    [InlineData("staff", 0, 550)]
    [InlineData("regular", 11, 725)]
    public void PreviewByKindReturnsTotal(
        string kind,
        int done,
        decimal expected)
    {
        LegacyPrescriptionProcessor sut = new();
        decimal actual = sut.Preview(kind, done, TwoLines());
        Assert.Equal(expected, actual);
    }

    /// <summary>Checks the exact free-shipping boundary.</summary>
    [Fact]
    public void PreviewAtFreeShippingBoundaryReturnsOneThousand()
    {
        LegacyPrescriptionProcessor sut = new();
        List<PrescriptionLine> items = new()
        {
            new() { Code = "MED-1000", Qty = 1, Price = 1000m },
        };

        Assert.Equal(1000m, sut.Preview("regular", 0, items));
    }

    /// <summary>Checks the legacy customer text and the current-year dependency.</summary>
    [Fact]
    public void DescribeClientVipBuildsCaption()
    {
        PatientRecord patient = new()
        {
            Name = " іван ",
            Email = "IVAN@MAIL.COM",
            Kind = "vip",
            DoneCount = 12,
            SinceUtc = new DateTime(2020, 1, 1),
        };

        int years = DateTime.Now.Year - 2020;
        string expected = "ІВАН [VIP] [ЛОЯЛЬНИЙ] " +
            "<ivan@mail.com> стаж " + years.ToString(CultureInfo.InvariantCulture);

        Assert.Equal(expected, patient.Describe());
    }

    /// <summary>Checks the exact log text after successful handling.</summary>
    [Fact]
    public void DumpLogAfterHandlingIncludesMail()
    {
        LegacyPrescriptionProcessor sut = new();
        sut.Handle(CreateRequest(
            TwoLines(), "new", "a@b.c", sendMail: true));

        Assert.Equal("ok 1001\nmail -> a@b.c\n", sut.DumpLog());
    }

    /// <summary>Checks that the VIP discount cap is preserved.</summary>
    [Fact]
    public void PreviewVipDiscountIsCappedAtFiveHundred()
    {
        LegacyPrescriptionProcessor sut = new();
        List<PrescriptionLine> items = new()
        {
            new() { Code = "MED-10000", Qty = 1, Price = 10000m },
        };

        Assert.Equal(9500m, sut.Preview("vip", 0, items));
    }

    private static PrescriptionRequest CreateRequest(
        List<PrescriptionLine>? items,
        string state,
        string? mail,
        string kind = "regular",
        int done = 0,
        bool sendMail = false)
    {
        DocState documentState = state switch
        {
            "new" => DocState.New,
            "paid" => DocState.Paid,
            _ => DocState.Draft,
        };

        PatientRecord patient = new()
        {
            Id = 7,
            Name = "Іван",
            Email = mail ?? string.Empty,
            Kind = kind,
            DoneCount = done,
        };

        return new PrescriptionRequest(
            1001, patient, items, documentState, "UAH", sendMail);
    }

    private static List<PrescriptionLine> TwoLines()
    {
        return new List<PrescriptionLine>
        {
            new() { Code = "A-1", Qty = 2, Price = 150m },
            new() { Code = "B-2", Qty = 1, Price = 400m },
        };
    }
}
