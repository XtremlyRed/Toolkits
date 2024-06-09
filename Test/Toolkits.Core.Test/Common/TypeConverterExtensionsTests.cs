using System;

namespace Toolkits.Core.Tests
{
    [TestClass]
    public class TypeConverterExtensionsTests
    {
        [Test]
        public void TryConvertTo_ValidConversion_ReturnsTrueAndConvertedValue()
        {
            // Arrange
            object from = 10;
            int expected = 10;

            // Act
            bool result = TypeConverterExtensions.TryConvertTo(from, out int outValue);

            // Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(expected, outValue);
        }

        [Test]
        public void TryConvertTo_InvalidConversion_ReturnsFalseAndDefault()
        {
            // Arrange
            object from = "invalid";
            string expected = default!;

            // Act
            bool result = TypeConverterExtensions.TryConvertTo(from, out string outValue);

            // Assert
            Assert.AreEqual(result, true);
            Assert.AreNotEqual(expected, outValue);
        }

        [Test]
        public void ConvertTo_ValidConversion_ReturnsConvertedValue()
        {
            // Arrange
            object from = "10";
            int expected = 10;

            // Act
            int result = TypeConverterExtensions.ConvertTo<int>(from);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConvertTo_InvalidConversion_ThrowsInvalidCastException()
        {
            // Arrange
            object from = null!;

            // Act & Assert
            Assert.ThrowsException<InvalidCastException>(() => TypeConverterExtensions.ConvertTo<int>(from));
        }

        [Test]
        public void ConvertTo_UnregisteredTypeConverter_ThrowsInvalidOperationException()
        {
            // Arrange
            object from = "10";

            // Act & Assert
            Assert.ThrowsException<FormatException>(() => TypeConverterExtensions.ConvertTo<DateTime>(from));
        }

        [Test]
        public void ConvertTo_UnregisteredTypeConversion_ThrowsInvalidOperationException()
        {
            // Arrange
            object from = 10;

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => TypeConverterExtensions.ConvertTo<string>(from));
        }

        [Test]
        public void CreateConverter_ValidConversion_CreatesConverter()
        {
            // Arrange
            Func<string, int> typeConverter = (s) => int.Parse(s);

            // Act
            var converterBuilder = TypeConverterExtensions.CreateConverter<string, int>(typeConverter);

            // Assert
            Assert.AreEqual(converterBuilder is not null, true);
        }

        [Test]
        public void CreateConverter_NullTypeConverter_ThrowsArgumentNullException()
        {
            // Arrange
            Func<string, int> typeConverter = null!;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => TypeConverterExtensions.CreateConverter<string, int>(typeConverter));
        }

        [Test]
        public void ReverseConverter_ValidConversion_ReversesConverter()
        {
            // Arrange
            Func<string, int> typeConverter = (s) => int.Parse(s);

            // Act
            var converterBuilder = TypeConverterExtensions.CreateConverter<string, int>(typeConverter);
            converterBuilder.ReverseConverter((i) => i.ToString());

            // Assert
            Assert.AreEqual(converterBuilder is not null, true);
        }

        [Test]
        public void ReverseConverter_NullTypeConverter_ThrowsArgumentNullException()
        {
            // Arrange
            Func<string, int> typeConverter = (s) => int.Parse(s);

            // Act
            var converterBuilder = TypeConverterExtensions.CreateConverter<string, int>(typeConverter);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => converterBuilder.ReverseConverter(null!));
        }
    }
}
