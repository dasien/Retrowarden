/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 *
 * Program.cs
 *
 * This is the main entry point for the Retrowarden application, handling
 * initialization, dependency injection setup, and application lifecycle.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;
using Retrowarden.Views;

namespace Retrowarden
{
    public class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            MainView mainView = new MainView(true);
            
            // Run the application loop.
            Application.Run(mainView);
            mainView.Dispose();
            Application.Shutdown();
        }
    }
}
