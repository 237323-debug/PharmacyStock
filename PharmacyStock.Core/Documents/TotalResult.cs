namespace PharmacyStock.Core.Documents;

/// <summary>
/// Contains the calculated prescription result.
/// </summary>
/// <param name="Total">Calculated amount.</param>
/// <param name="Status">Resulting prescription status.</param>
public sealed record TotalResult(
    decimal Total,
    DocumentStatus Status);
