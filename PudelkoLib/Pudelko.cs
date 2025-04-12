using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;
using static PudelkoLib.Pudelko.UnitOfMeasure;

namespace PudelkoLib
{
    public sealed class Pudelko
    {
        private readonly double _a, _b, _c;
        private readonly UnitOfMeasure _unit;

        public enum UnitOfMeasure
        {
            meter, //ustaw format na ... 0.000 -- dla metrów liczą się 3 miejsca po przecinku
            centimeter, //ustaw format na ... 000.0 -- dla centymertów liczy się tylko 1 miejsce po przecinku
            milimeter //ustaw format na ... 0000 -- dla milimetrów nie liczą się miejsca po przecinku
        }

        private static double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być wartością dodatnią");

            if ((value <= 0.1) && unit == milimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być większy od 0.1 milimetra");

            if ((value <= 0.01) && unit == centimeter)
                throw new ArgumentOutOfRangeException("Bok pudełka musi być większy od 0.01 centymetra");

            switch (unit)
            {
                case meter:
                    return value;
                case centimeter:
                    return value / 100;
                case milimeter:
                    return value / 1000;
                default:
                    throw new ArgumentOutOfRangeException("Błędna jednostka");
                    //Nie da się ustawić innej jednostki niż zdefiniowane w UnitOfMeasure,
                    //ale bez tego jest błąd metody, "not all path return a value
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


    }
}
