using Terminal.Gui;
using Retrowarden.Models;

namespace Retrowarden.Dialogs
{
    public sealed class AddCollectionDialog : BaseDialog
    {
        // Controls.
        private ComboBox? _cboOrganization;
        private TextField? _txtCollectionName;
        private TabView? _tabCollection;
        private Label? _lblName;
        private Label? _lblOrganization;
        private Label? _lblExternalId;
        private TextField? _txtExternalId;
        private Label? _lblPermission;
        private ComboBox? _cboPermission;
        private Label? _lblMembers;
        private ComboBox? _cboMembers;
        private FrameView? _fraPermissions;
        private ScrollView? _scrPermissionList;
        private Button? _btnOk;
        private Button? _btnCancel;
        private TabView.Tab? _tabCollectionAccess;
        private TabView.Tab? _tabCollectioncollectionInfo;

        // Other values.
        private readonly List<VaultCollection> _collections;
        private readonly List<Organization>? _organizations;
        private List<Member> _members;
        
        public AddCollectionDialog(List<Organization>? organizations, List<VaultCollection> collections)
        {
            // Initialize members.
            _organizations = organizations;
            _collections = collections;
            _members = new List<Member>();
            
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
            // Create dialog.
            _dialog = new Dialog()
            {
                Width = Dim.Percent(55f), Height = Dim.Percent(85f), X = Pos.Center(), Y = Pos.Center(), 
                Visible = true, Modal = true, IsMdiContainer = false, TextAlignment = TextAlignment.Left, 
                Title = "Add Collection", 
            };

            _dialog.Border.BorderStyle = BorderStyle.Single;
            _dialog.Border.Effect3D = true;
            _dialog.Border.Effect3DBrush = null;
            _dialog.Border.DrawMarginFrame = true;

            _tabCollection = new TabView()
            {
                Width = Dim.Fill(1), Height = Dim.Percent(90f), X = 0, Y = 0, Visible = true, 
                Data = "tabCollection", TextAlignment = TextAlignment.Left, MaxTabTextWidth = 30u, 
            };

            _tabCollection.Style.ShowBorder = true;
            _tabCollection.Style.ShowTopLine = true;
            _tabCollection.Style.TabsOnBottom = false;
            
            _tabCollectioncollectionInfo = new TabView.Tab("Collection Info", new View());
            _tabCollectioncollectionInfo.View.Width = Dim.Fill();
            _tabCollectioncollectionInfo.View.Height = Dim.Fill();

            _lblName = new Label()
            {
                Width = 4, Height = 1, X = 9, Y = 3, Visible = true, Data = "lblName", Text = "Name", 
                TextAlignment = TextAlignment.Left
            };
            
            _tabCollectioncollectionInfo.View.Add(_lblName);

            _txtCollectionName = new TextField()
            {
                Width = 30, Height = 1, X = 15, Y = 3, Visible = true, Secret = false, 
                Data = "txtName", Text = "", TextAlignment = TextAlignment.Left
            };
            
            _tabCollectioncollectionInfo.View.Add(_txtCollectionName);

            _lblOrganization = new Label()
            {
                Width = 4, Height = 1, X = 1, Y = 5, Visible = true, Data = "lblOrganization", 
                Text = "Organization", TextAlignment = TextAlignment.Left, 
            };

            _tabCollectioncollectionInfo.View.Add(_lblOrganization);

            _cboOrganization = new ComboBox()
            {
                Width = 30, Height = 4, X = 15, Y = 5, Visible = true, Data = "cboOrganization", 
                Text = "", TextAlignment = TextAlignment.Left, 
            };

            _tabCollectioncollectionInfo.View.Add(_cboOrganization);

            _lblExternalId = new Label()
            {
                Width = 4, Height = 1, X = 2, Y = 7, Visible = true, Data = "lblExternalId", 
                Text = "External Id", TextAlignment = TextAlignment.Left, 
            };
            
            _tabCollectioncollectionInfo.View.Add(_lblExternalId);

            _txtExternalId = new TextField()
            {
                Width = 30, Height = 1, X = 15, Y = 7, Visible = true, Secret = false, 
                Data = "txtExternalId", Text = "", TextAlignment = TextAlignment.Left, 
            };
            
            _tabCollectioncollectionInfo.View.Add(_txtExternalId);
            _tabCollection.AddTab(_tabCollectioncollectionInfo, false);
            
            _tabCollectionAccess = new TabView.Tab("Access", new View());
            _tabCollectionAccess.View.Width = Dim.Fill();
            _tabCollectionAccess.View.Height = Dim.Fill();

            _lblPermission = new Label()
            {
                Width = 4, Height = 1, X = 1, Y = 3, Visible = true, Data = "lblPermission", 
                Text = "Permission", TextAlignment = TextAlignment.Left
            };
            
            _tabCollectionAccess.View.Add(_lblPermission);

            _cboPermission = new ComboBox()
            {
                Width = 18, Height = 2, X = 12, Y = 3, Visible = true, Data = "cboPermission", 
                Text = "", TextAlignment = TextAlignment.Left, 
            };
            
            _tabCollectionAccess.View.Add(_cboPermission);

            _lblMembers = new Label()
            {
 
                Width = 4,  Height = 1,  X = 33,  Y = 3,  Visible = true,  Data = "lblMembers", 
                Text = "Members",  TextAlignment = TextAlignment.Left
            };
            
            _tabCollectionAccess.View.Add(_lblMembers);

            _cboMembers = new ComboBox()
            {
                Width = 18, Height = 2, X = 41, Y = 3, Visible = true, Data = "cboMembers", 
                Text = "", TextAlignment = TextAlignment.Left, 
            };
            
            _tabCollectionAccess.View.Add(_cboMembers);

            _fraPermissions = new FrameView()
            {
                Width = Dim.Fill(1), Height = 9, X = 1, Y = 6, Visible = true, Data = "fraPermissions", 
                TextAlignment = TextAlignment.Left, Title = "Permission List"
            };

            _fraPermissions.Border.BorderStyle = BorderStyle.Single;
            _fraPermissions.Border.Effect3D = false;
            _fraPermissions.Border.Effect3DBrush = null;
            _fraPermissions.Border.DrawMarginFrame = true; 

            _tabCollectionAccess.View.Add(_fraPermissions);

            _scrPermissionList = new ScrollView()
            {
                Width = Dim.Fill(0), Height = Dim.Fill(0), X = 0, Y = 0, Visible = true, 
                ContentSize = new Size(20,10), Data = "scrPermissionList", TextAlignment = TextAlignment.Left
            };

            _fraPermissions.Add(_scrPermissionList);
            _tabCollection.AddTab(_tabCollectionAccess, false);
            _tabCollection.ApplyStyleChanges();
            _dialog.Add(_tabCollection);

            _btnOk = new Button()
            {
                Width = 8, Height = 1, X = 19, Y = 21, Visible = true, Data = "btnOk", Text = "Save", 
                TextAlignment = TextAlignment.Centered, IsDefault = false, 
            };
            
            _btnOk.Clicked += OkButton_Clicked; 
            _dialog.Add(_btnOk);

            _btnCancel = new Button()
            {
                Width = 10, Height = 1, X = 34, Y = 21, Visible = true, Data = "btnCancel", Text = "Cancel", 
                TextAlignment = TextAlignment.Centered, IsDefault = false
            };

            _btnCancel.Clicked += CancelButton_Clicked;
            _dialog.Add(_btnCancel);
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

