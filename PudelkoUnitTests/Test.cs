using PudelkoLib;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using static PudelkoLib.Pudelko;
//using static PudelkoLib.Pudelko.UnitOfMeasure;

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


    #region Equals ===========================================

    [TestMethod, TestCategory("Equals")]
    public void Equals_OtherIsNull_False()
    {
        var p = new Pudelko();
        bool condition = p.Equals(null);

        Assert.IsFalse(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_OtherIsTheSameObject_True()
    {
        var p = new Pudelko();
        bool condition = p.Equals(p);

        Assert.IsTrue(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_PudelkoesHaveTheSameDimensions_True()
    {
        var p = new Pudelko();
        var p2 = new Pudelko();

        bool condition = p.Equals(p2);
        Assert.IsTrue(condition);
    }

    [DataTestMethod, TestCategory("Equals")]
    [DataRow(1.0, 2.543, 3.1)]
    [DataRow(1.0, 3.1, 2.543)]
    [DataRow(2.543, 1.0, 3.1)]
    [DataRow(2.543, 3.1, 1.0)]
    [DataRow(3.1, 1.0, 2.543)]
    [DataRow(3.1, 2.543, 1.0)]
    public void Equals_BoxesHaveTheSameDimensionsButAreRotated_True(double a, double b, double c)
    {
        var p = new Pudelko(1.0, 2.543, 3.1);
        var p2 = new Pudelko(a, b, c);

        bool condition = p.Equals(p2);

        Assert.IsTrue(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_BoxesDontHaveTheSameDimensions_False()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);
        var p2 = new Pudelko(1.0, 2.543, 3);

        bool condition = p.Equals(p2);

        Assert.IsFalse(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_StaticMethodTwoBoxes_False()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);
        var p2 = new Pudelko(1.0, 2.543, 3);

        bool condition = Pudelko.Equals(p, p2);

        Assert.IsFalse(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_BothObjectsAreNull_True()
    {
        //var p = new Pudelko(null);
        //var p2 = new Pudelko(null);

        bool condition = Pudelko.Equals(null, null);
        //bool condition = Pudelko.Equals(p, p2);

        Assert.IsTrue(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_OneObjectIsNull_False()
    {
        var p = new Pudelko(1, 2, 3, UnitOfMeasure.centimeter);

        bool condition = Pudelko.Equals(null, p);

        Assert.IsFalse(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_OneObjectIsBox_True()
    {
        var p = new Pudelko(1, 2, 3);
        object p2 = new Pudelko(1, 2, 3);

        bool condition = p.Equals(p2);

        Assert.IsTrue(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_OneObjectIsBox_False()
    {
        var p = new Pudelko(1, 2, 3);
        object p2 = new Pudelko(1, 3, 3);

        bool condition = p.Equals(p2);

        Assert.IsFalse(condition);
    }

    [TestMethod, TestCategory("Equals")]
    public void Equals_OneObjectIsNotBox_False()
    {
        var p = new Pudelko(1, 2, 3);
        object p2 = new string ("Im not a box!");

        bool condition = p.Equals(p2);

        Assert.IsFalse(condition);
    }

    #endregion

    #region GetHashCode ===========================

    [TestMethod, TestCategory("GetHashCode")]
    public void GetHashCode_RotatedBox_True()
    {
        var p = new Pudelko(1, 2, 3);
        var p2 = new Pudelko(2, 3, 1);

        Assert.AreEqual(p.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod, TestCategory("GetHashCode")]
    public void GetHashCode_DifferentBox_False()
    {
        var p = new Pudelko(1, 2, 3);
        var p2 = new Pudelko(1, 3, 1);

        Assert.AreNotEqual(p.GetHashCode(), p2.GetHashCode());
    }

    #endregion

    #region Operators overloading ===========================

    [TestMethod, TestCategory("Operators")]
    public void Operators_Equals_True()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);
        var p2 = new Pudelko(1.0, 2.543, 3.1);

        bool condition = Pudelko.Equals(p, p2);

        Assert.IsTrue(p==p2);
    }

    [TestMethod, TestCategory("Operators")]
    public void Operators_Equals_False()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);
        var p2 = new Pudelko(1.0, 2.543, 3);

        bool condition = Pudelko.Equals(p, p2);

        Assert.IsTrue(p != p2);
    }

    [TestMethod, TestCategory("Operators")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Operators_AddingNullBox_Excepiton()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);

        var actual = p + null;
    }

    [TestMethod, TestCategory("Operators")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Operators_AddingNullBox2_Excepiton()
    {
        var p = new Pudelko(1.0, 2.543, 3.1);

        var actual = null + p;
    }

    [TestMethod, TestCategory("Operators")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Operators_AddingNullBox3_Excepiton()
    {
        Pudelko p = null;

        var actual = null + p;
    }

    [TestMethod, TestCategory("Operators")]
    public void Operators_AddingTwoIdenticalCubes_ReturnsCorrectDimensions()
    {
        var p1 = new Pudelko(1, 1, 1);
        var p2 = new Pudelko(1, 1, 1);

        var actual = p1 + p2;

        Assert.AreEqual(2.0, actual.A);
        Assert.AreEqual(1.0, actual.B);
        Assert.AreEqual(1.0, actual.C);
    }

    [DataTestMethod, TestCategory("Operators")]
    [DataRow(1, 1, 1, 1, 1, 8, 1.0, 1.0, 9.0)]
    [DataRow(1, 2, 3, 4, 1, 2, 5.0, 2.0, 3.0)]
    [DataRow(1, 8, 1, 1, 1, 1, 1.0, 9.0, 1.0)]
    [DataRow(5, 5, 7, 1, 1, 1, 5.0, 5.0, 8.0)]
    [DataRow(4, 9, 1, 1, 1, 3, 4.0, 10.0, 3.0)]
    public void Operators_AddingTwoDifferentBoxes_ReturnsCorrectDimensions(double a1, double b1, double c1, double a2, double b2, double c2, double resultA, double resultB, double resultC)
    {
        var p1 = new Pudelko(a1, b1, c1);
        var p2 = new Pudelko(a2, b2, c2);

        var actual = p1 + p2;

        Assert.AreEqual(resultA, actual.A);
        Assert.AreEqual(resultB, actual.B);
        Assert.AreEqual(resultC, actual.C);
    }

    //[TestMethod, TestCategory("Operators")]
    //public void Operators_AddingTwoDifferentBoxes2_ReturnsCorrectDimensions()
    //{
    //    var p1 = new Pudelko(1, 2, 3);
    //    var p2 = new Pudelko(4, 1, 2);

    //    var actual = p1 + p2;

    //    Assert.AreEqual(5.0, actual.A);
    //    Assert.AreEqual(2.0, actual.B);
    //    Assert.AreEqual(3.0, actual.C);
    //}

    //[TestMethod, TestCategory("Operators")]
    //public void Operators_AddingTwoDifferentBoxes3_ReturnsCorrectDimensions()
    //{
    //    var p1 = new Pudelko(1, 8, 1);
    //    var p2 = new Pudelko(1, 1, 1);

    //    var actual = p1 + p2;

    //    Assert.AreEqual(1.0, actual.A);
    //    Assert.AreEqual(9.0, actual.B);
    //    Assert.AreEqual(1.0, actual.C);
    //}

    //[TestMethod, TestCategory("Operators")]
    //public void Operators_AddingTwoDifferentBoxes4_ReturnsCorrectDimensions()
    //{
    //    var p1 = new Pudelko(5, 5, 7);
    //    var p2 = new Pudelko(1, 1, 1);

    //    var actual = p1 + p2;

    //    Assert.AreEqual(5.0, actual.A);
    //    Assert.AreEqual(5.0, actual.B);
    //    Assert.AreEqual(8.0, actual.C);
    //}

    //[TestMethod, TestCategory("Operators")]
    //public void Operators_AddingTwoDifferentBoxes5_ReturnsCorrectDimensions()
    //{
    //    var p1 = new Pudelko(4, 9, 1);
    //    var p2 = new Pudelko(1, 1, 3);

    //    var actual = p1 + p2;

    //    Assert.AreEqual(4.0, actual.A);
    //    Assert.AreEqual(10.0, actual.B);
    //    Assert.AreEqual(3.0, actual.C);
    //}

    [TestMethod, TestCategory("Operators")]
    public void Operators_AddingTwoDifferentCubesInDifferentOrder_ReturnsCorrectDimensions()
    {
        var p1 = new Pudelko(1, 2, 3);
        var p2 = new Pudelko(4, 1, 2);

        var actual = p1 + p2;
        var actual2 = p2 + p1;

        Assert.AreEqual(actual, actual2);
    }



    #endregion

    #region Conversions =====================================
    [TestMethod, TestCategory("Conversions")]
    public void ExplicitConversion_ToDoubleArray_AsMeters()
    {
        var p = new Pudelko(1, 2.1, 3.231);
        double[] tab = (double[])p;
        Assert.AreEqual(3, tab.Length);
        Assert.AreEqual(p.A, tab[0]);
        Assert.AreEqual(p.B, tab[1]);
        Assert.AreEqual(p.C, tab[2]);
    }

    [TestMethod, TestCategory("Conversions")]
    public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
    {
        var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
        Pudelko p = (a, b, c);
        Assert.AreEqual((int)(p.A * 1000), a);
        Assert.AreEqual((int)(p.B * 1000), b);
        Assert.AreEqual((int)(p.C * 1000), c);
    }



    #endregion

    #region Parsing =========================================

    [TestMethod, TestCategory("Parsing")]
    public void Parsing_FromStringToBox_ImplicitMeters()
    {
        string s = new string("2.500 m × 9.321 m × 0.100 m");

        var p = new Pudelko(2.500, 9.321, 0.100);
        var p2 = Pudelko.Parse(s);

        Assert.AreEqual(p.A, p2.A);
        Assert.AreEqual(p.B, p2.B);
        Assert.AreEqual(p.C, p2.C);
    }

    [TestMethod, TestCategory("Parsing")]
    public void Parsing_FromStringToBox_ExplicitMeters()
    {
        string s = new string("2.500 m × 9.321 m × 0.100 m");

        var p = new Pudelko(2.500, 9.321, 0.100, UnitOfMeasure.meter);
        var p2 = Pudelko.Parse(s);

        Assert.AreEqual(p.A, p2.A);
        Assert.AreEqual(p.B, p2.B);
        Assert.AreEqual(p.C, p2.C);
    }

    [TestMethod, TestCategory("Parsing")]
    public void Parsing_FromStringToBox_Centimeters()
    {
        string s = new string("2.500 cm × 9.321 cm × 0.100 cm");

        var p = new Pudelko(2.500, 9.321, 0.100, UnitOfMeasure.centimeter);
        var p2 = Pudelko.Parse(s);

        Assert.AreEqual(p.A, p2.A);
        Assert.AreEqual(p.B, p2.B);
        Assert.AreEqual(p.C, p2.C);
    }

    [TestMethod, TestCategory("Parsing")]
    public void Parsing_FromStringToBox_Milimeter()
    {
        string s = new string("2500 mm × 9321 mm × 100 mm");

        var p = new Pudelko(2500, 9321, 100, UnitOfMeasure.milimeter);
        var p2 = Pudelko.Parse(s);

        Assert.AreEqual(p.A, p2.A);
        Assert.AreEqual(p.B, p2.B);
        Assert.AreEqual(p.C, p2.C);
    }

    [TestMethod, TestCategory("Parsing")]
    [ExpectedException(typeof(ArgumentException))]
    public void Parsing_FromStringToBoxEmptyString_Exception()
    {
        string s = new string(" ");

        var p = Pudelko.Parse(s);
    }

    [TestMethod, TestCategory("Parsing")]
    [ExpectedException(typeof(FormatException))]
    public void Parsing_FromStringToBoxWrongFormat_Exception()
    {
        string s = new string("2500 mm × 9321 mm");

        var p = Pudelko.Parse(s);
    }

    [TestMethod, TestCategory("Parsing")]
    [ExpectedException(typeof(FormatException))]
    public void Parsing_FromStringToBoxWrongFormat2_Exception()
    {
        string s = new string("2500 mm × 9321 mm × 100mm");

        var p = Pudelko.Parse(s);
    }

    [TestMethod, TestCategory("Parsing")]
    [ExpectedException(typeof(FormatException))]
    public void Parsing_FromStringToBoxWrongFormat3_Exception()
    {
        string s = new string("2500 mm × 9ll1 mm × 100mm");

        var p = Pudelko.Parse(s);
    }

    [TestMethod, TestCategory("Parsing")]
    [ExpectedException(typeof(FormatException))]
    public void Parsing_FromStringToBoxWrongFormat4_Exception()
    {
        string s = new string("2500 dm × 9001 dm × 100 dm");

        var p = Pudelko.Parse(s);
    }

    #endregion
}