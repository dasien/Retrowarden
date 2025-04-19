/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GeneratePassphraseDialog.cs
 *
 * Dialog for generating a passphrase using the specified options.
 * Displays a dialog with options for number of words, separator,
 * capitalization, and numbers.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Text.RegularExpressions;
using Retrowarden.Workers;
using RetrowardenSDK.Repositories;
using Terminal.Gui;

namespace Retrowarden.Dialogs 
{ 
    public sealed class GeneratePassphraseDialog : BaseDialog
    {
        private Label? _lblPassphrase;
        private FrameView? _fraOptions;
        private CheckBox? _chkCapitalize;
        private CheckBox? _chkIncludeNumbers;
        private Label? _lblNumOfWords;
        private TextField? _txtNumOfWords;
        private Label? _lblSeparator;
        private TextField? _txtSeparator;
        private Button? _btnGeneratePassphrase;
        private Button? _btnCopy;
        private Button? _btnClose;
        
        // Proxy reference.
        private readonly IVaultRepository _repository;
        private string? _passphrase;
        
        public GeneratePassphraseDialog(IVaultRepository repository) 
        {
            // Initialize members.
            _repository = repository;
            _passphrase = "";
            
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            bool retVal = true;

            // Numeric 5-9 allowed for number of words.
            string numberPattern = "^[5-9]+$";
            
            // Check to make sure we have valid controls.
            if (_txtNumOfWords != null && _txtSeparator != null)
            {
                // Check to see if there is a valid value for number of words.
                if (!Regex.IsMatch(_txtNumOfWords.Text ?? string.Empty, numberPattern))
                {
                    MessageBox.ErrorQuery("Values Missing", "Enter a number of words (5-9).", "Ok");
                    retVal = false;
                }

                // Check to see if there is a valid separator.
                else if (!Regex.IsMatch(_txtSeparator.Text ?? string.Empty, "^[!@#$%^&*-=+|_]{1}$"))
                {
                    MessageBox.ErrorQuery("Values Missing", "Enter a valid separator (!@#$%^&*-=+|_).", "Ok");
                    retVal = false;
                }
            }

            else
            {
                retVal = false;
            }
            
            // Return value.
            return retVal;
        }
        
        private void GenerateButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Validate input.
            if (ValidateInput())
            {
                // Set values.
                bool includeNumbers = _chkIncludeNumbers != null && _chkIncludeNumbers.CheckedState == CheckState.Checked;
                bool capitalize = _chkCapitalize != null && _chkCapitalize.CheckedState == CheckState.Checked;
                int words = _txtNumOfWords != null ? Convert.ToInt32(_txtNumOfWords.Text == null ? 1 :_txtNumOfWords.Text) : 1;
                string? sep = _txtSeparator != null ? _txtSeparator.Text : "-";
                
                // Show working dialog.
                GeneratePassphraseWorker worker =
                    new GeneratePassphraseWorker(_repository, capitalize, includeNumbers, words, sep, "Generating Passphrase...");
                
                // Generate the passphrase.
                worker.Run();
                
                // Check to see if we succeeded.
                if (_repository.ExitCode == "0")
                {
                    // Check to see tha the label control is initialized.
                    if (_lblPassphrase != null)
                    {
                        // Show the new passphrase.
                        _lblPassphrase.Text = worker.Passphrase;
                    }

                    // Also store it.
                    _passphrase = worker.Passphrase;
                }
            }
            
        }

        private void CopyButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(_passphrase);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Passphrase copied to clipboard.", "Ok");
        }

        #region Initialize Component
        private void InitializeComponent() 
        {
            _dialog = new Dialog()
            {
                Width = 44, Height = 16, X = Pos.Center(), Y = Pos.Center(), Visible = true, Modal = true,
                TextAlignment = Alignment.Start, Title = "Passphrase Generator"
            };

            _lblPassphrase = new Label()
            {
                Width = 39, Height = 1, X = 2, Y = 1, Visible = true,
                Data = "_lblPassphrase", Text = "No Passphrase Generated", TextAlignment = Alignment.Center
            };
            _dialog.Add(_lblPassphrase);

            _fraOptions = new FrameView()
            {
                Width = 40, Height = 8, X = 1, Y = 3, Visible = true, Data = "_fraOptions",
                TextAlignment = Alignment.Start, Title = "Options"
            };
            _dialog.Add(_fraOptions);

            _chkCapitalize = new CheckBox()
            {
                Width = 6, Height = 1, X = 1, Y = 0, Visible = true, Data = "_chkCapitalize",
                Text = "Capitalize First Letters", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
            _fraOptions.Add(_chkCapitalize);

            _chkIncludeNumbers = new CheckBox()
            {
                Width = 6, Height = 1, X = 1, Y = 1, Visible = true, Data = "_chkIncludeNumbers",
                Text = "Include Numbers", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
           _fraOptions.Add(_chkIncludeNumbers);

           _lblNumOfWords = new Label()
           {
               Width = 4, Height = 1, X = 1, Y = 2, Visible = true,
               Data = "_lblNumOfWords", Text = "Number of Words", TextAlignment = Alignment.Start
           };
           _fraOptions.Add(_lblNumOfWords);

           _txtNumOfWords = new TextField()
           {
               Width = 4, Height = 1, X = 17, Y = 2, Visible = true, Secret = false,
               Data = "_txtNumOfWords", Text = "5", TextAlignment = Alignment.Start
           };
           _fraOptions.Add(_txtNumOfWords);

           _lblSeparator = new Label()
           {
               Width = 4, Height = 1, X = 1, Y = 3, Visible = true, Data = "_lblSeparator",
               Text = "Word Separator", TextAlignment = Alignment.Start
           };
           _fraOptions.Add(_lblSeparator);

           _txtSeparator = new TextField()
           {
               Width = 4, Height = 1, X = 17, Y = 3, Visible = true, Secret = false,
               Data = "_txtSeparator", Text = "-", TextAlignment = Alignment.Start
           };
           _fraOptions.Add(_txtSeparator);

           _btnGeneratePassphrase = new Button()
           {
               Width = 8, Height = 1, X = 1, Y = 11, Visible = true, Data = "_btnGeneratePassphrase",
               Text = "Generate", TextAlignment = Alignment.Center, IsDefault = false
           };
            _btnGeneratePassphrase.Accepting += GenerateButton_Clicked;
            _dialog.Add(_btnGeneratePassphrase);

            _btnCopy = new Button()
            {
                Width = 4, Height = 1, X = 18, Y = 11, Visible = true, Data = "_btnCopy",
                Text = "Copy", TextAlignment = Alignment.Center, IsDefault = false
            };
            _btnCopy.Accepting += CopyButton_Clicked;
            _dialog.Add(_btnCopy);

            _btnClose = new Button()
            {
                Width = 9, Height = 1, X = 32, Y = 11, Visible = true, Data = "_btnClose",
                Text = "Close", TextAlignment = Alignment.Center, IsDefault = false
            };
            _btnClose.Accepting += CancelButton_Clicked;
            _dialog.Add(_btnClose);
        }
        #endregion

        public string? Passphrase
        {
            get { return _passphrase; }
        }
    }
}
