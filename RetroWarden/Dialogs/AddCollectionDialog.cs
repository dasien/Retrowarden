using Terminal.Gui;
using Retrowarden.Models;

namespace Retrowarden.Dialogs
{
    public sealed class AddCollectionDialog : BaseDialog
    {
        // Controls.
        private ComboBox? _cboOrganization;
        private TextField? _txtCollectionName;
        
        // Other values.
        private readonly List<VaultCollection> _collections;
        private readonly List<Organization> _organizations;

        public AddCollectionDialog(List<Organization> organizations, List<VaultCollection> collections)
        {
            // Initialize members.
            _organizations = organizations;
            _collections = collections;
            
            // Initialize dialog.
            InitializeComponent();
        }

        protected override void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_cboOrganization != null && _cboOrganization.SelectedItem == -1)
            {
                MessageBox.ErrorQuery("Values Missing", "Please select an Organization.", "Ok");
            }
            
            else if (_txtCollectionName != null && _txtCollectionName.Text.Length == 0)
            {
                MessageBox.ErrorQuery("Values Missing", "Please name the collection.", "Ok");
            }
            
            else
            {
                // Set flag for ok button and values.
                _okPressed = true;

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        protected override void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button(10, 6, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(20, 6, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Create Collection", 40, 8, okButton, cancelButton);

            // Create label.
            Label lblOrg = new Label()
            {
                X = 1, Y = 0, Width = 4, Height = 1, CanFocus = false, Visible = true, Text = "Organization:"
            };
            
            // Create folder dropdown.
            _cboOrganization = new ComboBox()
            {
                X = 14, Y = 0, Width = 22, Height = 5, CanFocus = true, Visible = true
            };
            
            // Set source for combobox.
            _cboOrganization.SetSource(_organizations);
            
            // Create label.
            Label lblCollectionName = new Label()
            {
                X = 1, Y = 2, Width = 4, Height = 1, CanFocus = false, Visible = true, Text = "Collection:"
            };
            
            // Create textbox.
            _txtCollectionName = new TextField()
            {
                X = 14, Y = 2, Width = 22, Height = 1, CanFocus = true, Visible = true

            };
            
            // Add controls to view.
            _dialog.Add(lblOrg, _cboOrganization, lblCollectionName, _txtCollectionName);

            // Set default control.
            _cboOrganization.SetFocus();

        }
        
        public Organization SelectedOrganization
        {
            get
            {
                if (_cboOrganization != null)
                {
                    return _organizations.ElementAt(_cboOrganization.SelectedItem);
                }

                else
                {
                    return _organizations.First();
                }
            }
        }

        public string CollectionName
        {
            get
            {
                if (_txtCollectionName != null)
                {
                    return _txtCollectionName.Text.ToString() ?? string.Empty;
                }

                else
                {
                    return string.Empty;
                }
            }
        }

    }    
}

