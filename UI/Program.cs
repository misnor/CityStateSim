using CityStateSim.Gameplay.DependencyInjection;
using CityStateSim.Infrastructure.Application;
using CityStateSim.Infrastructure.DependencyInjection;
using CityStateSim.Infrastructure.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UI;
using CityStateSim.UI.Input;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Rendering;
using CityStateSim.UI.Services;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.UI.Factories;
using CityStateSim.UI.Camera;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Core.Commands;

internal class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddScoped<MainGame>()
            .AddSingleton<IInputService, MonoGameInputService>()
            .AddScoped<IGameControl, GameControlAdapter>(sp => new GameControlAdapter(sp.GetRequiredService<MainGame>()))
            .AddScoped<IRenderService, RenderService>()
            .AddSingleton<IFontFactory, FontFactory>()
            .AddSingleton<ITextureFactory, TextureFactory>()
            .AddSingleton<IRenderSystem, TileRenderSystem>()
            .AddSingleton<IRenderSystem, AgentRenderSystem>()
            .AddSingleton<IRenderSystem, HoverRenderSystem>()
            .AddScoped<IRenderSystem, ToolbarRenderSystem>()
            .AddScoped<IRenderSystem, RectangleDrawSystem>()
            .AddSingleton<Camera2D>()
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