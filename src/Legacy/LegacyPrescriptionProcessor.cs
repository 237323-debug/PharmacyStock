using System;
using System.Collections.Generic;

namespace PharmacyStock.Legacy;

public class PatientRecord
{
    public int Id;
    public string Name = "";
    public string Email = "";
    public string Kind = "regular";
    public int DoneCount;
    public DateTime SinceUtc;
}

public class PrescriptionLine
{
    public string Code = "";
    public int Qty;
    public decimal Price;
    public string Group = "";
}

public class LegacyPrescriptionProcessor
{
    private decimal _tmpSum;
    private decimal _tmpDiscount;
    private readonly List<string> _log = new();
    private readonly DiscountPolicy _discountPolicy = new();
    private readonly ShippingPolicy _shippingPolicy = new();
    private readonly ReportBuilder _reportBuilder = new();

    public string Handle(int docId, int clientId,
        string clientName, string? clientMail,
        string clientKind, int clientDone,
        List<PrescriptionLine>? items, string state,
        string currency, DateTime createdAt, bool sendMail)
    {
        if (items is null)
        {
            return "ERR: null";
        }

        if (items.Count == 0)
        {
            return "ERR: empty";
        }

        if (LegacyTypeParser.StateOf(state) is not (DocState.New or DocState.Paid))
        {
            return "ERR: state";
        }

        if (clientMail is null || !clientMail.Contains("@"))
        {
            return "ERR: mail";
        }

        _log.Add("ok " + docId);

        _tmpSum = Subtotal(items);
        ClientKind kind = LegacyTypeParser.KindOf(clientKind);
        _tmpDiscount = _discountPolicy.For(_tmpSum, kind, clientDone);
        decimal ship = _shippingPolicy.For(_tmpSum - _tmpDiscount);
        decimal total = _tmpSum - _tmpDiscount + ship;

        string txt = _reportBuilder.Build(
            docId, clientName, items, currency, _tmpDiscount, ship, total);

        if (sendMail)
        {
            _log.Add("mail -> " + clientMail);
        }
        return txt;
    }

    public decimal Preview(string clientKind, int clientDone,
        List<PrescriptionLine> items)
    {
        decimal subtotal = Subtotal(items);
        ClientKind kind = LegacyTypeParser.KindOf(clientKind);
        decimal discount = _discountPolicy.For(subtotal, kind, clientDone);
        decimal shipping = _shippingPolicy.For(subtotal - discount);
        return subtotal - discount + shipping;
    }


    private static decimal Subtotal(List<PrescriptionLine> items)
    {
        decimal sum = 0m;
        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Qty * items[i].Price;
        }
        return sum;
    }

    public string DescribeClient(PatientRecord c)
    {
        ClientKind kind = LegacyTypeParser.KindOf(c.Kind);
        string s = c.Name.Trim().ToUpper();
        if (kind == ClientKind.Vip) s += " [VIP]";
        if (c.DoneCount > PricingRules.LoyalOrders) s += " [ЛОЯЛЬНИЙ]";
        s += " <" + c.Email.ToLower() + ">";
        int years = DateTime.Now.Year - c.SinceUtc.Year;
        s += " стаж " + years;
        return s;
    }

    public string DumpLog()
    {
        string r = "";
        foreach (var l in _log) r += l + "\n";
        return r;
    }
}
