/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * LoginDetailView.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Collections.ObjectModel;
using Retrowarden.Controls;
using Retrowarden.Dialogs;
using Retrowarden.Utils;
using RetrowardenSDK.Models;
using RetrowardenSDK.Repositories;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    /// <summary>
    /// Specialized detail view for creating, editing, and viewing login credential vault items
    /// within the Retrowarden Terminal.GUI interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class extends ItemDetailView to provide login-specific functionality including:
    /// </para>
    /// <list type="bullet">
    /// <item>Username and password field management with show/hide capability</item>
    /// <item>TOTP (Time-based One-Time Password) field for two-factor authentication</item>
    /// <item>Dynamic URI list management with match detection settings</item>
    /// <item>Integrated password generation using vault repository</item>
    /// <item>Clipboard operations for secure copying of credentials</item>
    /// <item>Custom URI scroll view for managing multiple website URLs</item>
    /// </list>
    /// <para>
    /// The view includes a sophisticated URI management system through the UriScrollView
    /// control, allowing users to associate multiple websites with login credentials
    /// and configure match detection behaviors for each URL.
    /// </para>
    /// <para>
    /// Password generation is integrated directly into the form, enabling users to
    /// generate secure passwords without leaving the detail view. The view requires
    /// a vault repository reference for accessing password generation capabilities.
    /// </para>
    /// <para>
    /// Login data is stored in the VaultItem.Login property and supports the standard
    /// three-state editing pattern (View, Edit, Create) inherited from ItemDetailView.
    /// </para>
    /// </remarks>
    public sealed partial class LoginDetailView : ItemDetailView
    {
        private ObservableCollection<CodeListItem> _matchDetections;
        private readonly IVaultRepository _repository;
        
        public LoginDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state, IVaultRepository repository) 
            : base (item, folders, state)
        {
            // Initialize members.
            _repository = repository;
            _matchDetections = new ObservableCollection<CodeListItem>();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private new void SetupView()
        {
            InitializeComponent();
            
            // Initialize any list controls.
            InitializeLists();
            
            // Base setup what kind of view state we are in.
            if (_viewState is VaultItemDetailViewState.View or VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }

            else
            {
                // Create empty URI scroll.
                scrURIList = new UriScrollView(null, _matchDetections)
                {
                    X = 0, Y = 0, Width = 95, Height = 5, Visible = true, CanFocus = true, Enabled = true, 
                    Data = "scrURIList", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabGroup
                };
                
                // Add it to the frame.
                fraURIList.Add(scrURIList);
            }
            
            // Set our main view to the view area of the parent view.
            DetailView = vwLogin;

            // Setup common view parts.
            base.SetupView();
            
            // Set tab order.
            //SetTabOrder();
            
            // Allow focusing in the frame (fix bug that was causing some of the views to not be focused).
            //fraURIList.FocusFirst();
            
            // Set focus to first field.
            SetItemNameControlFocus();
        }

        private new void LoadView()
        {
            // Check to make sure we have a login.
            if (_item.Login != null)
            {
                // Set current item values to controls.
                txtUserName.Text = _item.Login.UserName ?? "";
                txtPassword.Text = _item.Login.Password ?? "";
                txtTOTP.Text = _item.Login.TOTP ?? "";

                // Handle loading the list of other URIs.
                CreateUriListRows();
            }
        }
        
        private void InitializeLists()
        {
            // Create list of match types.
            _matchDetections = CodeListManager.GetObservableCollection("MatchDetections");
        }

        private void CreateUriListRows()
        {
            // Make sure we have a login object.
            if (_item.Login != null)
            {
                // Create new URI scroll control
                scrURIList = new UriScrollView(_item.Login.URIs, _matchDetections)
                {
                    X = 0, Y = 0, Width = 95, Height = 5, Visible = true, CanFocus = true, Enabled = true,
                    Data = "scrURIList", TextAlignment = Alignment.Start
                };

                fraURIList.Add(scrURIList);
            }
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Login ??= new Login();
            
            // Set item properties.
            _item.Login.UserName = txtUserName.Text;
            _item.Login.Password = txtPassword.Text;
            _item.Login.TOTP = txtTOTP.Text;

            // Update the URI list.
            _item.Login.URIs = scrURIList.URIs;
            
            // Call base method.
            base.UpdateItem();
        }

        /*protected override void SetTabOrder()
        {
            // Set tab order for controls.
            txtUserName.TabIndex = 0;
            btnCopyUserName.TabIndex = 1;
            txtPassword.TabIndex = 2;
            btnViewPassword.TabIndex = 3;
            btnCopyPassword.TabIndex = 4;
            btnGeneratePassword.TabIndex = 5;
            txtTOTP.TabIndex = 6;
            scrURIList.TabIndex = 7;
            
            // Call base order set.
            base.SetTabOrder();   
        }*/
        
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
                // Update item to current control values.
                UpdateItem();

                // Check to see which save mode we are in.
                switch (_viewState)
                {
                    case VaultItemDetailViewState.Create:
                        break;

                    case VaultItemDetailViewState.Edit:
                        break;
                }

                // Flag that the save button was pressed and close form.
                OkPressed = true;
                Application.RequestStop();
            }
        }

        private void CopyPasswordButtonClicked(object? sender, CommandEventArgs e)
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(txtPassword.Text);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");

        }

        private void ViewPasswordButtonClicked(object? sender, CommandEventArgs e)
        {
            // Toggle Flag.
            txtPassword.Secret = !txtPassword.Secret;
            
            // Flip button text to opposite action.
            btnViewPassword.Text = txtPassword.Secret ? "Show" : "Hide";
        }

        private void CopyUserNameButtonClicked(object? sender, CommandEventArgs e)
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtUserName.Text);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "User name copied to clipboard.", "Ok");
        }

        private void GeneratePasswordButtonClicked(object? sender, CommandEventArgs e)
        {
            GeneratePasswordDialog genPass = new GeneratePasswordDialog(_repository);
            genPass.Show();
            
            // Check to see if a password was generated.
            if (genPass.Password!= null && genPass.Password.Length > 0)
            {
                // Set password.
                txtPassword.Text = genPass.Password;
            }
        }

        private void NewUriButtonClicked(object? sender, CommandEventArgs e)
        {
            // Call scroll view add row method.
            scrURIList.CreateControlRow();
        }
        #endregion
    }
}
