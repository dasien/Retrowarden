/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CreateFileSendDialog.cs
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
    public sealed class CreateFileSendDialog : BaseSendDialog
    {
        // Controls
        private TextField? _txtName;
        private TextView? _txtNotes;
        private TextField? _txtFilePath;
        private DateField? _datDeletionDate;
        private DateField? _datExpirationDate;
        private TextField? _txtMaxAccessCount;
        private CheckBox? _chkPassword;
        private TextField? _txtPassword;
        private CheckBox? _chkDisableSave;
        private CheckBox? _chkHideEmail;

        private string _filePath;

        public CreateFileSendDialog() : base("1")
        {
            _filePath = string.Empty;
            _okPressed = false;

            InitializeComponent();
        }

        private void SelectFileButton_Clicked(object? sender, CommandEventArgs e)
        {
            OpenDialog openDialog = new OpenDialog(){Title = "Select File", Text = "Select the file you want to send."};
            Application.Run(openDialog);

            if (!openDialog.Canceled && openDialog.Path != null)
            {
                _filePath = openDialog.Path;
                if (_txtFilePath != null)
                {
                    _txtFilePath.Text = _filePath;
                }
            }
        }

        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(_txtName?.Text.ToString()))
            {
                MessageBox.ErrorQuery("Missing Information", "Send name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(_filePath))
            {
                MessageBox.ErrorQuery("Missing Information", "A file must be selected.", "OK");
                return;
            }

            // For password-protected Sends, validate password
            if (_chkPassword != null && _chkPassword.CheckedState == CheckState.Checked &&
                string.IsNullOrWhiteSpace(_txtPassword?.Text.ToString()))
            {
                MessageBox.ErrorQuery("Missing Information",
                    "Password is required when password protection is enabled.", "OK");
                return;
            }

            // Populate the Send object
            _send.Name = _txtName?.Text.ToString() ?? string.Empty;
            _send.Notes = _txtNotes?.Text.ToString() ?? string.Empty;

            _send.File = new SendFile()
            {
                FileName = Path.GetFileName(_filePath)
            };

            if (_datDeletionDate != null)
                _send.DeletionDate = _datDeletionDate.Date;

            if (_datExpirationDate != null)
                _send.ExpirationDate = _datExpirationDate.Date;

            if (_txtMaxAccessCount != null && !string.IsNullOrWhiteSpace(_txtMaxAccessCount.Text.ToString()))
                if (int.TryParse(_txtMaxAccessCount.Text.ToString(), out int maxCount))
                    _send.MaxAccessCount = maxCount;

            if (_chkPassword != null)
            {
                if (_chkPassword.CheckedState == CheckState.Checked && _txtPassword != null)
                    _send.Password = _txtPassword.Text;
            }

            if (_chkDisableSave != null)
                _send.IsDisabled = _chkDisableSave.CheckedState == CheckState.Checked ? true : false;

            if (_chkHideEmail != null)
                _send.IsEmailHidden = _chkHideEmail.CheckedState == CheckState.Checked ? true : false;

            _okPressed = true;
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
            _dialog = new Dialog()
            {
                Title = "Create File Send",
                Width = 70,
                Height = 20
            };

            int row = 1;

            // Name
            Label lblName = new Label() { Text = "Name:", X = 2, Y = row };
            _txtName = new TextField() { X = 20, Y = row, Width = 45 };
            row++;

            // File selection
            Label lblFile = new Label() { Text = "File:", X = 2, Y = row };
            
            _txtFilePath = new TextField() { X = 20, Y = row, Width = 33, ReadOnly = true };
            Button btnSelectFile = new Button() { Text = "Select...", X = 54, Y = row };
            btnSelectFile.Accepting += SelectFileButton_Clicked;
            row++;

            // Notes
            Label lblNotes = new Label() { Text = "Notes:", X = 2, Y = row };
            
            _txtNotes = new TextView() { X = 20, Y = row, Width = 45, Height = 2 };
            row += 2;

            // Deletion date
            Label lblDeletionDate = new Label() { Text = "Deletion Date:", X = 2, Y = row };
            
            _datDeletionDate = new DateField() { Date = DateTime.Now.AddDays(7), X = 20, Y = row, Width = 20 };
            row++;

            // Expiration date
            Label lblExpirationDate = new Label() { Text = "Expiration Date:", X = 2, Y = row };
            
            _datExpirationDate = new DateField() { X = 20, Y = row, Width = 20 };
            row++;

            // Max access count
            Label lblMaxAccessCount = new Label() { Text = "Max Access Count:", X = 2, Y = row };
            
            _txtMaxAccessCount = new TextField() { X = 20, Y = row, Width = 10 };
            row++;

            // Password protection
            _chkPassword = new CheckBox() { Text = "Password Protection", X = 2, Y = row };
            _txtPassword = new TextField() { X = 30, Y = row, Width = 35, Secret = true, Visible = false };
            row++;

            // Other options
            _chkDisableSave = new CheckBox() { Text = "Disable Save", X = 2, Y = row };
            row++;
            _chkHideEmail = new CheckBox() { Text = "Hide Email", X = 2, Y = row };
            row += 2;

            // Buttons
            Button btnSave = new Button() { Text = "_Save", X = 20, Y = row, IsDefault = true };
            btnSave.Accepting += OkButton_Clicked;
            
            Button btnCancel = new Button() { Text = "_Cancel", X = 35, Y = row };
            btnCancel.Accepting += CancelButton_Clicked;

            _dialog.Add(
                lblName, _txtName,
                lblFile, _txtFilePath, btnSelectFile,
                lblNotes, _txtNotes,
                lblDeletionDate, _datDeletionDate,
                lblExpirationDate, _datExpirationDate,
                lblMaxAccessCount, _txtMaxAccessCount,
                _chkPassword, _txtPassword,
                _chkDisableSave, _chkHideEmail,
                btnSave, btnCancel
            );

            _txtName?.SetFocus();
        }
        
        /// <summary>
        /// Gets the full path of the file selected by the user.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }
    }
}