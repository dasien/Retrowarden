/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ConfirmationDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    /// <summary>
    /// Custom confirmation dialog that provides consistent styling and behavior
    /// for user confirmation prompts throughout the Retrowarden application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This dialog extends BaseDialog to provide:
    /// </para>
    /// <list type="bullet">
    /// <item>Consistent color scheme application matching the application theme</item>
    /// <item>Customizable title, message, and button text</item>
    /// <item>Standard OK/Cancel button behavior with proper focus handling</item>
    /// <item>Keyboard navigation support (Enter for OK, Escape for Cancel)</item>
    /// </list>
    /// <para>
    /// The dialog returns true via OkPressed property if the user confirms the action,
    /// or false if they cancel. This replaces Terminal.GUI's MessageBox.Query() for
    /// better integration with the application's visual theme.
    /// </para>
    /// </remarks>
    public sealed class ConfirmationDialog : BaseDialog
    {
        private readonly string _title;
        private readonly string _message;
        private readonly string _okText;
        private readonly string _cancelText;

        /// <summary>
        /// Initializes a new instance of the ConfirmationDialog with customizable text.
        /// </summary>
        /// <param name="title">The dialog title text.</param>
        /// <param name="message">The confirmation message to display.</param>
        /// <param name="okText">Text for the OK/confirm button (default: "Ok").</param>
        /// <param name="cancelText">Text for the Cancel button (default: "Cancel").</param>
        public ConfirmationDialog(string title, string message, string okText = "Ok", string cancelText = "Cancel")
        {
            _title = title;
            _message = message;
            _okText = okText;
            _cancelText = cancelText;
            _okPressed = false;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Calculate dialog dimensions based on message length
            int messageWidth = Math.Max(_message.Length + 4, Math.Max(_title.Length + 4, 30));
            int dialogWidth = Math.Min(messageWidth, 60);
            int dialogHeight = 8;

            // Create the main dialog
            _dialog = new Dialog()
            {
                Title = _title,
                Width = dialogWidth,
                Height = dialogHeight,
                Modal = true,
                BorderStyle = LineStyle.Single
            };

            // Create message label
            var messageLabel = new Label()
            {
                Text = _message,
                X = Pos.Center(),
                Y = 1,
                Width = Dim.Fill(2),
                Height = 2,
                TextAlignment = Alignment.Center
            };

            // Create OK button
            var okButton = new Button()
            {
                Text = _okText,
                X = Pos.Center() - 8,
                Y = Pos.AnchorEnd(2),
                Width = 8,
                IsDefault = true
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button
            var cancelButton = new Button()
            {
                Text = _cancelText,
                X = Pos.Center() + 2,
                Y = Pos.AnchorEnd(2),
                Width = 8
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Add controls to dialog
            _dialog.Add(messageLabel);
            _dialog.Add(okButton);
            _dialog.Add(cancelButton);

            // Set focus to OK button by default
            okButton.SetFocus();
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            _okPressed = true;
            if (_dialog != null)
            {
                Application.RequestStop(_dialog);
            }
        }
    }
}