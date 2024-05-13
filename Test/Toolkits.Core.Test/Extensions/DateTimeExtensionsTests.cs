using System;

namespace Toolkits.Core.Tests.Extensions;

[TestClass]
public class DateTimeExtensionsTests
{
    [Test]
    public void GetMinuteBegin_ShouldReturnMinuteBegin()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 12, 30, 0);

        // Act
        DateTime result = dateTime.GetMinuteBegin();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetMinuteEnd_ShouldReturnMinuteEnd()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 12, 31, 59);

        // Act
        DateTime result = dateTime.GetMinuteEnd();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetHourBegin_ShouldReturnHourBegin()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 12, 0, 0);

        // Act
        DateTime result = dateTime.GetHourBegin();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetHourEnd_ShouldReturnHourEnd()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 13, 0, 59);

        // Act
        DateTime result = dateTime.GetHourEnd();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetDayBegin_ShouldReturnDayBegin()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 0, 0, 0);

        // Act
        DateTime result = dateTime.GetDayBegin();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetDayEnd_ShouldReturnDayEnd()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 1, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 2, 0, 0, 0).AddMilliseconds(-1);

        // Act
        DateTime result = dateTime.GetDayEnd();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetMonthBegin_ShouldReturnMonthBegin()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 15, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 0, 0, 0);

        // Act
        DateTime result = dateTime.GetMonthBegin();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetMonthEnd_ShouldReturnMonthEnd()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 1, 15, 12, 30, 45);
        DateTime expected = new DateTime(2022, 2, 1, 0, 0, 0).AddMilliseconds(-1);

        // Act
        DateTime result = dateTime.GetMonthEnd();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetYearBegin_ShouldReturnYearBegin()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 6, 15, 12, 30, 45);
        DateTime expected = new DateTime(2022, 1, 1, 0, 0, 0);

        // Act
        DateTime result = dateTime.GetYearBegin();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetYearEnd_ShouldReturnYearEnd()
    {
        // Arrange
        DateTime dateTime = new DateTime(2022, 6, 15, 12, 30, 45);
        DateTime expected = new DateTime(2023, 1, 1, 0, 0, 0).AddMilliseconds(-1);

        // Act
        DateTime result = dateTime.GetYearEnd();

        // Assert
        Assert.AreEqual(expected, result);
    }
}
