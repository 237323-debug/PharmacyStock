namespace PharmacyStock.Core.Domain;

/// <summary>
/// Represents an immutable pharmacy patient.
/// </summary>
public sealed record Patient
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="Patient"/> class.
    /// </summary>
    /// <param name="id">Patient identifier.</param>
    /// <param name="fullName">Patient full name.</param>
    /// <param name="email">Patient email address.</param>
    /// <param name="hasBenefits">Patient benefits indicator.</param>
    public Patient(
        int id,
        string fullName,
        string email,
        bool hasBenefits = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        if (!email.Contains('@'))
        {
            throw new ArgumentException(
                "Електронна пошта має містити символ @.",
                nameof(email));
        }

        Id = id;
        FullName = fullName;
        Email = email;
        HasBenefits = hasBenefits;
    }

    /// <summary>Gets the patient identifier.</summary>
    public int Id { get; }

    /// <summary>Gets the patient's full name.</summary>
    public string FullName { get; }

    /// <summary>Gets the patient email.</summary>
    public string Email { get; }

    /// <summary>
    /// Gets a value indicating whether the patient has benefits.
    /// </summary>
    public bool HasBenefits { get; }
}
