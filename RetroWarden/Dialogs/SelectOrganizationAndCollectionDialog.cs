using RetrowardenSDK.Models;
using Terminal.Gui;


namespace Retrowarden.Dialogs
{
    public sealed class SelectOrganizationAndCollectionDialog : BaseDialog
    {
        // Controls.
        private ComboBox? _cboOrganization;
        private ScrollView? _scrCollections;
        
        // Other values.
        private readonly List<VaultCollection> _collections;
        private readonly List<Organization>? _organizations;
        private readonly List<VaultCollection> _selectedCollections;
        
        public SelectOrganizationAndCollectionDialog(List<Organization>? organizations, List<VaultCollection> collections)
        {
            // Initialize members.
            _organizations = organizations;
            _collections = collections;
            _selectedCollections = new List<VaultCollection>();
            
            InitializeComponent();
        }
        
        private void HandleOrgSelectionChanged(ListViewItemEventArgs obj)
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
                
                // Check to see if the combobox is initialized.
                if (_cboOrganization != null && _organizations != null)
                {
                    // Get the org id.
                    string orgId = _organizations.ElementAt(_cboOrganization.SelectedItem).Id;
                    
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
                                Text = col.Name, Data = col
                            };

                            chkCollection.Toggled += prevValue =>
                            {
                                VaultCollection collection = (VaultCollection)chkCollection.Data;
                                
                                // This would indicate movement from checked to unchecked (remove).
                                if (prevValue)
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

                // Refresh URI scroll view.
                _scrCollections.SetNeedsDisplay();
            }
        }
        
        protected override void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_cboOrganization != null && _cboOrganization.SelectedItem == -1)
            {
                MessageBox.ErrorQuery("Values Missing", "Please select an Organization.", "Ok");
            }
            
            else if (_selectedCollections.Count == 0)
            {
                MessageBox.ErrorQuery("Values Missing", "Please select at least one collection.", "Ok");
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
            Button okButton = new Button(10, 11, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(20, 11, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Select Organization & Collection", 40, 14, okButton, cancelButton);

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
            
            // Create event to fire when selection changes.
            _cboOrganization.SelectedItemChanged += HandleOrgSelectionChanged;
            
            // Create frame and scroll for list of collections.
            FrameView fraCollections = new FrameView()
            {
                X = 1, Y = 2, Width = 35, Height = 8, CanFocus = true, Visible = true,
                Title = "Organization Collections"
            };
            
            // Create scrollview.
            _scrCollections = new ScrollView()
            {
                X = 0, Y = 0, Width = Dim.Percent(100f), Height = Dim.Percent(100f),
                CanFocus = true,Visible = true, ContentSize = new Size(35,100)
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
                
                // Check to see if the combobox is initialized.
                if (_cboOrganization != null && _organizations != null)
                {
                    retVal = _organizations.ElementAt(_cboOrganization.SelectedItem);
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