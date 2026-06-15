/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SelectItemTypeDialog.cs
 *
 * Dialog for picking a vault item type.
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
    public sealed class SelectItemTypeDialog : BaseDialog
    {
        private OptionSelector? _rdoItemType;
        private int _itemType = 0;
        
        public SelectItemTypeDialog()
        {
            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;
            _itemType = _rdoItemType?.Value ?? 1;
            
            // Close dialog.
            _dialog.App.RequestStop(_dialog);
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

            string[] types = ["Login", "Secure Note", "Card", "Identity"];
        
            // Create radio group.
            _rdoItemType = new OptionSelector()
            {
                X = 8, Y = 1, Labels = types
            };
                
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Item Type", Width = 30, Height = 9
            };
            _dialog.Add(_rdoItemType, okButton, cancelButton);
        }
        
        public int ItemType
        {
            get { return _itemType; }
        }
    }
}