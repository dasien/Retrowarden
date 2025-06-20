/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * Program.cs
 *
 * Copyright (C) 2024 Brian Gentry
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
