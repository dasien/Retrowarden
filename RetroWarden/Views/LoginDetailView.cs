using Terminal.Gui;
using Retrowarden.Controls;
using Retrowarden.Dialogs;
using Retrowarden.Models;
using Retrowarden.Utils;
using Retrowarden.Repositories;

namespace Retrowarden.Views 
{
    public partial class LoginDetailView : ItemDetailView
    {
        private List<CodeListItem> _matchDetections;
        private readonly VaultRepository _repository;
        
        // This sizes the underlying view appropriately.
        private const int scrollBottom = 39;
        
        public LoginDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state, VaultRepository repository) 
            : base (item, folders, state, scrollBottom)
        {
            // Initialize members.
            _repository = repository;
            _matchDetections = new List<CodeListItem>();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private new void SetupView()
        {
            InitializeComponent();
            
            // Initialize any list controls.
            InitializeLists();
            
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View || _viewState == VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }

            else
            {
                // Create empty URI scroll.
                scrURIList = new UriScrollView(null, _matchDetections);
            }
            
            // Set our main view to the view area of the parent view.
            DetailView = vwLogin;

            // Setup common view parts.
            base.SetupView();
            
            // Set tab order.
            SetTabOrder();
            
            // Allow focusing in the frame (fix bug that was causing somme of the views to not be focused).
            fraURIList.FocusFirst();
            
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
            _matchDetections = CodeListManager.GetList("MatchDetections");
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
                    ContentSize = new Size(95, 10), Data = "scrURIList", TextAlignment = TextAlignment.Left
                };

                fraURIList.Add(scrURIList);
            }
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Login ??= new Login();
            
            // Set item properties.
            _item.Login.UserName = txtUserName.Text.ToString();
            _item.Login.Password = txtPassword.Text.ToString();
            _item.Login.TOTP = txtTOTP.Text.ToString();

            // Update the URI list.
            _item.Login.URIs = scrURIList.URIs;
            
            // Call base method.
            base.UpdateItem();
        }

        protected override void SetTabOrder()
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
        }
        
        #region Event Handlers
        protected override void SaveButtonClicked()
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

        private void CopyPasswordButtonClicked()
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(txtPassword.Text.ToString());

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");

        }

        private void ViewPasswordButtonClicked()
        {
            // Toggle Flag.
            txtPassword.Secret = !txtPassword.Secret;
            
            // Flip button text to opposite action.
            btnViewPassword.Text = txtPassword.Secret ? "Show" : "Hide";
        }

        private void CopyUserNameButtonClicked()
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtUserName.Text.ToString());

            // Indicate data copied.
            MessageBox.Query("Action Completed", "User name copied to clipboard.", "Ok");
        }

        private void GeneratePasswordButtonClicked()
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

        private void NewUriButtonClicked()
        {
            // Call scroll view add row method.
            scrURIList.CreateControlRow();
        }
        #endregion
    }
}
