/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SelectSendTypeDialog.cs
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
    public sealed class SelectSendTypeDialog : BaseDialog
    {
        private RadioGroup? _rdoSendType;
        private int _sendType = 0;

        public SelectSendTypeDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;

            if (_rdoSendType != null)
            {
                _sendType = _rdoSendType.SelectedItem;
            }

            // Close dialog.
            Application.RequestStop(_dialog);
        }

        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 5, Y = 4, Text = "Ok", IsDefault = true
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 14, Y = 4, Text = "Cancel"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            string[] types = { "File", "Text" };

            // Create radio group.
            _rdoSendType = new RadioGroup()
            {
                X = 8, Y = 1, RadioLabels = types
            };

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Send Type", Width = 30, Height = 7
            };
            _dialog.Add(_rdoSendType, okButton, cancelButton);
        }

        /// <summary>
        /// Gets the selected Send type. 0 for File, 1 for Text.
        /// </summary>
        public int SendType => _sendType;
    }
}