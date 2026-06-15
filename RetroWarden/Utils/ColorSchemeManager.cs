/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ColorSchemeManager.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Utils
{
    /// <summary>
    /// Centralized color scheme management for consistent UI appearance across
    /// all views and dialogs in the Retrowarden application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This static class provides a unified color scheme that can be applied to:
    /// </para>
    /// <list type="bullet">
    /// <item>Main application views (MainView)</item>
    /// <item>Modal dialogs (BaseDialog derivatives)</item>
    /// <item>Item detail views (ItemDetailView derivatives)</item>
    /// <item>Working dialogs and other UI components</item>
    /// </list>
    /// <para>
    /// The color scheme uses Terminal.GUI v2 syntax with explicit Attribute constructors
    /// to ensure compatibility across different terminal emulators and environments.
    /// </para>
    /// </remarks>
    public static class ColorSchemeManager
    {
        /// <summary>
        /// Gets the application's standard color scheme for consistent UI appearance.
        /// </summary>
        /// <returns>A Scheme configured for optimal readability and Terminal.GUI compatibility.</returns>
        public static Scheme GetApplicationColorScheme()
        {
            return new Scheme(new Terminal.Gui.Drawing.Attribute(Color.White, Color.Black))
            {
                Normal = new Terminal.Gui.Drawing.Attribute(Color.White, Color.Black),
                Focus = new Terminal.Gui.Drawing.Attribute(Color.Black, Color.White),  // Better contrast for menu highlighting
                HotNormal = new Terminal.Gui.Drawing.Attribute(Color.Yellow, Color.Black),
                HotFocus = new Terminal.Gui.Drawing.Attribute(Color.Yellow, Color.White)
            };
        }

        /// <summary>
        /// Applies the application color scheme to any view or dialog.
        /// </summary>
        /// <param name="view">The view to apply the color scheme to.</param>
        public static void ApplyColorScheme(View view)
        {
            if (view != null)
            {
                view.SetScheme(GetApplicationColorScheme());
            }
        }

        /// <summary>
        /// Recursively applies the application color scheme to a view and all its child controls.
        /// This is useful for complex views with multiple nested controls.
        /// </summary>
        /// <param name="view">The parent view to apply color scheme to recursively.</param>
        public static void ApplyColorSchemeRecursively(View view)
        {
            if (view == null) return;
            
            // Apply to the view itself
            view.SetScheme(GetApplicationColorScheme());
            
            // Apply to all child views
            foreach (View child in view.SubViews)
            {
                ApplyColorSchemeRecursively(child);
            }
        }
    }
}