namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Represents a legacy medicine line.
/// </summary>
public class LegacyPrescriptionLine
{
    /// <summary>Gets or sets the medicine code.</summary>
    public string MedicineCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the quantity.</summary>
    public int Quantity { get; set; }

    /// <summary>Gets or sets the unit price.</summary>
    public decimal UnitPrice { get; set; }
}
