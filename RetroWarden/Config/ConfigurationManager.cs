using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Retrowarden.Config
{
    public sealed class ConfigurationManager
    {
        private static RetrowardenConfig? _config;

        static ConfigurationManager()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.AddJsonFile("appsettings.json");
            
            // Read the configuration from file.
            IConfigurationRoot root = builder.Build();
            
            // Get the config object.
            _config = root.Get<RetrowardenConfig>();

        }
        
        public static void WriteConfig(RetrowardenConfig config)
        {
            // Create new options bag for serialization.
            JsonSerializerOptions jsonWriteOptions = new JsonSerializerOptions();
            
            // Set write options.
            jsonWriteOptions.WriteIndented = true;
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());
            
            // Get new config json.
            string newJson = JsonSerializer.Serialize(config, jsonWriteOptions);
            
            // Set file location.
            string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            
            // Write the new appsettings file.
            File.WriteAllText(appSettingsPath, newJson);
        }
        
        public static RetrowardenConfig? GetConfig()
        {
            return _config;
        }
    }
}

