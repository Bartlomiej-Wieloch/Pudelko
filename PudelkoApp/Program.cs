using PudelkoLib;
using System.Globalization;
using System.Net.WebSockets;
using static PudelkoApp.Extensions;


internal class Program
{
    private static int ComparePudelka(Pudelko p1, Pudelko p2)
    {
        // 1. Kryterium: Objętość
        int volumeComparison = p1.Objetosc.CompareTo(p2.Objetosc);
        if (volumeComparison != 0)
        {
            return volumeComparison;
        }

        // 2. Kryterium (jeśli objętości równe): Pole powierzchni
        int areaComparison = p1.Pole.CompareTo(p2.Pole);
        if (areaComparison != 0)
        {
            return areaComparison;
        }

        // 3. Kryterium (jeśli objętości i pola równe): Suma wymiarów
        double sum1 = p1.A + p1.B + p1.C;
        double sum2 = p2.A + p2.B + p2.C;
        int sumComparison = sum1.CompareTo(sum2);
        return sumComparison;
    }

    private static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

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
        Console.WriteLine(p1 == p2);

        Console.WriteLine("\nUtwórz nową tablicę z pudełka p1");

        double[] tablica = (double[])p2;

        foreach (var item in tablica)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\nUtwórz nowe pudełko p3 z krotki ValueTuple<int,int,int>. Wymiary domyślnie w milimetrach");
        var ValueTuple = (113, 113, 113);
        Pudelko p3 = ValueTuple;
        Console.WriteLine($"p3 = {p3.ToString()}\n");

        Console.WriteLine("Utwórz nowe pudełko p4 z stringa w formacie \"«liczba» «jednostka» × «liczba» «jednostka» × «liczba» «jednostka»\"");
        string s = new string("2200 mm × 9721 mm × 10 mm");
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

        Console.WriteLine("\nMetoda Kompresuj() pozwala na skompresowania pudełka aby stał się sześcianem o takiej samej objętośći");
        Pudelko p7 = new Pudelko(1.0, 2.0, 4.0);
        Console.WriteLine($"p7 = {p7.ToString()}");
        Console.WriteLine($"objetosc p7 = {p7.Objetosc}");
        Pudelko p8 = p7.Kompresuj();
        Console.WriteLine($"p8 = {p8.ToString()}");
        Console.WriteLine($"objetosc p8 = {p8.Objetosc}\n");

        List<Pudelko> pudelka = new List<Pudelko>();
        pudelka.Add(p7);
        pudelka.Add(p1);
        pudelka.Add(p3);
        pudelka.Add(new Pudelko(2, 2, 4));
        pudelka.Add(p4);
        pudelka.Add(p8);
        pudelka.Add(new Pudelko(1, 4, 4));
        pudelka.Add(p2);


        Console.WriteLine("Przed sortowaniem :\n");
        Console.WriteLine($"| {"Pudełko",-27} {"| Objętość [m^3]",-16} {"| Pole całkowite [m^2]",-21} {"| Suma wymiarów [m]",-14}");
        foreach (var p in pudelka)
        {
            Console.WriteLine($"| {p,-20} | {p.Objetosc,-15}| {p.Pole,-20} | {p.A + p.B + p.C,-15}");
        }
        Console.WriteLine("\nPo sortowaniu :\n");

        Comparison<Pudelko> comparisonDelegate = ComparePudelka;
        pudelka.Sort(comparisonDelegate);

        Console.WriteLine($"| {"Pudełko",-27} {"| Objętość [m^3]",-16} {"| Pole całkowite [m^2]",-21} {"| Suma wymiarów [m]",-14}");
        foreach (var p in pudelka)
        {
            Console.WriteLine($"| {p,-20} | {p.Objetosc,-15}| {p.Pole,-20} | {p.A + p.B + p.C,-15}");
        }
    }
}