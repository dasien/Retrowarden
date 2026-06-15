/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * SelectOrganizationAndCollectionDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Collections.ObjectModel;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class SelectOrganizationAndCollectionDialog : BaseDialog
    {
        // Controls.
        private DropDownList? _cboOrganization;
        private View? _scrCollections;
        
        // Other values.
        private readonly List<VaultCollection> _collections;
        private readonly List<Organization> _organizations;
        private readonly List<VaultCollection> _selectedCollections;
        
        public SelectOrganizationAndCollectionDialog(List<Organization> organizations, List<VaultCollection> collections)
        {
            // Initialize members.
            _organizations = organizations;
            _collections = collections;
            _selectedCollections = new List<VaultCollection>();
            
            InitializeComponent();
        }
        
        private void HandleOrgSelectionChanged()
        {
            // Clear out currently selected collections (if any).
            _selectedCollections.Clear();
            
            // Create new list of collections for this org.
            CreateCollectionList();
        }
        
        private void CreateCollectionList()
        {
            // Check to see if the scroll view is initialized.
            if (_scrCollections != null)
            {
                // Clear out any existing collection from checkbox view.
                _scrCollections.RemoveAll();
                
                // Check to see if the dropdown is initialized.
                if (_cboOrganization != null)
                {
                    // DropDownList exposes selection as text; map it back to the source index.
                    int orgIndex = _organizations.FindIndex(o => o.ToString() == _cboOrganization.Text);
                    if (orgIndex < 0)
                    {
                        return;
                    }

                    // Get the org id.
                    string orgId = _organizations[orgIndex].Id;

                    // This counter is for the Y of the checkboxes.
                    int row = 0;
                    
                    // Loop through the collections.
                    foreach(VaultCollection col in _collections)
                    {
                        // Check to see if this collection belongs to this org.
                        if (col.OrganizationId == orgId)
                        {
                            // Create the checkbox for the row.
                            CheckBox chkCollection = new CheckBox()
                            {
                                X = 1, Y = row, Width = 20, Height = 1, CanFocus = true, Visible = true,
                                Text = col.Name == null ? "" : col.Name, Data = col
                            };

                            chkCollection.ValueChanging += (_, e) =>
                            {
                                VaultCollection collection = (VaultCollection) chkCollection.Data;
                                
                                // This would indicate movement from checked to unchecked (remove).
                                if (e.CurrentValue == CheckState.Checked)
                                {
                                    // Remove the value.
                                    _selectedCollections.Remove(collection);
                                }
                                
                                // Unchecked to checked (add).
                                else
                                {
                                    // Add the value to the list.
                                    _selectedCollections.Add(collection);
                                }
                            };

                            // Add to scroll view.
                            _scrCollections.Add(chkCollection);
                            
                            // Increment counter.
                            row++;
                        }
                    }
                }
            }
        }
        
        private void OkButton_Clicked(object? sender, CommandEventArgs e)
        {
            // DropDownList exposes selection as text; map it back to the source index.
            int orgIndex = _cboOrganization == null ? -1 : _organizations.FindIndex(o => o.ToString() == _cboOrganization.Text);

            // Check to see if required values are present.
            if (orgIndex == -1)
            {
                MessageBox.ErrorQuery(_dialog?.App, "Values Missing", "Please select an Organization.", "Ok");
            }
            
            else if (_selectedCollections.Count == 0)
            {
                MessageBox.ErrorQuery(_dialog?.App, "Values Missing", "Please select at least one collection.", "Ok");
            }
            
            else
            {
                // Set flag for ok button and values.
                _okPressed = true;

                // Close dialog.
                _dialog.App.RequestStop(_dialog);
            }
        }
        
        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 10, Y =11, Text = "Ok"
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X= 20, Y = 11, Text = "Cancel"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Organization & Collection", Width = 40, Height = 14, Buttons = [okButton, cancelButton]
            };

            // Create label.
            Label lblOrg = new Label()
            {
                X = 1, Y = 0, Width = 4, Height = 1, CanFocus = false, Visible = true, Text = "Organization:"
            };
            
            // Create organization dropdown.
            _cboOrganization = new DropDownList()
            {
                X = 14, Y = 0, Width = 22, Height = 1, CanFocus = true, Visible = true, ReadOnly = true
            };

            // Set source for dropdown.
            ObservableCollection<Organization> orgCollection = new ObservableCollection<Organization>(_organizations);
            _cboOrganization.Source = new ListWrapper<Organization>(orgCollection);

            // Create event to fire when selection changes.
            _cboOrganization.ValueChanged += (_, _) => HandleOrgSelectionChanged();
            
            // Create frame and scroll for list of collections.
            FrameView fraCollections = new FrameView()
            {
                X = 1, Y = 2, Width = 35, Height = 8, CanFocus = true, Visible = true,
                Title = "Organization Collections"
            };
            
            // Create scrollview.
            _scrCollections = new View()
            {
                X = 0, Y = 0, Width = Dim.Percent(100), Height = Dim.Percent(100),
                CanFocus = true,Visible = true
            };
            
            // Add scroll view to frame view.
            fraCollections.Add(_scrCollections);
            
            // Add controls to view.
            _dialog.Add(lblOrg, _cboOrganization, fraCollections);

            // Set default control.
            _cboOrganization.SetFocus();
        }
        
        public Organization? SelectedOrganization
        {
            get
            {
                // The return value.
                Organization? retVal = null;
                
                // Check to see if the dropdown is initialized.
                if (_cboOrganization != null && _organizations != null)
                {
                    // DropDownList exposes selection as text; map it back to the source index.
                    int idx = _organizations.FindIndex(o => o.ToString() == _cboOrganization.Text);
                    if (idx >= 0)
                    {
                        retVal = _organizations[idx];
                    }
                }

                return retVal;
            }
        }

        public List<VaultCollection> SelectedCollections
        {
            get { return _selectedCollections; }
        }
    }
}