
using CityStateSim.Infrastructure.Config.Interfaces;
using CityStateSim.Infrastructure.Config;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace CityStateSim.Infrastructure.Tests.Config;
internal class JsonConfigProviderTest
{
    private string originalDir;
    private string tempDir;
    private IConfigProvider provider;

    [SetUp]
    public void SetUp()
    {
        // Capture and replace the working directory
        originalDir = Directory.GetCurrentDirectory();
        tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(tempDir, "Config"));
        Directory.SetCurrentDirectory(tempDir);

        // Use a no-op logger
        var logger = NullLogger<JsonConfigProvider>.Instance;
        provider = new JsonConfigProvider(logger);
    }

    [TearDown]
    public void TearDown()
    {
        // Restore original directory and clean up
        Directory.SetCurrentDirectory(originalDir);
        if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, recursive: true);
    }

    private class TestConfig
    {
        public int Foo { get; set; }
        public string Bar { get; set; } = string.Empty;
    }

    [Test]
    public void LoadConfig_ValidJson_ReturnsDeserializedObject()
    {
        // Arrange: write a valid JSON config
        var configJson = "{\"Foo\":123, \"Bar\":\"baz\"}";
        File.WriteAllText(Path.Combine("Config", "test.json"), configJson);

        // Act
        var result = provider.LoadConfig<TestConfig>("test.json");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Foo, Is.EqualTo(123));
        Assert.That(result.Bar, Is.EqualTo("baz"));
    }

    [Test]
    public void LoadConfig_MissingFile_ThrowsFileNotFoundException()
    {
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
            provider.LoadConfig<object>("nofile.json"));
    }

    [Test]
    public void LoadConfig_InvalidJson_ThrowsJsonException()
    {
        // Arrange: write malformed JSON
        File.WriteAllText(Path.Combine("Config", "bad.json"), "{ not: valid ]");

        // Act & Assert
        Assert.Throws<JsonException>(() =>
            provider.LoadConfig<object>("bad.json"));
    }
}
