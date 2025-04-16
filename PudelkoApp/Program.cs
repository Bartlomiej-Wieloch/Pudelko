using PudelkoLib;
using System.Globalization;


var p1 = new Pudelko(2.5, 9.321, 0.1);

Console.WriteLine("p1 = " + p1.ToString());
Console.WriteLine("1. " + p1.ToString("m"));
Console.WriteLine("2. " + p1.ToString("cm"));
Console.WriteLine("3. " + p1.ToString("mm"));
Console.WriteLine();

Console.WriteLine("Objętość -> " + p1.Objetosc + " m^3");
Console.WriteLine("Pole -> " + p1.Pole + " m^2");
Console.WriteLine();

var p2 = new Pudelko(10, 932.1, 250, Pudelko.UnitOfMeasure.centimeter);
Console.WriteLine("p2 = " + p2.ToString("cm"));
Console.WriteLine();

Console.WriteLine("Czy p1 = p2 ?");
Console.WriteLine(p1==p2);

