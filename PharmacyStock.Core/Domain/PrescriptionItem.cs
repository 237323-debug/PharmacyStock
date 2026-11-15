namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents an immutable medicine item in a prescription.
/// </summary>
public sealed record PrescriptionItem
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionItem"/> class.
    /// </summary>
    /// <param name="medicineCode">Medicine code.</param>
    /// <param name="quantity">Medicine quantity.</param>
    /// <param name="unitPrice">Medicine unit price.</param>
    public PrescriptionItem(
        string medicineCode,
        int quantity,
        decimal unitPrice)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(medicineCode);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);

        MedicineCode = medicineCode;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    /// <summary>Gets the medicine code.</summary>
    public string MedicineCode { get; }

    /// <summary>Gets the medicine quantity.</summary>
    public int Quantity { get; }

    /// <summary>Gets the unit price.</summary>
    public decimal UnitPrice { get; }

    /// <summary>Gets the total item amount.</summary>
    public decimal Amount => Quantity * UnitPrice;
}
