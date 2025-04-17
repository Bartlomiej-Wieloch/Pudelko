using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System.Threading.Channels;
using static PudelkoLib.Pudelko.UnitOfMeasure;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private static readonly CultureInfo cInfo = new CultureInfo("en-US");

        private readonly double _a, _b, _c;
        private readonly UnitOfMeasure _unit;

        public enum UnitOfMeasure
        {
            meter,      //ustaw format na ... 0.000 -- dla metrów liczą się 3 miejsca po przecinku
            centimeter, //ustaw format na ... 000.0 -- dla centymertów liczy się tylko 1 miejsce po przecinku
            milimeter   //ustaw format na ... 0000 -- dla milimetrów nie liczą się miejsca po przecinku
        }

        private static double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być wartością dodatnią");

            if ((value < 1) && unit == milimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka nie może być mniejszy od 1 milimetra");

            if ((value < 0.1) && unit == centimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka nie może być mniejszy od 0.1 centymetra");

            switch (unit)
            {
                case meter:
                    return value;
                case centimeter:
                    return value / 100;
                case milimeter:
                    return value / 1000;
                default:
                    throw new FormatException("Błędna jednostka");
                    //Nie da się ustawić innej jednostki niż zdefiniowane w UnitOfMeasure,
                    //ale bez tego jest błąd metody, "not all path return a value"
            }
        }

        /*
         * taka implementacja jest lepsza niż na sztywno przypisanie wartości warunkowej = 0.1 
         * Gdy wybierzemy inną jednostkę niż metr i zostawimy pare argumentów pustych, 
         * pozostałe wartości też będą przeliczone (np. 0.1 /100) 
         * co nie jest tym co chcemy osiągnąć
         */
        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = meter)
        {
            double aM = a.HasValue ? ConvertToMeters(a.Value, unit) : 0.1;
            double bM = b.HasValue ? ConvertToMeters(b.Value, unit) : 0.1;
            double cM = c.HasValue ? ConvertToMeters(c.Value, unit) : 0.1;

            if ((aM > 10) || (bM > 10) || (cM > 10))
                throw new ArgumentOutOfRangeException("Bok pudełka musi być mniejszy od 10 metrów");

            _a = aM;
            _b = bM;
            _c = cM;

            _unit = unit;
        }

        public double A => _a;
        public double B => _b;
        public double C => _c;
        public UnitOfMeasure Unit => _unit;

        public override string ToString()
        {
            return ToString("m", null);
        }

        public string ToString(string format, IFormatProvider? provider = null)
        {
            if (String.IsNullOrEmpty(format)) format = "m";

            if (provider == null)
            {
                provider = cInfo;
            }

            switch (format.ToLowerInvariant())
            {
                case "m":
                    return string.Format(provider, "{0:F3} m × {1:F3} m × {2:F3} m", A, B, C);

                case "cm":
                    var cmA = A * 100;
                    var cmB = B * 100;
                    var cmC = C * 100;
                    return string.Format(provider, "{0:F1} cm × {1:F1} cm × {2:F1} cm", cmA, cmB, cmC);

                case "mm":
                    var mmA = A * 1000;
                    var mmB = B * 1000;
                    var mmC = C * 1000;
                    return string.Format(provider, "{0:F0} mm × {1:F0} mm × {2:F0} mm", mmA, mmB, mmC);

                default:
                    throw new FormatException("Błędna jednostka");
            }
        }

        public bool Equals(Pudelko other)
        {
            if (other is null) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            if ((_a == other._a && _b == other._b && _c == other._c) ||
                (_a == other._b && _b == other._a && _c == other._c) ||
                (_a == other._c && _b == other._a && _c == other._b) ||
                (_a == other._c && _b == other._b && _c == other._a) ||
                (_a == other._b && _b == other._c && _c == other._a) ||
                (_a == other._a && _b == other._c && _c == other._b))
                return true;

            //if ((_a == other._a && _b == other._b && _c == other._c)) return true;
            //if ((_a == other._b && _b == other._a && _c == other._c)) return true;
            //if ((_a == other._c && _b == other._a && _c == other._b)) return true;
            //if ((_a == other._c && _b == other._b && _c == other._a)) return true;
            //if ((_a == other._b && _b == other._c && _c == other._a)) return true;
            //if ((_a == other._a && _b == other._c && _c == other._b)) return true;

            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko)
                return Equals((Pudelko)obj);
            else
                return false;
        }

        public static bool Equals(Pudelko p1, Pudelko p2)
        {
            if ((p1 is null) && (p2 is null)) return true;
            if (p1 is null) return false;

            return p1.Equals(p2);
        }

        public override int GetHashCode()
        {
            var dimensions = new List<double> { A, B, C };
            dimensions.Sort();

            HashCode hash = new HashCode();
            hash.Add(dimensions[0]);
            hash.Add(dimensions[1]);
            hash.Add(dimensions[2]);
            return hash.ToHashCode();
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round(2 * (A * B + A * C + B * C), 6);

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            if (p1 is null || p2 is null)
                throw new ArgumentNullException("Nie można dodać pustego pudełka");

            double a1 = p1.A, b1 = p1.B, c1 = p1.C;
            double a2 = p2.A, b2 = p2.B, c2 = p2.C;

            double box1_A = a1 + a2;
            double box1_B = Math.Max(b1, b2);
            double box1_C = Math.Max(c1, c2);
            double vol1 = box1_A * box1_B * box1_C;

            double box2_A = Math.Max(a1, a2);
            double box2_B = b1 + b2;
            double box2_C = Math.Max(c1, c2);
            double vol2 = box2_A * box2_B * box2_C;

            double box3_A = Math.Max(a1, a2);
            double box3_B = Math.Max(b1, b2);
            double box3_C = c1 + c2;
            double vol3 = box3_A * box3_B * box3_C;

            if (vol1 <= vol2 && vol1 <= vol3)
            {
                return new Pudelko(box1_A, box1_B, box1_C);
            }
            else if (vol2 <= vol1 && vol2 <= vol3)
            {
                return new Pudelko(box2_A, box2_B, box2_C);
            }
            else
            {
                return new Pudelko(box3_A, box3_B, box3_C);
            }
        }

        public static explicit operator double[](Pudelko p)
            => new double[] { p.A, p.B, p.C };

        public static implicit operator Pudelko(ValueTuple<int, int, int> valueTuple)
        {
            int aV = valueTuple.Item1;
            int bV = valueTuple.Item2;
            int cV = valueTuple.Item3;
            return new Pudelko(aV, bV, cV, milimeter);
        }

        public static Pudelko Parse(string input)
        {
            //P.Parse("2.500 m × 9.321 m × 0.100 m")
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input nie może być pusty");

            string[] parts = input.Split(new string[] { " × " }, StringSplitOptions.TrimEntries);
            if(parts.Length != 3)
                throw new FormatException("Nie poprawny format. " +
                    "Spodziewano się «liczba» «jednostka» × «liczba» «jednostka» × «liczba» «jednostka»");

            UnitOfMeasure? determinedUnit = null;
            double[] dimensions = new double[3];
            for (int i = 0; i < parts.Length; i++)
            {
                int spaceIndex = parts[i].LastIndexOf(' ');

                if (spaceIndex < 0 || spaceIndex == parts[i].Length - 1)
                    throw new FormatException("Nie poprawny format. " +
                        "Spodziewano się «liczba» «jednostka» × «liczba» «jednostka» × «liczba» «jednostka»");

                string number = parts[i].Substring(0, spaceIndex);
                string unit = parts[i].Substring(spaceIndex + 1);

                if (double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    dimensions[i] = result;
                }
                else
                {
                    throw new FormatException("Błąd parsowania liczby ze stringa");
                }
                if (determinedUnit == null) 
                {
                    if (unit.Equals("m", StringComparison.OrdinalIgnoreCase))
                    {
                        determinedUnit = UnitOfMeasure.meter;
                    }
                    else if (unit.Equals("cm", StringComparison.OrdinalIgnoreCase))
                    {
                        determinedUnit = UnitOfMeasure.centimeter;
                    }
                    else if (unit.Equals("mm", StringComparison.OrdinalIgnoreCase))
                    {
                        determinedUnit = UnitOfMeasure.milimeter;
                    }
                    else
                    {

                        throw new FormatException($"Nieznana jednostka '{unit}'.");
                    }
                }
            }
            return new Pudelko(dimensions[0], dimensions[1], dimensions[2], determinedUnit.Value);
        }

        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new IndexOutOfRangeException()
                };
            }

        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
