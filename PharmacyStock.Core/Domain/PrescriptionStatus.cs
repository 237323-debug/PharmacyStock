namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents possible prescription statuses.
/// </summary>
public enum PrescriptionStatus
{
    /// <summary>New prescription.</summary>
    New = 0,

    /// <summary>Approved prescription.</summary>
    Approved = 1,

    /// <summary>Dispensed prescription.</summary>
    Dispensed = 2,

    /// <summary>Cancelled prescription.</summary>
    Cancelled = 3,

    /// <summary>Expired prescription.</summary>
    Expired = 4,
}
