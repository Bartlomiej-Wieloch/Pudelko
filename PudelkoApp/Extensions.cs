using PudelkoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoApp;

public static class Extensions
{
    public static Pudelko Kompresuj(this Pudelko p)
    {
        if (p == null)
        {
            throw new ArgumentNullException("Pudelko nie może być null");
        }
        double bokA = Math.Pow(p.Objetosc, 1.0 / 3.0);
        return new Pudelko(bokA, bokA, bokA);
    }
}
