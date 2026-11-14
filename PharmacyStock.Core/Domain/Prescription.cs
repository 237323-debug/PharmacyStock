namespace PharmacyStock.Core.Domain;

public sealed class Prescription
{
    private readonly List<PrescriptionItem> _items = new();

    public int Id { get; init; }

    public int PatientId { get; init; }

    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.New;

    public DateTimeOffset CreatedAt { get; init; }

    public IReadOnlyList<PrescriptionItem> Items => _items;

    public void AddItem(PrescriptionItem item)
    {
        _items.Add(item);
    }

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