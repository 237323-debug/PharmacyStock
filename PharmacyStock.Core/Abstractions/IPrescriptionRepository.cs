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
    /// Gets a prescription by identifier.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>The requested prescription.</returns>
    /// <exception cref="PharmacyStock.Core.Errors.DomainRuleException">
    /// Thrown when the prescription does not exist.
    /// </exception>
    Prescription GetById(int id);

    /// <summary>
    /// Returns all stored prescriptions.
    /// </summary>
    /// <returns>Stored prescriptions.</returns>
    IReadOnlyList<Prescription> GetAll();
}
