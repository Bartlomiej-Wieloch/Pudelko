﻿using PudelkoLib;
using System.Globalization;
using static PudelkoLib.Pudelko;

namespace PudelkoUnitTests;

[TestClass]
public sealed class Test
{
    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }
}

[TestClass]
public class UnitTestsPudelkoConstructors
{
    private static double defaultSize = 0.1; // w metrach
    private static double accuracy = 0.001; //dokładność 3 miejsca po przecinku

    private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
    {
        Assert.AreEqual(expectedA, p.A, delta: accuracy);
        Assert.AreEqual(expectedB, p.B, delta: accuracy);
        Assert.AreEqual(expectedC, p.C, delta: accuracy);
    }

    #region Constructor tests ================================

    [TestMethod, TestCategory("Constructors")]
    public void Constructor_Default()
    {
        Pudelko p = new Pudelko();

        Assert.AreEqual(defaultSize, p.A, delta: accuracy);
        Assert.AreEqual(defaultSize, p.B, delta: accuracy);
        Assert.AreEqual(defaultSize, p.C, delta: accuracy);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1,
             1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005,
             1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
    public void Constructor_3params_DefaultMeters(double a, double b, double c,
                                                  double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1,
             1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005,
             1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
    public void Constructor_3params_InMeters(double a, double b, double c,
                                                  double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(100.0, 25.5, 3.1,
             1.0, 0.255, 0.031)]
    [DataRow(100.0, 25.58, 3.13,
             1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
    public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                  double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(100, 255, 3,
             0.1, 0.255, 0.003)]
    [DataRow(100.0, 25.58, 3.13,
             0.1, 0.025, 0.003)] // dla milimetrów nie liczą się miejsca po przecinku
    public void Constructor_3params_InMilimeters(double a, double b, double c,
                                                 double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }


    // ----

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.5, 1.0, 2.5)]
    [DataRow(1.001, 2.599, 1.001, 2.599)]
    [DataRow(1.0019, 2.5999, 1.001, 2.599)]
    public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(a, b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.5, 1.0, 2.5)]
    [DataRow(1.001, 2.599, 1.001, 2.599)]
    [DataRow(1.0019, 2.5999, 1.001, 2.599)]
    public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(a: a, b: b, unit: UnitOfMeasure.meter);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(11.0, 2.5, 0.11, 0.025)]
    [DataRow(100.1, 2.599, 1.001, 0.025)]
    [DataRow(2.0019, 0.25999, 0.02, 0.002)]
    public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a, b: b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(11, 2.0, 0.011, 0.002)]
    [DataRow(100.1, 2599, 0.1, 2.599)]
    [DataRow(200.19, 2.5999, 0.2, 0.002)]
    public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    // -------

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(2.5)]
    public void Constructor_1param_DefaultMeters(double a)
    {
        Pudelko p = new Pudelko(a);

        Assert.AreEqual(a, p.A);
        Assert.AreEqual(0.1, p.B);
        Assert.AreEqual(0.1, p.C);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(2.5)]
    public void Constructor_1param_InMeters(double a)
    {
        Pudelko p = new Pudelko(a);

        Assert.AreEqual(a, p.A);
        Assert.AreEqual(0.1, p.B);
        Assert.AreEqual(0.1, p.C);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(11.0, 0.11)]
    [DataRow(100.1, 1.001)]
    [DataRow(2.0019, 0.02)]
    public void Constructor_1param_InCentimeters(double a, double expectedA)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a);

        AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(11, 0.011)]
    [DataRow(100.1, 0.1)]
    [DataRow(200.19, 0.2)]
    public void Constructor_1param_InMilimeters(double a, double expectedA)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a);

        AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
    }

    // ---

    public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

    [DataTestMethod, TestCategory("Constructors")]
    [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1, 1, 1)]
    [DataRow(1, -1, 1)]
    [DataRow(1, 1, -1)]
    [DataRow(-1, -1, 1)]
    [DataRow(-1, 1, -1)]
    [DataRow(1, -1, -1)]
    [DataRow(-1, -1, -1)]
    [DataRow(0, 1, 1)]
    [DataRow(1, 0, 1)]
    [DataRow(1, 1, 0)]
    [DataRow(0, 0, 1)]
    [DataRow(0, 1, 0)]
    [DataRow(1, 0, 0)]
    [DataRow(0, 0, 0)]
    [DataRow(0.01, 0.1, 1)]
    [DataRow(0.1, 0.01, 1)]
    [DataRow(0.1, 0.1, 0.01)]
    [DataRow(1001, 1, 1)]
    [DataRow(1, 1001, 1)]
    [DataRow(1, 1, 1001)]
    [DataRow(1001, 1, 1001)]
    [DataRow(1, 1001, 1001)]
    [DataRow(1001, 1001, 1)]
    [DataRow(1001, 1001, 1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);
    }


    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1, 1, 1)]
    [DataRow(1, -1, 1)]
    [DataRow(1, 1, -1)]
    [DataRow(-1, -1, 1)]
    [DataRow(-1, 1, -1)]
    [DataRow(1, -1, -1)]
    [DataRow(-1, -1, -1)]
    [DataRow(0, 1, 1)]
    [DataRow(1, 0, 1)]
    [DataRow(1, 1, 0)]
    [DataRow(0, 0, 1)]
    [DataRow(0, 1, 0)]
    [DataRow(1, 0, 0)]
    [DataRow(0, 0, 0)]
    [DataRow(0.1, 1, 1)] /*w zadaniu nie ma mowy o minimalnej wartości milimetra, */
    [DataRow(1, 0.1, 1)] /*jest podane tylko że bok ma być mniejszy niż 10 metrów i większy od 0*/
    [DataRow(1, 1, 0.1)]
    [DataRow(10001, 1, 1)]
    [DataRow(1, 10001, 1)]
    [DataRow(1, 1, 10001)]
    [DataRow(10001, 10001, 1)]
    [DataRow(10001, 1, 10001)]
    [DataRow(1, 10001, 10001)]
    [DataRow(10001, 10001, 10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);
    }


    public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

    [DataTestMethod, TestCategory("Constructors")]
    [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.meter);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1, 1)]
    [DataRow(1, -1)]
    [DataRow(-1, -1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(0, 0)]
    [DataRow(0.01, 1)]
    [DataRow(1, 0.01)]
    [DataRow(0.01, 0.01)]
    [DataRow(1001, 1)]
    [DataRow(1, 1001)]
    [DataRow(1001, 1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.centimeter);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1, 1)]
    [DataRow(1, -1)]
    [DataRow(-1, -1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(0, 0)]
    [DataRow(0.1, 1)]
    [DataRow(1, 0.1)]
    [DataRow(0.1, 0.1)]
    [DataRow(10001, 1)]
    [DataRow(1, 10001)]
    [DataRow(10001, 10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.milimeter);
    }




    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(10.1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(10.1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: UnitOfMeasure.meter);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(0.01)]
    [DataRow(1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: UnitOfMeasure.centimeter);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(0.1)]
    [DataRow(10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: UnitOfMeasure.milimeter);
    }



    #endregion

    #region ToString tests ===================================

    [TestMethod, TestCategory("String representation")]
    public void ToString_Default_Culture_EN()
    {
        var p = new Pudelko(2.5, 9.321);
        string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

        Assert.AreEqual(expectedStringEN, p.ToString());
    }

    [DataTestMethod, TestCategory("String representation")]
    [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
    [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
    [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
    [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
    public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
    {
        var p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
        Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
    }

    [TestMethod, TestCategory("String representation")]
    [ExpectedException(typeof(FormatException))]
    public void ToString_Formattable_WrongFormat_FormatException()
    {
        var p = new Pudelko(1);
        var stringformatedrepreentation = p.ToString("wrong code");
    }

    #endregion

    #region Pole, Objętość ===================================

    [TestMethod, TestCategory("Objętość")]
    public void Objetosc_Property_RoundsCorrectly_Down()
    {
        var p = new Pudelko(0.7777, 0.7777, 0.7777);
        Assert.AreEqual(p.Objetosc, 0.470366406);
    }

    [TestMethod, TestCategory("Objętość")]
    public void Objetosc_Property_RoundsCorrectly_Up()
    {
        var p = new Pudelko(0.1111, 0.2221, 0.3331);
        Assert.AreEqual(p.Objetosc, 0.008219346);
    }

    [TestMethod, TestCategory("Pole")]
    public void Pole_Property_RoundsCorrectly_Up()
    {
        var p = new Pudelko(0.1111, 0.2222, 0.7777);
        Assert.AreEqual(p.Pole, 0.567788);
    }

    [TestMethod, TestCategory("Pole")]
    public void Pole_Property_RoundsCorrectly_Down()
    {
        var p = new Pudelko(0.1111, 0.2222, 0.3331);
        Assert.AreEqual(p.Pole, 0.271417);
    }

    #endregion
}