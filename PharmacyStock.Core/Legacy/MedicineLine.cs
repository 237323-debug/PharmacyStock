namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Represents a medicine line in a prescription.
/// </summary>
public sealed class MedicineLine
{
    /// <summary>Gets or sets the medicine code.</summary>
    public string MedicineCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the medicine quantity.</summary>
    public int Quantity { get; set; }

    /// <summary>Gets or sets the medicine unit price.</summary>
    public decimal UnitPrice { get; set; }
}
