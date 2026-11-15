using System.Text;
using PharmacyStock.Core.Domain;

Console.OutputEncoding = Encoding.UTF8;

var prescription = new PrescriptionRecord("R-1001", "Черевко");

prescription.AddLine("MED-1", 3, 250m);
prescription.AddLine("MED-2", 12, 90m);

Console.WriteLine(prescription.CalculateTotal(false));
Console.WriteLine(prescription.CalculateTotal(true));
Console.WriteLine(prescription.IsValid());
Console.WriteLine(prescription.TryChangeStatus(1));
Console.Write(prescription.BuildReport());
