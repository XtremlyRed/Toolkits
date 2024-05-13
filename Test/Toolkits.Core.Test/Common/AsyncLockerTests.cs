using System;
using System.Threading;
using System.Threading.Tasks;

namespace Toolkits.Core.Tests;

[TestClass]
public class AsyncLockerTests
{
    [Test]
    public void Release_ShouldDecrementCounterAndReleaseSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);
        asyncLocker.Wait();

        // Act
        asyncLocker.Release();

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, true);
    }

    [Test]
    public void ReleaseAll_ShouldDecrementCounterToZeroAndReleaseAllSemaphores()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(2, 2);
        asyncLocker.Wait();
        asyncLocker.Wait();

        // Act
        asyncLocker.ReleaseAll();

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, true);
    }

    [Test]
    public void Wait_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);

        // Act
        asyncLocker.Wait();

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }

    [Test]
    public async Task WaitAsync_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);

        // Act
        await asyncLocker.WaitAsync();

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }

    [Test]
    public void Wait_WithTimeout_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);

        // Act
        bool result = asyncLocker.Wait(1000);

        // Assert
        Assert.AreEqual(result, true);
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }

    [Test]
    public async Task WaitAsync_WithTimeout_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);

        // Act
        bool result = await asyncLocker.WaitAsync(1000);

        // Assert
        Assert.AreEqual(result, true);
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }

    [Test]
    public void Wait_WithCancellationToken_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        asyncLocker.Wait(cancellationTokenSource.Token);

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }

    [Test]
    public async Task WaitAsync_WithCancellationToken_ShouldIncrementCounterAndAcquireSemaphore()
    {
        // Arrange
        var asyncLocker = new AsyncLocker(1, 1);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await asyncLocker.WaitAsync(cancellationTokenSource.Token);

        // Assert
        Assert.AreEqual(asyncLocker.IsIdle, false);
    }
}
