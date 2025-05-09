using System.Text.Json;
using Infrastructure.Config.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Config;
public class JsonConfigProvider : IConfigProvider
{
    private readonly ILogger<JsonConfigProvider> logger;
    private readonly string configFolder;

    public JsonConfigProvider(ILogger<JsonConfigProvider> logger)
    {
        this.logger = logger;
        
        configFolder = Path.Combine(Directory.GetCurrentDirectory(), "Config");
    }

    public T LoadConfig<T>(string fileName)
    {
        var path = Path.Combine(configFolder, fileName);
        logger.LogInformation("Loading config from {Path}", path);
        
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
