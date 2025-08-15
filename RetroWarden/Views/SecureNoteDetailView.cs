/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SecureNoteDetailView.cs
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

namespace Retrowarden.Views 
{
    /// <summary>
    /// Specialized detail view for creating, editing, and viewing secure note vault items
    /// within the Retrowarden Terminal.GUI interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class extends ItemDetailView to provide secure note functionality with:
    /// </para>
    /// <list type="bullet">
    /// <item>Minimal interface focusing on common vault item properties</item>
    /// <item>Secure note type management (currently supports type 0)</item>
    /// <item>Standard three-state editing pattern (View, Edit, Create)</item>
    /// <item>Form validation for required fields</item>
    /// <item>Lightweight implementation for simple text storage</item>
    /// </list>
    /// <para>
    /// Unlike other detail views, SecureNoteDetailView has a minimal interface since
    /// secure notes primarily use the common fields provided by the base ItemDetailView
    /// (name, notes, folder, custom fields). The main content is stored in the notes
    /// field of the base item.
    /// </para>
    /// <para>
    /// The view creates an empty detail view container to satisfy the base class
    /// requirements while focusing user interaction on the common fields provided
    /// by ItemDetailView.
    /// </para>
    /// <para>
    /// Secure note data is stored in the VaultItem.SecureNote property with a type
    /// field that currently defaults to 0, following Bitwarden's secure note structure.
    /// </para>
    /// </remarks>
    public sealed partial class SecureNoteDetailView : ItemDetailView
    {
       public SecureNoteDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state)
        {
            InitializeComponent();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private new void SetupView()
        {
            // Base setup what kind of view state we are in.
            if (_viewState is VaultItemDetailViewState.View or VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }
            
            // Create an empty view so that the base view can resize.
            View empty = new View()
                {
                    X=1, Y=3, Width = 1, Height = 1
                };

            // Set to base.
            DetailView = empty;
            
            // Setup common view parts.
            base.SetupView();
            
            // Set focus to first field.
            SetItemNameControlFocus();
        }
        
        #region Event Handlers

        protected override void SaveButtonClicked(object? sender, CommandEventArgs e)
        {
            // Check to see that an item name is present (it is required).
            if (ItemName.Text == null)
            {
                MessageBox.ErrorQuery("Action failed.", "Item name must have a value.", "Ok");
            }

            else
            {
                // Check to see if the sub object is null (create mode).
                _item.SecureNote ??= new SecureNote();
                
                // Set the note type (currently always 0).
                _item.SecureNote.Type = 0;

                // Update item to current control values.
                UpdateItem();

                // Indicate Save was pressed.
                OkPressed = true;

                // Close dialog.
                Application.RequestStop();
            }
        }
        #endregion
    }
}
