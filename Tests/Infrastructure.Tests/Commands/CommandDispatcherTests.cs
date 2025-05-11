using Core.Commands;
using Infrastructure.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tests.Commands;

[TestFixture]
public class CommandDispatcherTests
{
    private record FakeCommand(int Value) : ICommand;

    private class FakeHandler : ICommandHandler<FakeCommand>
    {
        public int HandleCount { get; private set; }
        public FakeCommand LastCommand { get; private set; } = null!;

        public void Handle(FakeCommand command)
        {
            HandleCount++;
            LastCommand = command;
        }
    }

    private record UnhandledCommand() : ICommand;

    [Test]
    public void Dispatch_WithRegisteredHandler_InvokesHandle()
    {
        // Arrange
        var services = new ServiceCollection();
        var handler = new FakeHandler();
        services.AddSingleton<ICommandHandler<FakeCommand>>(handler);
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var cmd = new FakeCommand(42);

        // Act
        dispatcher.Dispatch(cmd);

        // Assert
        Assert.That(handler.HandleCount, Is.EqualTo(1), "Handle should be called exactly once");
        Assert.That(handler.LastCommand, Is.SameAs(cmd), "The same command instance should be passed through");
    }

    [Test]
    public void Dispatch_WithoutHandler_DoesNotThrow_LogsError()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var sw = new StringWriter();
        Console.SetOut(sw);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            dispatcher.Dispatch(new UnhandledCommand()),
            "Dispatch should catch and swallow exceptions when no handler is registered"
        );

        var output = sw.ToString();
        StringAssert.Contains("Error dispatching command", output,
            "Expected an error message to be written to the console");
    }
}