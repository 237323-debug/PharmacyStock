using System.Globalization;

namespace PharmacyStock.App.Input;

/// <summary>
/// Analyzes quantities entered by the user.
/// </summary>
public static class QuantityInputAnalyzer
{
    /// <summary>
    /// Counts positive integer quantities.
    /// </summary>
    /// <param name="rows">User input values.</param>
    /// <returns>Number of valid quantities.</returns>
    public static int CountValidQuantities(string[] rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        int valid = 0;

        foreach (string row in rows)
        {
            bool parsed = int.TryParse(
                row,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int quantity);

            if (parsed && quantity > 0)
            {
                valid++;
            }
        }

        return valid;
    }
}
