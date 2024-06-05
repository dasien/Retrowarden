using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public sealed class ConnectDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtUserId;
        private TextField? _txtPassword;

        // Other values.
        private string _userId;
        private string _password;
        
        public ConnectDialog() 
        {
            _userId = "";
            _password = ""; 
            _okPressed = false;
            
            InitializeComponent();
        }

        protected override void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_txtPassword != null && _txtUserId != null 
               && (_txtUserId.Text.TrimSpace().Length == 0 || _txtPassword.Text.TrimSpace().Length == 0))
            {
                MessageBox.ErrorQuery("Values Missing", "Both User Id and Password are required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtUserId != null)
                {
                    _userId = _txtUserId.Text.ToString() ?? string.Empty;
                }

                if (_txtPassword != null)
                {
                    _password = _txtPassword.Text.ToString() ?? string.Empty;
                }

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        protected override void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button(8, 6, "_Connect");
            okButton.Clicked += OkButton_Clicked;
            okButton.IsDefault = true;

            // Create Cancel button.
            Button cancelButton = new Button(24, 6, "Cance_l");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Connect to Vault", 40, 10, okButton, cancelButton);

            // Create labels.
            Label lblUserId = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = true, Visible = true,
                Text = "*User Id:"
            };

            Label lblPassword = new Label()
            {
                X = 3, Y = 4, Width = 10, Height = 1, CanFocus = true, Visible = true,
                Text = "*Password:"
            };
            
            // Create text inputs.
            _txtUserId = new TextField()
            {
                X = 15, Y = 2, Height = 1, Width = 20, CanFocus = true, Visible = true
            };

            _txtPassword = new TextField()
            {
                X = 15, Y = 4, Width = 20, Height = 1, CanFocus = true, Visible = true, Secret = true
            };
            
            // Add controls to view.
            _dialog.Add(lblUserId, lblPassword, _txtUserId, _txtPassword);

            // Set default control.
            _txtUserId.SetFocus();
        }
        
        public string UserId
        {
            get { return _userId; }
        }

        public string Password
        {
            get { return _password; }
        }
    }
}