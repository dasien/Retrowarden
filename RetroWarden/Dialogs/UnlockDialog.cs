/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GeneratePassphraseDialog.cs
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
    public sealed class UnlockDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtPassword;

        // Other values.
        private string _password;
        
        public UnlockDialog() 
        {
            _password = ""; 
            _okPressed = false;

            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Check to see if required values are present.
            if (_txtPassword != null &&  _txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.ErrorQuery("Value Missing", "Password is required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtPassword != null)
                {
                    _password = _txtPassword.Text ?? string.Empty;
                }

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 8, Y = 4, Text = "_Unlock", IsDefault = true
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 24, Y = 4, Text = "Cance_l"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Unlock Vault", Width = 40, Height = 8
            };

            // Create labels.
            Label lblPassword = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true, Text = "*Password:"
            };
            
            // Create text inputs.
            _txtPassword = new TextField()
            {
                X = 15, Y = 2, Width = 20, Height = 1, CanFocus = true, Visible = true, Secret = true
            };

            // Add controls to view.
            _dialog.Add(lblPassword, _txtPassword, okButton, cancelButton);

            // Set default control.
            _txtPassword.SetFocus();
        }
        
        public string Password
        {
            get { return _password; }
        }
    }
}