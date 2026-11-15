using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Abstractions;

/// <summary>
/// Defines operations for storing prescriptions.
/// </summary>
public interface IPrescriptionRepository
{
    /// <summary>
    /// Adds a prescription to storage.
    /// </summary>
    /// <param name="prescription">Prescription to add.</param>
    void Add(Prescription prescription);

    /// <summary>
    /// Finds a prescription by identifier.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>Prescription or null if not found.</returns>
    Prescription? GetById(int id);

    /// <summary>
    /// Returns all stored prescriptions.
    /// </summary>
    /// <returns>List of prescriptions.</returns>
    IReadOnlyList<Prescription> GetAll();
}
