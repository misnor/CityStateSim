using Gameplay.DependencyInjection;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UI;

internal class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddLogging(config => config.AddConsole())
            .AddWorldFactory()
            .AddConfiguration()
            .AddEventBuses()
            .AddGameplaySimulation()
            .AddScoped<MainGame>();

        var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });

        using var scope = provider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Starting CityStateSim…");

        using var game = scope.ServiceProvider.GetRequiredService<MainGame>();
        game.Run();
    }
}