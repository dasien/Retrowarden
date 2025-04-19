/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * WorkingDialog.cs
 *
 * Progress indication dialog for long-running operations. Provides
 * visual feedback during background tasks and async operations
 * with customizable progress messages.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class WorkingDialog : BaseDialog
    {
        // Controls.
        private SpinnerView? _spnAnimation;
        private Label? _lblMessage;
        private Label? _lblProgress;
        private readonly string _messageText;
        
        // Other values.
        private readonly string[] _spinFrames = [
            "01100010", "01101001", "01110100", "01110111", "01100001", "01110010", "01100100", "01100101",
            "01101110"
        ];
        
        public WorkingDialog(string message)
        {
            // Initialize members.
            _messageText = message;
            
            InitializeComponent();
        }

        public new void Show()
        {
            if (_dialog != null)
            {
                Application.Run(_dialog);
            }
        }

        public void Hide()
        {
            Application.RequestStop(_dialog);
        }

        private void InitializeComponent()
        {
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Working...", Width = 40, Height = 6
            };

            // Create controls.
            _spnAnimation = new SpinnerView()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true,
                AutoSpin = true, SpinDelay = 80, Sequence = _spinFrames, Data = "spnAnimation"
            };

            _lblMessage = new Label()
            {
                X = 13, Y = 2, Width = 35, Height = 1, CanFocus = false, Visible = true,
                Text = _messageText, Data = "lblMessage"
            };

            _lblProgress = new Label()
            {
                X = 13, Y = 3, Width = 35, Height = 1, CanFocus = false, Visible = true,
                Text = "", Data = "lblProgress"
            };
            
            // Add controls to view.
            _dialog.Add(_spnAnimation, _lblMessage, _lblProgress);
        }
        
        public string ProgressMessage
        {
            set
            {
                // Check to see if the control has been created.
                if (_lblProgress != null)
                {
                    _lblProgress.Text = value;
                }
            }
        }
        public bool IsCurrentTop
        {
            get
            {
                return _dialog != null && _dialog.IsCurrentTop;
            }
        }
    }
}