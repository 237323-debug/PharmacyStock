namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a pharmacy patient.
/// </summary>
public sealed class Patient
{
    /// <summary>Gets the patient identifier.</summary>
    public int Id { get; init; }

    /// <summary>Gets the patient's full name.</summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the patient has benefits.
    /// </summary>
    public bool HasBenefits { get; init; }
}
