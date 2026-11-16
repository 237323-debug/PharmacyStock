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

    /// <summary>
    /// Describes the patient using the original legacy text format.
    /// </summary>
    public string Describe()
    {
        ClientKind kind = LegacyTypeParser.KindOf(Kind);
        string s = Name.Trim().ToUpper();
        if (kind == ClientKind.Vip) s += " [VIP]";
        if (DoneCount > PricingRules.LoyalOrders) s += " [ЛОЯЛЬНИЙ]";
        s += " <" + Email.ToLower() + ">";
        int years = DateTime.Now.Year - SinceUtc.Year;
        s += " стаж " + years;
        return s;
    }
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

    public string Handle(PrescriptionRequest request)
    {
        List<PrescriptionLine>? items = request.Items;
        if (items is null)
        {
            return "ERR: null";
        }

        if (items.Count == 0)
        {
            return "ERR: empty";
        }

        if (request.State is not (DocState.New or DocState.Paid))
        {
            return "ERR: state";
        }

        if (request.Patient.Email is null || !request.Patient.Email.Contains("@"))
        {
            return "ERR: mail";
        }

        _log.Add("ok " + request.DocId);

        _tmpSum = Subtotal(items);
        ClientKind kind = LegacyTypeParser.KindOf(request.Patient.Kind);
        _tmpDiscount = _discountPolicy.For(_tmpSum, kind, request.Patient.DoneCount);
        decimal ship = _shippingPolicy.For(_tmpSum - _tmpDiscount);
        decimal total = _tmpSum - _tmpDiscount + ship;

        string txt = _reportBuilder.Build(
            request.DocId, request.Patient.Name, items, request.Currency,
            _tmpDiscount, ship, total);

        if (request.SendMail)
        {
            _log.Add("mail -> " + request.Patient.Email);
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

    public string DumpLog()
    {
        string r = "";
        foreach (var l in _log) r += l + "\n";
        return r;
    }
}
