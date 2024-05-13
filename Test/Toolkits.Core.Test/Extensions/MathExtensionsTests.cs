using Toolkits.Core;

namespace Toolkits.Core.Tests;

[TestClass]
public class MathExtensionsTests
{
    [Test]
    public void FromRange_Long_ReturnsValueWithinRange()
    {
        long value = 10;
        long minValue = 0;
        long maxValue = 20;

        long result = value.FromRange(minValue, maxValue);

        Assert.AreEqual(value, result);
    }

    [Test]
    public void FromRange_Long_ReturnsMinValueWhenBelowRange()
    {
        long value = -10;
        long minValue = 0;
        long maxValue = 20;

        long result = value.FromRange(minValue, maxValue);

        Assert.AreEqual(minValue, result);
    }

    [Test]
    public void FromRange_Long_ReturnsMaxValueWhenAboveRange()
    {
        long value = 30;
        long minValue = 0;
        long maxValue = 20;

        long result = value.FromRange(minValue, maxValue);

        Assert.AreEqual(maxValue, result);
    }

    // Add similar tests for other data types and methods

    [Test]
    public void InRange_Long_ReturnsTrueWhenValueInRange()
    {
        long value = 10;
        long minValue = 0;
        long maxValue = 20;

        bool result = value.InRange(minValue, maxValue);

        Assert.IsTrue(result);
    }

    [Test]
    public void InRange_Long_ReturnsFalseWhenValueBelowRange()
    {
        long value = -10;
        long minValue = 0;
        long maxValue = 20;

        bool result = value.InRange(minValue, maxValue);

        Assert.IsFalse(result);
    }

    [Test]
    public void InRange_Long_ReturnsFalseWhenValueAboveRange()
    {
        long value = 30;
        long minValue = 0;
        long maxValue = 20;

        bool result = value.InRange(minValue, maxValue);

        Assert.IsFalse(result);
    }

    // Add similar tests for other data types and methods
}
