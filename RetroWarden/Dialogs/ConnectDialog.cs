/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ConnectDialog.cs
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
    public sealed class ConnectDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtUserId;
        private TextField? _txtPassword;

        // Other values.
        private string _userId;
        private string _password;
        
        public ConnectDialog() 
        {
            _userId = "";
            _password = ""; 
            _okPressed = false;
            
            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Check to see if required values are present.
            if (_txtPassword != null && _txtUserId != null 
               && (_txtUserId.Text.Trim().Length == 0 || _txtPassword.Text.Trim().Length == 0))
            {
                MessageBox.ErrorQuery("Values Missing", "Both User Id and Password are required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtUserId != null)
                {
                    _userId = _txtUserId.Text ?? string.Empty;
                }

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
                X = 8, Y =6, Text = "_Connect", IsDefault = true
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 24, Y = 6, Title = "Cance_l"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Connect to Vault", Width = 40, Height =10
            };

            // Create labels.
            Label lblUserId = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true,
                Text = "*User Id:"
            };

            Label lblPassword = new Label()
            {
                X = 3, Y = 4, Width = 10, Height = 1, CanFocus = false, Visible = true,
                Text = "*Password:"
            };
            
            // Create text inputs.
            _txtUserId = new TextField()
            {
                X = 15, Y = 2, Height = 1, Width = 20, CanFocus = true, Visible = true
            };

            _txtPassword = new TextField()
            {
                X = 15, Y = 4, Width = 20, Height = 1, CanFocus = true, Visible = true, Secret = true
            };
            
            // Add controls to view.
            _dialog.Add(lblUserId, lblPassword, _txtUserId, _txtPassword, okButton, cancelButton);

            // Set default control.
            _txtUserId.SetFocus();
        }
        
        public string UserId
        {
            get { return _userId; }
        }

        public string Password
        {
            get { return _password; }
        }
    }
}