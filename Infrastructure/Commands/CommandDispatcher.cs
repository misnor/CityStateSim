using CityStateSim.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CityStateSim.Infrastructure.Commands;
public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider services;
    private readonly ILogger<CommandDispatcher> logger;

    private readonly List<string> suppressedCommands = new List<string>
    {
        "MoveCameraCommand"
    };

    public CommandDispatcher(IServiceProvider services, ILogger<CommandDispatcher> logger)
    {
        this.services = services;
        this.logger = logger;
    }

    public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
    {
        string commandText = typeof(TCommand).Name;
        if(!suppressedCommands.Contains(commandText))
        {
            this.logger.LogInformation($"Dispatching command of type {typeof(TCommand).Name}");
        }

        try
        {
            var handler = services.GetRequiredService<ICommandHandler<TCommand>>();
            handler.Handle(command);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error dispatching command: {e.Message}");
        }
    }
}
