namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents a medicine item in a prescription.
/// </summary>
public sealed class PrescriptionItem
{
    /// <summary>Gets the medicine code.</summary>
    public string MedicineCode { get; init; } = string.Empty;

    /// <summary>Gets the medicine quantity.</summary>
    public int Quantity { get; init; }

    /// <summary>Gets the unit price.</summary>
    public decimal UnitPrice { get; init; }

    /// <summary>Gets the total amount.</summary>
    public decimal Amount => Quantity * UnitPrice;
}
