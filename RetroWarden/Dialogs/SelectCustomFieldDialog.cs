/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SelectCustomFieldDialog.cs
 *
 * Dialog for picking a custom field type.
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
    public sealed class SelectCustomFieldDialog : BaseDialog
    {
        private RadioGroup? _rdoFieldType;
        private int _fieldType = 0;
        
        public SelectCustomFieldDialog()
        {
            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;
            
            if (_rdoFieldType != null)
            {
                _fieldType = _rdoFieldType.SelectedItem;
            }

            // Close dialog.
            Application.RequestStop(_dialog);
        }
        
        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 5, Y = 6, Text = "Ok", IsDefault = true
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 14, Y = 6, Text = "Cancel"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            string[] types = new string[] { "Text", "Hidden", "Boolean", "Linked" };
        
            // Create radio group.
            _rdoFieldType = new RadioGroup()
            {
                X = 8, Y = 1, RadioLabels = types
            };
                
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Item Type", Width = 30, Height = 9
            };
            _dialog.Add(_rdoFieldType, okButton, cancelButton);
        }
        
        public int FieldType
        {
            get { return _fieldType; }
        }
    }
}