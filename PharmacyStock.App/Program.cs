using System.Text;
using PharmacyStock.Core.Domain;

Console.OutputEncoding = Encoding.UTF8;

var z = new recept("R-1001", "Іваненко");

z.Add("MED-1", 3, 250m);
z.Add("MED-2", 12, 90m);

Console.WriteLine(z.ProcessData(false));
Console.WriteLine(z.ProcessData(true));
Console.WriteLine(z.Ck());
Console.WriteLine(z.Chg(1));
Console.Write(z.Rep());