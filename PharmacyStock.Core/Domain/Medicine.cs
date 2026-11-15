namespace PharmacyStock.Core.Domain;

/// <summary>Лікарський препарат в аптеці.</summary>
public sealed class Medicine
{
    /// <summary>Код препарату.</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>Назва препарату.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Ціна препарату.</summary>
    public decimal Price { get; init; }

    /// <summary>Кількість на складі.</summary>
    public int Stock { get; init; }
}