using System;

namespace Toolkits.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void IsNullOrWhiteSpace_Should_Return_True_When_String_Is_Null()
    {
        string value = null;

        bool result = value.IsNullOrWhiteSpace();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_Should_Return_True_When_String_Is_Empty()
    {
        string value = "";

        bool result = value.IsNullOrWhiteSpace();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_Should_Return_True_When_String_Is_Whitespace()
    {
        string value = "   ";

        bool result = value.IsNullOrWhiteSpace();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_Should_Return_False_When_String_Is_Not_Null_Or_Whitespace()
    {
        string value = "Hello, World!";

        bool result = value.IsNullOrWhiteSpace();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void WhenIsNullOrWhiteSpaceUse_Should_Return_DefaultValue_When_String_Is_Null_Or_Whitespace()
    {
        string value = null;
        string defaultValue = "Default";

        string result = value.WhenIsNullOrWhiteSpaceUse(defaultValue);

        Assert.AreEqual(defaultValue, result);
    }

    [TestMethod]
    public void WhenIsNullOrWhiteSpaceUse_Should_Return_String_When_String_Is_Not_Null_Or_Whitespace()
    {
        string value = "Hello, World!";
        string defaultValue = "Default";

        string result = value.WhenIsNullOrWhiteSpaceUse(defaultValue);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void WhenIsNullOrWhiteSpaceUse_Should_Throw_Exception_When_String_Is_Null_Or_Whitespace()
    {
        string value = null;
        Exception exception = new Exception("String is null or whitespace");

        Assert.ThrowsException<Exception>(() => value.WhenIsNullOrWhiteSpaceUse(exception));
    }

    [TestMethod]
    public void WhenIsNullOrWhiteSpaceUse_Should_Return_String_When_String_Is_Not_Null_Or_Whitespace_1()
    {
        string value = "Hello, World!";
        Exception exception = new Exception("String is null or whitespace");

        string result = value.WhenIsNullOrWhiteSpaceUse(exception);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpace_Should_Return_False_When_String_Is_Null()
    {
        string value = null;

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpace_Should_Return_False_When_String_Is_Empty()
    {
        string value = "";

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpace_Should_Return_False_When_String_Is_Whitespace()
    {
        string value = "   ";

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpace_Should_Return_True_When_String_Is_Not_Null_Or_Whitespace()
    {
        string value = "Hello, World!";

        bool result = value.IsNotNullOrWhiteSpace();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void WhenIsNullOrEmptyUse_Should_Return_DefaultValue_When_String_Is_Null_Or_Empty()
    {
        string value = null;
        string defaultValue = "Default";

        string result = value.WhenIsNullOrEmptyUse(defaultValue);

        Assert.AreEqual(defaultValue, result);
    }

    [TestMethod]
    public void WhenIsNullOrEmptyUse_Should_Return_String_When_String_Is_Not_Null_Or_Empty()
    {
        string value = "Hello, World!";
        string defaultValue = "Default";

        string result = value.WhenIsNullOrEmptyUse(defaultValue);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void WhenIsNullOrEmptyUse_Should_Throw_Exception_When_String_Is_Null_Or_Empty()
    {
        string value = null;
        Exception exception = new Exception("String is null or empty");

        Assert.ThrowsException<Exception>(() => value.WhenIsNullOrEmptyUse(exception));
    }

    [TestMethod]
    public void WhenIsNullOrEmptyUse_Should_Return_String_When_String_Is_Not_Null_Or_Empty_1()
    {
        string value = "Hello, World!";
        Exception exception = new Exception("String is null or empty");

        string result = value.WhenIsNullOrEmptyUse(exception);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void IsNullOrEmpty_Should_Return_True_When_String_Is_Null()
    {
        string value = null;

        bool result = value.IsNullOrEmpty();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrEmpty_Should_Return_True_When_String_Is_Empty()
    {
        string value = "";

        bool result = value.IsNullOrEmpty();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrEmpty_Should_Return_False_When_String_Is_Not_Null_Or_Empty()
    {
        string value = "Hello, World!";

        bool result = value.IsNullOrEmpty();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrEmpty_Should_Return_False_When_String_Is_Null()
    {
        string value = null;

        bool result = value.IsNotNullOrEmpty();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrEmpty_Should_Return_False_When_String_Is_Empty()
    {
        string value = "";

        bool result = value.IsNotNullOrEmpty();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNotNullOrEmpty_Should_Return_True_When_String_Is_Not_Null_Or_Empty()
    {
        string value = "Hello, World!";

        bool result = value.IsNotNullOrEmpty();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Join_Should_Return_Joined_String_With_Default_Interval_Symbol()
    {
        var source = new[] { "Hello", "World" };

        string result = source.Join();

        Assert.AreEqual("Hello,World", result);
    }

    [TestMethod]
    public void Join_Should_Return_Joined_String_With_Custom_Interval_Symbol()
    {
        var source = new[] { "Hello", "World" };
        string intervalSymbol = "-";

        string result = source.Join(intervalSymbol);

        Assert.AreEqual("Hello-World", result);
    }

    [TestMethod]
    public void Join_With_Selector_Should_Return_Joined_String_With_Default_Interval_Symbol()
    {
        var source = new[] { 1, 2, 3 };

        string result = source.Join(x => x.ToString());

        Assert.AreEqual("1,2,3", result);
    }

    [TestMethod]
    public void Join_With_Selector_Should_Return_Joined_String_With_Custom_Interval_Symbol()
    {
        var source = new[] { 1, 2, 3 };
        string intervalSymbol = "-";

        string result = source.Join(x => x.ToString(), intervalSymbol);

        Assert.AreEqual("1-2-3", result);
    }
}
