using Gameplay.DependencyInjection;
using Infrastructure.Application;
using Infrastructure.DependencyInjection;
using Infrastructure.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI;
using UI.Input;
using UI.Rendering.Interfaces;
using UI.Rendering;
using UI.Services;

internal class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddScoped<MainGame>()
            .AddSingleton<IInputService, MonoGameInputService>()
            .AddScoped<IGameControl, GameControlAdapter>(sp => new GameControlAdapter(sp.GetRequiredService<MainGame>()))
            .AddSingleton<IRenderService, RenderService>()
            .AddInfrastructure()
            .AddGameplaySimulation()
            .AddWorldFactory()
            .AddEventBuses()
            .AddLogging(lb =>
            {
                lb.ClearProviders();
                lb.AddSimpleConsole(opts =>
                {
                    opts.SingleLine = true;
                    opts.TimestampFormat = "HH:mm:ss.fff - ";
                    opts.IncludeScopes = false;
                });
                lb.SetMinimumLevel(LogLevel.Information);
            });

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