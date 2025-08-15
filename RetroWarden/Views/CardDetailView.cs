/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CardDetailView.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Collections.ObjectModel;
using Retrowarden.Utils;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    /// <summary>
    /// Specialized detail view for creating, editing, and viewing credit card vault items
    /// within the Retrowarden Terminal.GUI interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class extends ItemDetailView to provide card-specific functionality including:
    /// </para>
    /// <list type="bullet">
    /// <item>Credit card fields (cardholder name, card number, CVV, expiry)</item>
    /// <item>Brand selection from predefined list (Visa, MasterCard, etc.)</item>
    /// <item>Expiry month selection with dropdown interface</item>
    /// <item>Show/hide functionality for sensitive card data</item>
    /// <item>Clipboard operations for secure copying of card details</item>
    /// <item>Form validation ensuring required fields are populated</item>
    /// </list>
    /// <para>
    /// The view utilizes code lists from CodeListManager for brand and month selections,
    /// providing consistent dropdown experiences. Sensitive fields like card number and
    /// CVV support toggle visibility with dedicated show/hide buttons.
    /// </para>
    /// <para>
    /// Card data is stored in the VaultItem.Card property and follows the standard
    /// three-state pattern (View, Edit, Create) inherited from the base ItemDetailView.
    /// </para>
    /// </remarks>
    public sealed partial class CardDetailView : ItemDetailView
    {
        private ObservableCollection<CodeListItem> _cardBrands;
        private ObservableCollection<CodeListItem> _expMonths;
        
        public CardDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state)
        {
            // Create members.
            _cardBrands = new ObservableCollection<CodeListItem>();
            _expMonths = new ObservableCollection<CodeListItem>();
            
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
            
            // Set our main view to the view area of the parent view.
            DetailView = vwCard;

            // Setup common view parts.
            base.SetupView();
            
            // Set focus to first field.
            SetItemNameControlFocus(); 
        }

        private new void LoadView()
        {
            // Set values.
            if (_item.Card != null)
            {
                txtCardholderName.Text = _item.Card.CardholderName ?? "";
                txtCardNumber.Text = _item.Card.CardNumber ?? "";
                txtExpYear.Text = _item.Card.ExpiryYear ?? "";
                txtCVV.Text = _item.Card.SecureCode ?? "";

                // Set combo box default values.
                cboCardBrand.SelectedItem = _cardBrands.IndexOf(_cardBrands.First(o => o.Index == _item.Card.Brand));
                cboExpMonth.SelectedItem = _expMonths.IndexOf(_expMonths.First(o => o.Index == _item.Card.ExpiryMonth));
            }
        }
        
        private void InitializeLists()
        {
            // Load lists.
            _cardBrands = CodeListManager.GetObservableCollection("CardBrands");
            _expMonths = CodeListManager.GetObservableCollection("ExpiryMonths");
            
            // Set combox sources.
            cboCardBrand.SetSource(_cardBrands);
            cboExpMonth.SetSource(_expMonths);
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Card ??= new Card();
            
            // Set values.
            _item.Card.CardholderName =  txtCardholderName.Text ?? "";
            _item.Card.CardNumber = txtCardNumber.Text ?? "";
            _item.Card.ExpiryYear = txtExpYear.Text ?? "";
            _item.Card.SecureCode = txtCVV.Text ?? "";
            _item.Card.Brand = _cardBrands.ElementAt(cboCardBrand.SelectedItem).Index;
            _item.Card.ExpiryMonth = _expMonths.ElementAt(cboExpMonth.SelectedItem).Index;
            
            // Call base method.
            base.UpdateItem();
            
        }
        
        /*protected override void SetTabOrder()
        {
            // Set tab order for controls.
            txtCardholderName.TabIndex = 0;
            cboCardBrand.TabIndex = 1;
            txtCardNumber.TabIndex = 2;
            btnShowCardNumber.TabIndex = 3;
            btnCopyCardNumber.TabIndex = 4;
            txtCVV.TabIndex = 5;
            btnShowCVV.TabIndex = 6;
            btnCopyCVV.TabIndex = 7;
            cboExpMonth.TabIndex = 8;
            txtExpYear.TabIndex = 9;
            
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
                
                // Indicate Save was pressed.
                OkPressed = true;
                
                // Close dialog.
                Application.RequestStop();
            }
        }

        private void ShowCardButtonClicked(object? sender, CommandEventArgs e)
        {
            // Toggle Flag.
            txtCardNumber.Secret = !txtCardNumber.Secret;
            
            // Flip button text to opposite action.
            btnShowCardNumber.Text = txtCardNumber.Secret ? "Show" : "Hide";
        }

        private void CopyCardButtonClicked(object? sender, CommandEventArgs e)
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtCardNumber.Text);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Card number copied to clipboard.", "Ok");
        }

        private void ShowCVVButtonClicked(object? sender, CommandEventArgs e)
        {
            // Toggle Flag.
            txtCVV.Secret = !txtCVV.Secret;
            
            // Flip button text to opposite action.
            btnShowCVV.Text = txtCVV.Secret ? "Show" : "Hide";
        }

        private void CopyCVVButtonClicked(object? sender, CommandEventArgs e)
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtCVV.Text);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Card CVV copied to clipboard.", "Ok");
        }
        #endregion
    }
}
