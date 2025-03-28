using System.Collections.ObjectModel;
using System.ComponentModel;
using Retrowarden.Utils;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    public partial class IdentityDetailView : ItemDetailView
    {
        private ObservableCollection<CodeListItem> _titles;
        
        public IdentityDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state)
        {
            // Create members.
            _titles = new ObservableCollection<CodeListItem>();
            
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
            DetailView = vwIdentity;

            // Setup common view parts.
            base.SetupView();
            
            // Set tab order.
            //SetTabOrder();

            // Set focus to first field.
            SetItemNameControlFocus();
        }
        
        private new void LoadView()
        {
            // Check to see if we have an identity.
            if (_item.Identity != null)
            {
                // Set current item values to controls.
                txtFirstName.Text = _item.Identity.FirstName ?? "";
                txtMiddleName.Text = _item.Identity.MiddleName ?? "";
                txtLastName.Text = _item.Identity.LastName ?? "";
                txtUserName.Text = _item.Identity.UserName ?? "";
                txtCompany.Text = _item.Identity.Company ?? "";
                txtSSN.Text = _item.Identity.Ssn ?? "";
                txtPassportNumber.Text = _item.Identity.PassportNumber ?? "";
                txtLicenseNumber.Text = _item.Identity.LicenseNumber ?? "";
                txtEmailAddress.Text = _item.Identity.Email ?? "";
                txtPhoneNumber.Text = _item.Identity.Phone ?? "";
                txtAddress1.Text = _item.Identity.Address1 ?? "";
                txtAddress2.Text = _item.Identity.Address2 ?? "";
                txtAddress3.Text = _item.Identity.Address3 ?? "";
                txtCity.Text = _item.Identity.City ?? "";
                txtState.Text = _item.Identity.State ?? "";
                txtZipCode.Text = _item.Identity.PostalCode ?? "";
                txtCountry.Text = _item.Identity.Country ?? "";

                // Set combo box default values.
                cboTitle.SelectedItem = _titles.IndexOf(_titles.First(o => o.Index == _item.Identity.Title));
            }
        }
        
        private void InitializeLists()
        {
            // Load list.
            _titles = CodeListManager.GetObservableCollection("Titles");
            
            // Load titles combo box.
            cboTitle.SetSource(_titles);
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Identity ??= new Identity();

            // Set values.
            _item.Identity.FirstName =  txtFirstName.Text ?? "";
            _item.Identity.MiddleName = txtMiddleName.Text ?? "";
            _item.Identity.LastName = txtLastName.Text ?? "";
            _item.Identity.UserName = txtUserName.Text ?? "";
            _item.Identity.Company = txtCompany.Text ?? "";
            _item.Identity.Ssn = txtSSN.Text ?? "";
            _item.Identity.PassportNumber = txtPassportNumber.Text ?? "";
            _item.Identity.LicenseNumber = txtLicenseNumber.Text ?? "";
            _item.Identity.Email = txtEmailAddress.Text ?? "";
            _item.Identity.Phone = txtPhoneNumber.Text ?? "";
            _item.Identity.Address1 = txtAddress1.Text ?? "";
            _item.Identity.Address2 = txtAddress2.Text ?? "";
            _item.Identity.Address3 = txtAddress3.Text ?? "";
            _item.Identity.City = txtCity.Text ?? "";
            _item.Identity.State = txtState.Text ?? "";
            _item.Identity.PostalCode = txtZipCode.Text ?? "";
            _item.Identity.Country = txtCountry.Text ?? "";
            _item.Identity.Title = _titles.ElementAt(cboTitle.SelectedItem).Index;
            
            // Call base method.
            base.UpdateItem();
        }

        /*protected override void SetTabOrder()
        {
            cboTitle.TabIndex = 0;
            txtFirstName.TabIndex = 1;
            txtMiddleName.TabIndex = 2;
            txtLastName.TabIndex = 3;
            txtSSN.TabIndex = 4;
            txtPassportNumber.TabIndex = 5;
            txtLicenseNumber.TabIndex = 6;
            txtAddress1.TabIndex = 7;
            txtAddress2.TabIndex = 8;
            txtAddress3.TabIndex = 9;
            txtCity.TabIndex = 10;
            txtState.TabIndex = 11;
            txtZipCode.TabIndex = 12;
            txtCountry.TabIndex = 13;
            txtUserName.TabIndex = 14;
            txtCompany.TabIndex = 15;
            txtEmailAddress.TabIndex = 16;
            txtPhoneNumber.TabIndex = 17;
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
        #endregion
    }
}
