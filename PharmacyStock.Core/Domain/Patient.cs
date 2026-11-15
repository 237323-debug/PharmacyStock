namespace PharmacyStock.Core.Domain;

/// <summary>Інформація про пацієнта.</summary>
public sealed class Patient
{
    /// <summary>Номер пацієнта.</summary>
    public int Id { get; init; }

    /// <summary>Повне ім'я пацієнта.</summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>Наявність пільг.</summary>
    public bool HasBenefits { get; init; }
}