using PudelkoLib;
using System.Globalization;


var pudelko = new Pudelko(2.5, 9.321, 0.1);

Console.WriteLine("0. " + pudelko.ToString());
Console.WriteLine("1. " + pudelko.ToString("m"));
Console.WriteLine("2. " + pudelko.ToString("cm"));
Console.WriteLine("3. " + pudelko.ToString("mm"));

Console.WriteLine("Objętość -> " + pudelko.Objetosc + " m^3");
Console.WriteLine("Pole -> " + pudelko.Pole + " m^2");


