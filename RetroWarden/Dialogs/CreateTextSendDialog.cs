/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CreateTextSendDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/

using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class CreateTextSendDialog : BaseSendDialog
    {
        // Controls
        private TextField? _txtName;
        private TextView? _txtNotes;
        private TextView? _txtText;
        private DateField? _datDeletionDate;
        private DateField? _datExpirationDate;
        private TextField? _txtMaxAccessCount;
        private CheckBox? _chkPassword;
        private TextField? _txtPassword;
        private CheckBox? _chkDisableSave;
        private CheckBox? _chkHideEmail;

        public CreateTextSendDialog() : base("2")
        {
            _okPressed = false;

            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Validate required fields
            if (_txtName == null || string.IsNullOrWhiteSpace(_txtName.Text.ToString()))
            {
                MessageBox.ErrorQuery("Missing Information", "Send name is required.", "OK");
                return;
            }

            if (_txtText == null || string.IsNullOrWhiteSpace(_txtText.Text.ToString()))
            {
                MessageBox.ErrorQuery("Missing Information", "Text content is required.", "OK");
                return;
            }

            // For password-protected Sends, validate password
            if (_chkPassword != null && _chkPassword.CheckedState == CheckState.Checked &&
                (_txtPassword == null || string.IsNullOrWhiteSpace(_txtPassword.Text.ToString())))
            {
                MessageBox.ErrorQuery("Missing Information",
                    "Password is required when password protection is enabled.", "OK");
                return;
            }

            // Populate the Send object
            _send.Name = _txtName?.Text.ToString() ?? string.Empty;
            _send.Notes = _txtNotes?.Text.ToString() ?? string.Empty;
            _send.Text.Text = _txtText.Text;
            

            if (_datDeletionDate != null)
            {
                _send.DeletionDate = _datDeletionDate.Date;
            }

            if (_datExpirationDate != null)
            {
                _send.ExpirationDate = _datExpirationDate.Date;
            }

            if (_txtMaxAccessCount != null && !string.IsNullOrWhiteSpace(_txtMaxAccessCount.Text.ToString()))
            {
                if (int.TryParse(_txtMaxAccessCount.Text.ToString(), out int maxCount))
                {
                    _send.MaxAccessCount = maxCount;
                }
            }

            if (_chkPassword != null)
            {
                if (_chkPassword.CheckedState == CheckState.Checked && _txtPassword != null)
                {
                    _send.Password = _txtPassword.Text;
                }
            }

            if (_chkDisableSave != null && _chkDisableSave.CheckedState == CheckState.Checked)
            {
                _send.IsDisabled = true;
            }

            if (_chkHideEmail != null && _chkHideEmail.CheckedState == CheckState.Checked)
            {
                _send.IsEmailHidden = true;
            }

            // Set flag for ok button
            _okPressed = true;

            // Close dialog
            Application.RequestStop(_dialog);
        }

        private void PasswordCheckedChanged(bool value)
        {
            if (_txtPassword != null)
            {
                _txtPassword.Visible = value;
            }
        }

        private void InitializeComponent()
        {
            // Create dialog
            _dialog = new Dialog()
            {
                Title = "Create Text Send",
                Width = 70,
                Height = 20
            };

            // Create basic info section
            int row = 1;

            // Name field
            Label lblName = new Label() { Text = "Name:", X = 2, Y = row };
            _txtName = new TextField() { X = 20, Y = row, Width = 45 };
            row += 1;

            // Notes field
            Label lblNotes = new Label() { Text = "Notes:", X = 2, Y = row };
            _txtNotes = new TextView() { X = 20, Y = row, Width = 45, Height = 2 };
            row += 2;

            // Text field
            Label lblText = new Label() { Text = "Text Content:", X = 2, Y = row };
            _txtText = new TextView() { X = 20, Y = row, Width = 45, Height = 3 };
            row += 3;

            // Deletion date field
            Label lblDeletionDate = new Label() { Text = "Deletion Date:", X = 2, Y = row };
            _datDeletionDate = new DateField(DateTime.Now.AddDays(7)) { X = 20, Y = row, Width = 20 };
            row += 1;

            // Expiration date field
            Label lblExpirationDate = new Label() { Text = "Expiration Date:", X = 2, Y = row };
            _datExpirationDate = new DateField() { X = 20, Y = row, Width = 20};
            row += 1;

            // Max access count field
            Label lblMaxAccessCount = new Label() { Text = "Max Access Count:", X = 2, Y = row };
            _txtMaxAccessCount = new TextField() { X = 20, Y = row, Width = 10 };
            row += 1;

            // Password protection
            _chkPassword = new CheckBox() { Text = "Password Protection", X = 2, Y = row };
            _txtPassword = new TextField() { X = 30, Y = row, Width = 35, Secret = true, Visible = false };
            row += 1;

            // Other options
            _chkDisableSave = new CheckBox() { Text = "Disable Save", X = 2, Y = row };
            row += 1;

            _chkHideEmail = new CheckBox() { Text = "Hide Email", X = 2, Y = row };
            row += 1;

            // Create action buttons
            Button btnSave = new Button()
            {
                Text = "_Save",
                X = 20,
                Y = row + 1
            };
            btnSave.Accepting += OkButton_Clicked;

            Button btnCancel = new Button()
            {
                Text = "Cance_l",
                IsDefault = true,
                X = 35,
                Y = row + 1
            };
            btnCancel.Accepting += CancelButton_Clicked;

            // Add all controls to dialog
            _dialog.Add(
                lblName, _txtName,
                lblNotes, _txtNotes,
                lblText, _txtText,
                lblDeletionDate, _datDeletionDate,
                lblExpirationDate, _datExpirationDate,
                lblMaxAccessCount, _txtMaxAccessCount,
                _chkPassword, _txtPassword,
                _chkDisableSave, _chkHideEmail,
                btnSave, btnCancel
            );

            // Set initial focus
            _txtName?.SetFocus();
        }
    }
}