using System;
using System.Threading.Tasks;

namespace Toolkits.Core.Tests.Delegates;

[TestClass]
public class DelegateRepeaterTests
{
    private DelegateRepeater _delegateRepeater;

    [TestInitialize]
    public void Setup()
    {
        _delegateRepeater = new DelegateRepeater();
    }

    [TestCleanup]
    public void TearDown()
    {
        _delegateRepeater = null;
    }

    [Test]
    public void Unregister_WithValidToken_RemovesDelegate()
    {
        // Arrange
        string token = "testToken";
        bool delegateCalled = false;
        Action subscribeDelegate = () => delegateCalled = true;
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act
        _delegateRepeater.Unregister(token);

        // Assert
        Assert.IsFalse(delegateCalled);
    }

    [Test]
    public void Unregister_WithInvalidToken_ThrowsArgumentNullException()
    {
        // Arrange
        string token = null;

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => _delegateRepeater.Unregister(token));
    }

    [Test]
    public void Publish_WithValidToken_CallsDelegate()
    {
        // Arrange
        string token = "testToken";
        bool delegateCalled = false;
        Action subscribeDelegate = () => delegateCalled = true;
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act
        _delegateRepeater.Publish(token);

        // Assert
        Assert.IsTrue(delegateCalled);
    }

    [Test]
    public void Publish_WithInvalidToken_ThrowsArgumentException()
    {
        // Arrange
        string token = "invalidToken";

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _delegateRepeater.Publish(token));
    }

    [Test]
    public void Publish_WithMismatchReturnType_ThrowsArgumentException()
    {
        // Arrange
        string token = "testToken";
        Func<int> subscribeDelegate = () => 10;
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _delegateRepeater.Publish(token));
    }

    [Test]
    public void Publish_WithInconsistentParameters_ThrowsArgumentException()
    {
        // Arrange
        string token = "testToken";
        Action<int> subscribeDelegate = (int value) => { };
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _delegateRepeater.Publish(token));
    }

    [Test]
    public async Task PublishAsync_WithValidToken_CallsDelegate()
    {
        // Arrange
        string token = "testToken";
        bool delegateCalled = false;
        Func<Task> subscribeDelegate = async () =>
        {
            await Task.Delay(100);
            delegateCalled = true;
        };
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act
        await _delegateRepeater.PublishAsync(token);

        // Assert
        Assert.IsTrue(delegateCalled);
    }

    [Test]
    public async Task PublishAsync_WithInvalidToken_ThrowsArgumentException()
    {
        // Arrange
        string token = "invalidToken";

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _delegateRepeater.PublishAsync(token));
    }

    [Test]
    public async Task PublishAsync_WithMismatchReturnType_ThrowsArgumentException()
    {
        // Arrange
        string token = "testToken";
        Func<Task<int>> subscribeDelegate = async () =>
        {
            await Task.Delay(100);
            return 10;
        };
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _delegateRepeater.PublishAsync(token));
    }

    [Test]
    public async Task PublishAsync_WithInconsistentParameters_ThrowsArgumentException()
    {
        // Arrange
        string token = "testToken";
        Func<int, Task> subscribeDelegate = async (int value) =>
        {
            await Task.Delay(100);
        };
        _delegateRepeater.Subscribe(token, subscribeDelegate);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _delegateRepeater.PublishAsync(token));
    }

    [Test]
    public void Subscribe_WithInvalidToken_ThrowsArgumentNullException()
    {
        // Arrange
        string token = null;
        Action subscribeDelegate = () => { };

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => _delegateRepeater.Subscribe(token, subscribeDelegate));
    }

    [Test]
    public void Subscribe_WithNullDelegate_ThrowsArgumentNullException()
    {
        // Arrange
        string token = "testToken";
        Action subscribeDelegate = null;

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => _delegateRepeater.Subscribe(token, subscribeDelegate));
    }
}
