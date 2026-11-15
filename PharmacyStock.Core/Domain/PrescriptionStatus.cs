namespace PharmacyStock.Core.Domain;

/// <summary>Можливі стани рецепта.</summary>
public enum PrescriptionStatus
{
    /// <summary>Новий рецепт.</summary>
    New = 0,

    /// <summary>Підтверджений рецепт.</summary>
    Approved = 1,

    /// <summary>Препарати видано.</summary>
    Dispensed = 2,

    /// <summary>Рецепт скасовано.</summary>
    Cancelled = 3,

    /// <summary>Термін рецепта завершився.</summary>
    Expired = 4
}