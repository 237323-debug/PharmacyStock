namespace PharmacyStock.Core.Domain;

public sealed class Patient
{
    public int Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public bool HasBenefits { get; init; }
}