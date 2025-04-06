using PudelkoLib;
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

    [TestMethod, TestCategory("Constructors")]
    public void Constructor_Default()
    {
        Pudelko p = new Pudelko();

        Assert.AreEqual(defaultSize, p.A, delta: accuracy);
        Assert.AreEqual(defaultSize, p.B, delta: accuracy);
        Assert.AreEqual(defaultSize, p.C, delta: accuracy);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1, 1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005, 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
    public void Constructor_3params_DefaultMeters(double a, double b, double c, double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(-1.0, 2.543, 3.1)]
    [DataRow(1.0, 0, 3.1)]
    [DataRow(1.0, 2.543, 0)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3paramsOneTooSmall_Exception(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 12.543, 3.1)]
    [DataRow(1.0, 13, 13)]
    [DataRow(13, 13, 13)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3paramsTooBig_Exception(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1, 1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005, 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
    public void Constructor_3params_InMeters(double a, double b, double c, double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(100.0, 25.5, 3.1, 1.0, 0.255, 0.031)]
    [DataRow(100.0, 25.58, 3.13, 1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
    public void Constructor_3params_InCentimeters(double a, double b, double c, double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod, TestCategory("Constructors")]
    [DataRow(100, 255, 3, 0.1, 0.255, 0.003)]
    [DataRow(100.0, 25.58, 3.13, 0.1, 0.025, 0.003)] // dla milimetrów nie liczą się miejsca po przecinku
    public void Constructor_3params_InMilimeters(double a, double b, double c, double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }
}

