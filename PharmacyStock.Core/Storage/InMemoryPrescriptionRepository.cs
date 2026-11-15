using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Storage;

/// <summary>
/// Stores prescriptions in memory.
/// </summary>
public sealed class InMemoryPrescriptionRepository : IPrescriptionRepository
{
    private readonly Dictionary<int, Prescription> _prescriptions = new();

    /// <summary>
    /// Adds a prescription to memory storage.
    /// </summary>
    /// <param name="prescription">Prescription to add.</param>
    public void Add(Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        if (!_prescriptions.TryAdd(prescription.Id, prescription))
        {
            throw new InvalidOperationException(
                "Рецепт із таким номером уже існує.");
        }
    }

    /// <summary>
    /// Finds a prescription by identifier.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>Prescription or null if not found.</returns>
    public Prescription? GetById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        return _prescriptions.TryGetValue(id, out Prescription? prescription)
            ? prescription
            : null;
    }

    /// <summary>
    /// Returns all stored prescriptions.
    /// </summary>
    /// <returns>Copy of the prescription list.</returns>
    public IReadOnlyList<Prescription> GetAll()
    {
        return _prescriptions.Values.ToList().AsReadOnly();
    }
}
