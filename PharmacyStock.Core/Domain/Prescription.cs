namespace PharmacyStock.Core.Domain;

/// <summary>Рецепт пацієнта.</summary>
public sealed class Prescription
{
    private readonly List<PrescriptionItem> _items = new();

    /// <summary>Номер рецепта.</summary>
    public int Id { get; init; }

    /// <summary>Номер пацієнта.</summary>
    public int PatientId { get; init; }

    /// <summary>Поточний стан рецепта.</summary>
    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.New;

    /// <summary>Дата створення.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Список препаратів.</summary>
    public IReadOnlyList<PrescriptionItem> Items => _items;

    /// <summary>Додає препарат до рецепта.</summary>
    /// <param name="item">Препарат для додавання.</param>
    public void AddItem(PrescriptionItem item)
    {
        _items.Add(item);
    }

    /// <summary>Обчислює загальну вартість.</summary>
    /// <returns>Загальна сума.</returns>
    public decimal Total()
    {
        decimal sum = 0m;

        foreach (PrescriptionItem item in _items)
        {
            sum += item.Amount;
        }

        return sum;
    }
}