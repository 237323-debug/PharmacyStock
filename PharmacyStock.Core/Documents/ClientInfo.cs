namespace PharmacyStock.Core.Documents;

/// <summary>
/// Contains information about a pharmacy patient.
/// </summary>
/// <param name="Name">Patient name.</param>
/// <param name="Email">Patient email.</param>
/// <param name="IsRegular">Regular patient indicator.</param>
public sealed record ClientInfo(
    string Name,
    string Email,
    bool IsRegular);
