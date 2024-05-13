using System;

namespace Toolkits.Core.Tests;

[TestClass]
public class TypeStructurerTests
{
    [Test]
    public void GetPropertyAttributes_ReturnsCorrectAttributes()
    {
        // Arrange
        var expectedAttribute = new CustomAttribute();
        var propertyInfo = typeof(TestClass).GetProperty("TestProperty");

        // Act
        var attributes = TypeStructurer<TestClass>.GetPropertyAttributes<CustomAttribute>();

        // Assert
        Assert.AreEqual(expectedAttribute, attributes[propertyInfo]);
    }

    [Test]
    public void GetFieldAttributes_ReturnsCorrectAttributes()
    {
        // Arrange
        var expectedAttribute = new CustomAttribute();
        var fieldInfo = typeof(TestClass).GetField("TestField");

        // Act
        var attributes = TypeStructurer<TestClass>.GetFieldAttributes<CustomAttribute>();

        // Assert
        Assert.AreEqual(expectedAttribute, attributes[fieldInfo]);
    }

    private class TestClass
    {
        [Custom]
        public string TestProperty { get; set; }

        [Custom]
        public string TestField;
    }

    private class CustomAttribute : Attribute { }
}
