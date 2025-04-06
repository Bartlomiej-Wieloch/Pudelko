using System;
using System.Threading.Channels;
using static PudelkoLib.Pudelko.UnitOfMeasure;

namespace PudelkoLib
{
    public sealed class Pudelko
    {
        private double _a, _b, _c;
        private UnitOfMeasure _unit;

        public Pudelko (double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = meter)
        {
            if ((a <= 0) || (b <= 0) || (c <= 0))
                throw new ArgumentOutOfRangeException("Bok pudełka musi być wartością dodatnią");

            if (((a > 10) || (b > 10) || (c > 10)) && unit == meter)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być mniejszy od 10 metrów");

            _a = a;
            _b = b;
            _c = c;
            _unit = unit;
        }

        public double A => _a;
        public double B => _b;
        public double C => _c;
        public UnitOfMeasure Unit => _unit;

        public enum UnitOfMeasure
        {
            meter,
            centimeter,
            milimeter
        }
    }
}
