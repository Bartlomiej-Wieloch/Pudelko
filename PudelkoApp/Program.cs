using PudelkoLib;
using System.Globalization;
using System.Net.WebSockets;


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

Console.WriteLine("\nUtwórz nową tablicę z pudełka p1");

double[] tablica = ((double[])p2);

foreach (var item in tablica)
{
    Console.WriteLine(item);
}

Console.WriteLine("\nUtwórz nowe pudełko p3 z krotki ValueTuple<int,int,int>. Wymiary domyślnie w milimetrach");
var ValueTuple = (10, 10, 10);
Pudelko p3 = ValueTuple;
Console.WriteLine($"p3 = {p3.ToString()}\n");

Console.WriteLine("Utwórz nowe pudełko p4 z stringa w formacie \"«liczba» «jednostka» × «liczba» «jednostka» × «liczba» «jednostka»\"");
string s = new string("2500 mm × 9321 mm × 100 mm");
Console.WriteLine($"string s = {s}");

Pudelko p4 = Pudelko.Parse(s);
Console.WriteLine($"p4 = {p4.ToString()}\n");


