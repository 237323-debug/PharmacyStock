using System.Globalization;
using PharmacyStock.Core.Domain;
using PharmacyStock.Core.Errors;

namespace PharmacyStock.App.Input;

/// <summary>
/// Converts user input into validated prescription items.
/// </summary>
public static class PrescriptionItemParser
{
    /// <summary>
    /// Parses a medicine item from user input.
    /// </summary>
    /// <param name="medicineCode">Medicine code.</param>
    /// <param name="quantityText">Quantity as text.</param>
    /// <param name="priceText">Price as text.</param>
    /// <returns>Parsed item or validation error.</returns>
    public static Result<PrescriptionItem> Parse(
        string? medicineCode,
        string? quantityText,
        string? priceText)
    {
        if (string.IsNullOrWhiteSpace(medicineCode))
        {
            return ResultFactory.Fail<PrescriptionItem>(
                "Введіть код препарату.");
        }

        bool quantityValid = int.TryParse(
            quantityText,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int quantity);

        if (!quantityValid || quantity <= 0)
        {
            return ResultFactory.Fail<PrescriptionItem>(
                "Кількість має бути цілим числом більше 0.");
        }

        bool priceValid = decimal.TryParse(
            priceText,
            NumberStyles.Number,
            CultureInfo.GetCultureInfo("uk-UA"),
            out decimal price);

        if (!priceValid || price < 0m)
        {
            return ResultFactory.Fail<PrescriptionItem>(
                "Ціна має бути числом і не може бути від'ємною.");
        }

        PrescriptionItem item = new(
            medicineCode.Trim(),
            quantity,
            price);

        return ResultFactory.Ok(item);
    }
}
