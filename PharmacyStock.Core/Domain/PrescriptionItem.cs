namespace PharmacyStock.Core.Domain;

/// <summary>Препарат у рецепті.</summary>
public sealed class PrescriptionItem
{
    /// <summary>Код препарату.</summary>
    public string MedicineCode { get; init; } = string.Empty;

    /// <summary>Кількість препарату.</summary>
    public int Quantity { get; init; }

    /// <summary>Ціна за одиницю.</summary>
    public decimal UnitPrice { get; init; }

    /// <summary>Загальна вартість позиції.</summary>
    public decimal Amount => Quantity * UnitPrice;
}