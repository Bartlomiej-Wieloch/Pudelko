using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System.Threading.Channels;
using static PudelkoLib.Pudelko.UnitOfMeasure;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable
    {
        CultureInfo cInfo = new CultureInfo("en-US");

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

            if ((value <= 0.9) && unit == milimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być większy od 0.9 milimetra");

            if ((value <= 0.09) && unit == centimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być większy od 0.09 centymetra");

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
         * Gdy wybierzemy inną jednostkę niż metr i zostawimy pare argumentów pustych, pozostałe wartości
         * też będą przeliczone (np. 0.1 /100) co nie jest tym co chcemy osiągnąć
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

        //public string ToString(string format)
        //{
        //    return ToString(format, cInfo);
        //}

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
    }
}
