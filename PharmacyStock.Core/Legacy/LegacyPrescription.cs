namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Represents a prescription without encapsulation.
/// </summary>
public class LegacyPrescription
{
    /// <summary>Gets or sets the prescription identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the prescription status.</summary>
    public int Status { get; set; }

    /// <summary>Gets or sets the prescription total.</summary>
    public decimal Total { get; set; }

    /// <summary>Gets or sets the patient email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the medicine lines.</summary>
    public List<LegacyPrescriptionLine> Lines { get; set; } = new();

    /// <summary>
    /// Adds a medicine line to the prescription.
    /// </summary>
    /// <param name="line">Medicine line.</param>
    public virtual void AddLine(LegacyPrescriptionLine line)
    {
        Lines.Add(line);
    }
}
