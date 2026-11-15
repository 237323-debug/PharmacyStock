namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a medicine in the pharmacy.
/// </summary>
public sealed class Medicine
{
    /// <summary>Gets the medicine code.</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>Gets the medicine name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the medicine price.</summary>
    public decimal Price { get; init; }

    /// <summary>Gets the available stock quantity.</summary>
    public int Stock { get; init; }
}
