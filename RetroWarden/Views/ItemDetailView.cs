/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ItemDetailView.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/using System.Collections.ObjectModel;
using Retrowarden.Controls;
using Retrowarden.Dialogs;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    public abstract partial class ItemDetailView
    {
        private View? _subView;
        protected VaultItem _item;
        private readonly List<VaultFolder> _folders;
        protected readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;
        
        protected ItemDetailView(VaultItem? item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            
            InitializeComponent();
        }
        
        public void Show()
        {
            Application.Run(this);
        }

        protected void SetupView()
        {
            AssembleView();
            
            // Check to make sure we have a sub-view to add (Secure Note doesn't have one)
            if (_subView != null)
            {
                // Update location of 'footer' controls to below the particular detail view.
                RelocateFooterControls();
            }

            // Initialize any list controls.
            InitializeLists();
            
            // Load form based on action.
            switch (_viewState)
            {
                case VaultItemDetailViewState.Create:
                    
                    // Set title.
                    this.Title = "Create New Item";
                    
                    // Create empty custom field scroll.
                    scrCustomFields = new CustomFieldScrollView(null, _item.ItemType)
                    {
                        X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true, 
                        Data = "scrCustomFields", TextAlignment = Alignment.Start
                    };
                    
                    // Add scroll to view.
                    fraCustomFieldList.Add(scrCustomFields);

                    break;

                case VaultItemDetailViewState.Edit:
                    
                    // Load controls with current data only.
                    LoadView();
                    
                    // Set title.
                    this.Title = "Edit Item - " + _item.ItemName;
                    break;

                case VaultItemDetailViewState.View:
                    
                    // Load controls with current data only.
                    LoadView();
                    
                    // Disable control state.
                    DisableView();
                    
                    // Set title.
                    this.Title = "View Item - " + _item.ItemName;
                    break;
            }
        }

        protected void LoadView()
        {
            // Set current item values to controls.
            txtItemName.Text = _item.ItemName;
            chkFavorite.CheckedState = _item.IsFavorite ? CheckState.Checked : CheckState.UnChecked;
            tvwNotes.Text = _item.Notes ?? "";
            chkReprompt.CheckedState = _item.Reprompt == 1 ? CheckState.Checked : CheckState.UnChecked;
            stbDetail.Add(new Shortcut(KeyCode.Null, "Created On: " + _item.CreationDate + 
                                                     " | Last Updated On: " + _item.RevisionDate, null));
            
            // Set the folder to the current folder (or "No Folder" as a null default.
            cboFolder.SelectedItem = _folders.FindIndex(o => o.Id == _item.FolderId);
            
            // Create new field scroll control.
            scrCustomFields = new CustomFieldScrollView(_item.CustomFields, _item.ItemType)
            {
                X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true,
                Data = "scrCustomFields", TextAlignment = Alignment.Start
            };
            
            // Add scroll to view.
            fraCustomFieldList.Add(scrCustomFields);
        }

        protected void SetItemNameControlFocus()
        {
            // This is part of a bug fix that allows focus in the URI list frame.
            //txtItemName.HasFocus = true;
        }
        
        private void DisableView()
        {
            btnSave.Enabled = false;
        }

        private void AssembleView()
        {
            // Add header controls.
            this.Add(this.lblItemName);
            this.Add(this.lblFolder);
            this.Add(this.txtItemName);
            this.Add(this.cboFolder);
            this.Add(this.chkFavorite);
            this.Add(this.chkReprompt);

            // Add custom view.
            // Check to make sure we have a sub-view to add (Secure Note doesn't have one)
            if (_subView != null)
            {
                // Add subview to view.
                Add(_subView);
            }
            
            // Add footer controls.
            this.fraNotes.Add(this.tvwNotes);
            this.Add(this.fraNotes);
            this.Add(this.fraCustomFieldList);
            this.Add(this.btnNewCustomField);
            this.Add(this.btnSave);
            this.Add(this.btnCancel);
            this.Add(this.stbDetail);

        }
        
        private void InitializeLists()
        {
            // Set folder comboxbox source.
            ObservableCollection<VaultFolder> folderCollection = new ObservableCollection<VaultFolder>(_folders);
            cboFolder.SetSource(folderCollection);
        }

        private void RelocateFooterControls()
        {
            // Update Y of the notes frame to detail view bottom + 2
            fraNotes.Y = Pos.Bottom(_subView) + 1;
            
            // Update the custom fields frame.
            fraCustomFieldList.Y = Pos.Bottom(fraNotes) + 1;
            btnNewCustomField.Y = Pos.Bottom(fraCustomFieldList);
            
            // Update the save and cancel button locations relative to the new notes frame.
            btnSave.Y = Pos.Bottom(btnNewCustomField) + 1;
            btnCancel.Y = Pos.Bottom(btnNewCustomField) + 1;
        }
        
        protected virtual void UpdateItem()
        {
            // Set item properties.
            _item.ItemName = txtItemName.Text ?? "";
            
            // Check to see if there is a selected item.
            if (cboFolder.SelectedItem == -1)
            {
                // Set to "No Folder"
                _item.FolderId = null;
            }

            else
            {
                _item.FolderId = _folders.ElementAt(cboFolder.SelectedItem).Id;
            }
            
            _item.IsFavorite = chkFavorite.CheckedState == CheckState.Checked;
            _item.Notes = tvwNotes.Text;
            _item.Reprompt = chkReprompt.CheckedState == CheckState.Checked ? 1 : 0;
            
            // Get the custom fields.
            _item.CustomFields = scrCustomFields.Fields;
        }
        
        #region  Properties
        public bool OkPressed
        {
            get { return _okPressed; }
            set { _okPressed = value; }
        }

        public VaultItem Item
        {
            get { return _item; }
        }

        protected TextField ItemName
        {
            get { return this.txtItemName; }
        }

        protected View DetailView
        {
            set { _subView = value; }
        }

        #endregion
        
        #region Event Handlers
        protected abstract void SaveButtonClicked(object? sender, CommandEventArgs e);
        
        private void CancelButtonClicked(object? sender, CommandEventArgs e)
        {
            // Close dialog.
            Application.RequestStop();
        }

        protected void HandleFocusChange(View? sender, HasFocusEventArgs eventArgs)
        {
            // This offset is necessary because of labels being over controls vertically.
            int offset = 1;
            
            // Get the view which is receiving focus.
            View gettingFocus = eventArgs.NewFocused;
            
            // Check to see if the view below the viewport.
            if (gettingFocus.Frame.Y + gettingFocus.Frame.Height > Viewport.Bottom)
            {
                // Scroll down until control is in the frame.
                ScrollVertical(gettingFocus.Frame.Y + gettingFocus.Frame.Height - Viewport.Bottom);
            }
            
            // Check to see if the view is above the viewport.
            else if (gettingFocus.Frame.Top < Viewport.Top)
            {
                // Scroll up until control is in the frame.
                ScrollVertical(gettingFocus.Frame.Top - Viewport.Top - offset);
            }
            
            // Check to see if the view is too far right.
            if (gettingFocus.Frame.X + gettingFocus.Frame.Width > Viewport.Right)
            {
                // Scroll right until control is in frame.
                ScrollHorizontal(gettingFocus.Frame.X + gettingFocus.Frame.Width - Viewport.Right);
            }
            
            // Check to see if the view is too far left.
            if (gettingFocus.Frame.Left < Viewport.Left)
            {
                // Scroll left until control is in frame.
                ScrollHorizontal(gettingFocus.Frame.Left - Viewport.Left);
            }

            // If this is a textfield, select the text in it.
            if (sender is TextField)
            {
                ((TextField)sender).SelectAll();
            }
        }

        private void NewCustomFieldButtonClicked(object? sender, CommandEventArgs e)
        {
            // Show the dialog for field type choice.
            SelectCustomFieldDialog dialog = new SelectCustomFieldDialog();
            dialog.Show();
            
            // Check to see what was selected.
            if (dialog.OkPressed)
            {
                // Create a new control row.
                scrCustomFields.CreateControlRow(dialog.FieldType);
            }
        }
        
        private void HandleMouseEvent(object? sender, MouseEventArgs MouseEventArgs)
        {
            switch (MouseEventArgs.Flags)
            {
                // Check to see if this is a wheel scroll down.
                case MouseFlags.WheeledDown:
                    
                    // Check to see if we are past the bottom of the view.
                    if (Viewport.Top < Frame.Bottom)
                    {
                        ScrollVertical(1);
                    }
                    break;
                
                case MouseFlags.WheeledUp:
                    
                    // Check to see if we are past the top of the view.
                    if (Viewport.Bottom > Frame.Top)
                    {
                        ScrollVertical(-1);
                    }
                    break;
                
                case MouseFlags.WheeledLeft:
                    
                    // Check to see if we are past the left.
                    if (Viewport.Left > Frame.Right)
                    {
                        ScrollHorizontal(-1);
                    }
                    break;
                
                case MouseFlags.WheeledRight:
                    
                    // Check to see if we are past the right.
                    if (Viewport.Right < Frame.Left)
                    {
                        ScrollHorizontal(1);
                    }
                    break;
            }
        }
        
        #endregion
    }
}
