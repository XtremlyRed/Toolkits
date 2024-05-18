namespace Toolkits.Core.Tests;

[TestClass]
public class AwaiterTests
{
    [TestMethod]
    public void Release_IncrementCounterAndReleaseSemaphore()
    {
        // Arrange
        var awaiter = new Awaiter(1);

        // Act
        awaiter.Release();

        // Assert
        Assert.AreEqual(2, awaiter.Counter);
    }

    [TestMethod]
    public void ReleaseAll_IncrementCounterAndReleaseSemaphoreUntilMaxCount()
    {
        // Arrange
        var awaiter = new Awaiter(1, 3);

        // Act
        awaiter.ReleaseAll();

        // Assert
        Assert.AreEqual(3, awaiter.Counter);
    }

    [TestMethod]
    public void Wait_DecrementCounterAndWaitOnSemaphore()
    {
        // Arrange
        var awaiter = new Awaiter(1);

        // Acttask
        Task.Run(() => awaiter.Wait());

        Thread.Sleep(300);

        // Assert
        Assert.AreEqual(0, awaiter.Counter);
    }

    [TestMethod]
    public void WaitAsync_DecrementCounterAndWaitAsyncOnSemaphore()
    {
        // Arrange
        var awaiter = new Awaiter(1);

        // Act
        Task.Run(async () => await awaiter.WaitAsync()).GetAwaiter();

        Thread.Sleep(300);
        // Assert
        Assert.AreEqual(0, awaiter.Counter);
    }

    [TestMethod]
    public void Dispose_ReleaseSemaphoreAndSetCounterToZero()
    {
        // Arrange
        var awaiter = new Awaiter(1);

        // Act
        awaiter.Dispose();

        // Assert
        Assert.AreEqual(0, awaiter.Counter);
    }
}
