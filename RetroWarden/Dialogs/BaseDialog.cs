/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * BaseDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Retrowarden.Utils;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    /// <summary>
    /// Base class for all application dialogs providing consistent UI behavior,
    /// color scheme management, and common dialog functionality.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class serves as the foundation for all modal dialogs in the application:
    /// </para>
    /// <list type="bullet">
    /// <item>Consistent color scheme application for uniform appearance</item>
    /// <item>Standard OK/Cancel button handling and dialog lifecycle management</item>
    /// <item>Centralized dialog showing and closing functionality</item>
    /// <item>Common properties like OkPressed status tracking</item>
    /// </list>
    /// <para>
    /// All dialogs inherit from this base class to ensure consistent behavior and
    /// appearance throughout the application. The color scheme is automatically
    /// applied when the dialog is shown to match the main application UI.
    /// </para>
    /// </remarks>
    public abstract class BaseDialog
    {
        protected Dialog? _dialog;
        protected bool _okPressed;

        // The running application, supplied by the (already-running) caller via Show(app).
        // A not-yet-run top-level Dialog's App is null, so we can't read it off _dialog before Run.
        protected IApplication? _app;

        protected void CancelButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Set ok button flag.
            _okPressed = false;

            // Close dialog.
            _app?.RequestStop(_dialog);
        }

        public void Show(IApplication? app)
        {
            if (app == null || _dialog == null)
            {
                return;
            }

            // Stash the app so handlers can stop the modal session.
            _app = app;

            // Apply consistent color scheme before showing
            ColorSchemeManager.ApplyColorScheme(_dialog);
            app.Run(_dialog);
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }
    }
}

