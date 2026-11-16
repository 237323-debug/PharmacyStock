using PharmacyStock.Core.Abstractions;
using PharmacyStock.Core.Domain;

namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Demonstrates incorrect exception handling.
/// </summary>
public sealed class PrescriptionErrorAntipatterns
{
    private readonly IPrescriptionRepository _repository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PrescriptionErrorAntipatterns"/> class.
    /// </summary>
    /// <param name="repository">Prescription repository.</param>
    public PrescriptionErrorAntipatterns(
        IPrescriptionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Demonstrates a swallowed exception.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <returns>Prescription or null.</returns>
    public Prescription? Load(int id)
    {
        try
        {
            return _repository.GetById(id);
        }
        catch (Exception)
        {
        }

        return null;
    }

    /// <summary>
    /// Demonstrates broad catch and lost stack trace.
    /// </summary>
    /// <param name="id">Prescription identifier.</param>
    /// <param name="reason">Cancellation reason.</param>
    public void Cancel(int id, string reason)
    {
        try
        {
            Prescription prescription = _repository.GetById(id);
            prescription.Cancel();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Щось пішло не так");
            throw ex;
        }
    }

    /// <summary>
    /// Demonstrates exceptions used for normal input validation.
    /// </summary>
    /// <param name="rows">Quantity values.</param>
    /// <returns>Number of valid quantities.</returns>
    public int CountValidQuantities(string[] rows)
    {
        int valid = 0;

        foreach (string row in rows)
        {
            try
            {
                int quantity = int.Parse(row);

                if (quantity > 0)
                {
                    valid++;
                }
            }
            catch (FormatException)
            {
            }
        }

        return valid;
    }
}