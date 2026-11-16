using System.Collections.Generic;

namespace PharmacyStock.Legacy;

/// <summary>
/// Builds the unchanged text of a legacy prescription report.
/// </summary>
public sealed class ReportBuilder
{
    /// <summary>
    /// Builds the document text in the original format.
    /// </summary>
    public string Build(
        int docId,
        string clientName,
        List<PrescriptionLine> items,
        string currency,
        decimal discount,
        decimal shipping,
        decimal total)
    {
        string txt = "Документ #" + docId + "\n";
        txt += "Клієнт: " + clientName + "\n";
        for (int i = 0; i < items.Count; i++)
        {
            txt += items[i].Code + " x" + items[i].Qty + " = " +
                (items[i].Qty * items[i].Price)
                    .ToString("0.00") + " " + currency + "\n";
        }
        txt += "Знижка: " + discount.ToString("0.00") +
            " " + currency + "\n";
        txt += "Доставка: " + shipping.ToString("0.00") +
            " " + currency + "\n";
        txt += "Разом: " + total.ToString("0.00") +
            " " + currency + "\n";
        return txt;
    }
}
