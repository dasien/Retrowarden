/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ConfigurationManager.cs
 *
 * Core configuration management class responsible for loading, saving,
 * and maintaining application settings. Handles serialization and
 * persistence of configuration data across application sessions.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
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

