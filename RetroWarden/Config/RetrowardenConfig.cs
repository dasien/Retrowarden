/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * RetrowardenConfig.cs
 *
 * Configuration management class that represents specific configuration
 * settings for the Retrowarden application. 
 *
 * Configuration settings include:
 * - CLI location (path)
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
namespace Retrowarden.Config
{
    public sealed class RetrowardenConfig
    {
        public RetrowardenConfig(string? cliLocation = "")
        {
            CLILocation = cliLocation;
        }

        public string? CLILocation { get; set; }
    }
}