using System;

namespace Toolkits.Core.Tests;

[TestClass]
public class BufferSegmentsTests
{
    [Test]
    public void Read_ThrowsArgumentOutOfRangeException_WhenReadLengthIsGreaterThanCount()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>();
        bufferSegments.Write(new int[] { 1, 2, 3 }, 0, 3);

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => bufferSegments.Read(4));
    }

    [Test]
    public void Read_ReturnsCorrectBuffer_WhenReadLengthIsLessThanCount()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>();
        bufferSegments.Write(new int[] { 1, 2, 3 }, 0, 3);

        // Act
        var result = bufferSegments.Read(2);

        // Assert

        Assert.AreEqual(Enumerable.SequenceEqual(new int[] { 1, 2 }, result), true);
    }

    [Test]
    public void Write_ThrowsArgumentNullException_WhenBufferIsNull()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>();

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => bufferSegments.Write(null, 0, 3));
    }

    [Test]
    public void Write_WritesBufferToSegments_WhenWriteLengthIsLessThanSegmentCapacity()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>();

        // Act
        bufferSegments.Write(new int[] { 1, 2, 3 }, 0, 3);

        // Assert
        Assert.AreEqual(3, bufferSegments.Count);
    }

    [Test]
    public void Write_WritesBufferToMultipleSegments_WhenWriteLengthIsGreaterThanSegmentCapacity()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>(2);

        // Act
        bufferSegments.Write(new int[] { 1, 2, 3 }, 0, 3);

        // Assert
        Assert.AreEqual(3, bufferSegments.Count);
    }

    [Test]
    public async Task Write_WritesBufferToMultipleSegments_WhenWriteLengthIsGreaterThanSegmentCapacity2()
    {
        // Arrange
        var bufferSegments = new BufferSegments<int>();

        // Act
        //     bufferSegments.Write(new int[] { 1, 2, 3 }, 0, 3);
        var uffer = await bufferSegments.ReadAsync(3);

        // Assert
        Assert.AreEqual(3, uffer.Length);
    }
}
