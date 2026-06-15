/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SplashDialog.cs
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
    public sealed class SplashDialog : BaseDialog
    {
        // Controls.
        private Label? _message;
        private readonly string? _messageText;
        private object? _timerToken;

        // The running application. A not-yet-run top-level view's App is null, so the caller
        // (which is already running) must supply it; we can't read it off _dialog before Run.
        private IApplication? _app;

        public SplashDialog(string? message)
        {
            // Initialize members.
            _messageText = message;

            InitializeComponent();
        }

        public void Show(IApplication? app)
        {
            if (app == null || _dialog == null)
            {
                return;
            }

            // Stash the app so the timer callback can stop the modal session.
            _app = app;

            // Schedule the auto-hide before running the modal splash.
            _timerToken = app.AddTimeout(TimeSpan.FromMilliseconds(3000), Hide);

            // Apply consistent color scheme before showing
            ColorSchemeManager.ApplyColorScheme(_dialog);
            app.Run(_dialog);
        }

        private bool Hide()
        {
            if (_timerToken != null)
            {
                _app?.RemoveTimeout(_timerToken);
            }

            _app?.RequestStop(_dialog);

            return false;
        }

        private void InitializeComponent()
        {
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "", Width = 85, Height = 12
            };
            
            // Create labels.
            _message = new Label()
            {
                X = 0, Y = 0, Width = 85, Height = 9, CanFocus = false, Visible = true,
                Text = _messageText == null ? "" : _messageText, Data = "lblMessage"
            };
            
            // Add controls to view.
            _dialog.Add(_message);
        }
    }
}