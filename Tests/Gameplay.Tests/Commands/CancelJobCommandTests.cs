using CityStateSim.Gameplay.Commands;

namespace CityStateSim.Gameplay.Tests.Commands;

[TestFixture]
public class CancelJobCommandTests
{
    [Test]
    public void Constructor_ValidCoordinates_SetsProperties()
    {
        // Arrange & Act
        var command = new CancelJobCommand(1, 2, 3, 4);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(command.MinX, Is.EqualTo(1));
            Assert.That(command.MinY, Is.EqualTo(2));
            Assert.That(command.MaxX, Is.EqualTo(3));
            Assert.That(command.MaxY, Is.EqualTo(4));
        });
    }

    [Test]
    public void Constructor_MinGreaterThanMax_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
        {
            new CancelJobCommand(5, 2, 3, 4);
        });
        Assert.Throws<ArgumentException>(() => 
        {
            new CancelJobCommand(1, 6, 3, 4);
        });
    }

    [Test]
    public void Constructor_NegativeCoordinates_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => 
        {
            new CancelJobCommand(-1, -2, 3, 4);
        });
    }
} 