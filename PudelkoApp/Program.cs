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

Console.WriteLine("Utwórz nowe pudełko które zmieści dwa inne, będąc przy tym możliwie najmniejsze");
var p5 = new Pudelko(1, 2, 1);
var p6 = new Pudelko(1, 1, 3);

Console.WriteLine($"Pudełko p5 = {p5.ToString()}");
Console.WriteLine($"Pudełko p6 = {p6.ToString()}");

Pudelko p56 = p5 + p6;

Console.WriteLine($"Najmniejsze pudełko mieszczące oba = {p56.ToString()}\n");

Console.WriteLine("Za pomocą indeksera możemy dostać się do dowolnego wymiaru pudełka p56 (w metrach)");
Console.WriteLine($"Wymiar o indeksie 2 = {p56[2]:F3} m\n");

Console.WriteLine("Wymiary pudełka p56 za pomocą pętli foreach:");
foreach (var dimension in p56)
{
    Console.WriteLine($"- {dimension:F3} m");
}