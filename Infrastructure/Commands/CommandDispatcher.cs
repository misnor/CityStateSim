using CityStateSim.Core.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CityStateSim.Infrastructure.Commands;
public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider services;

    public CommandDispatcher(IServiceProvider services)
    {
        this.services = services;
    }

    public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
    {
        Console.WriteLine($"Dispatching command of type {typeof(TCommand).Name}");
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
