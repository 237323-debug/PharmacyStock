namespace PharmacyStock.Core.Documents;

/// <summary>
/// Represents the status of a pharmacy prescription.
/// </summary>
public enum DocumentStatus
{
    /// <summary>New prescription.</summary>
    New,

    /// <summary>Approved prescription.</summary>
    Approved,

    /// <summary>Dispensed prescription.</summary>
    Dispensed,

    /// <summary>Cancelled prescription.</summary>
    Cancelled,
}
