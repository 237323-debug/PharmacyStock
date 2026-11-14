namespace PharmacyStock.Core.Domain;

public sealed class PrescriptionItem
{
    public string MedicineCode { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal Amount => Quantity * UnitPrice;
}
