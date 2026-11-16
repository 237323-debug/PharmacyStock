using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;

namespace PharmacyStock.Core.Storage;

/// <summary>
/// Stores prescriptions in memory.
/// </summary>
public sealed class InMemoryPrescriptionRepository : IPrescriptionRepository
{
    private readonly Dictionary<int, Prescription> _prescriptions = new();

    /// <summary>
    /// Adds a prescription to storage.
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
    /// Gets a prescription by identifier.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>The requested prescription.</returns>
    public Prescription GetById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        if (!_prescriptions.TryGetValue(
            id, out Prescription? prescription))
        {
            throw new DomainRuleException(
                "prescription.exists",
                $"Рецепт № {id} не знайдено.");
        }

        return prescription;
    }

    /// <summary>
    /// Returns all stored prescriptions.
    /// </summary>
    /// <returns>Copy of stored prescriptions.</returns>
    public IReadOnlyList<Prescription> GetAll()
    {
        return _prescriptions.Values.ToList().AsReadOnly();
    }
}
