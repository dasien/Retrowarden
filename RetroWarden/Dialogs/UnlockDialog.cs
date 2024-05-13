using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class UnlockDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtPassword;

        // Other values.
        private string _password;
        
        public UnlockDialog() 
        {
            _password = ""; 
            _okPressed = false;

            InitializeComponent();
        }

        protected override void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_txtPassword != null &&  _txtPassword.Text.TrimSpace().Length == 0)
            {
                MessageBox.ErrorQuery("Value Missing", "Password is required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
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
            Button okButton = new Button(8, 4, "_Unlock");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(24, 4, "Cance_l");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Unlock Vault", 40, 8, okButton, cancelButton);

            // Create labels.
            Label lblPassword = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true, Text = "*Password:"
            };
            
            // Create text inputs.
            _txtPassword = new TextField()
            {
                X = 15, Y = 2, Width = 20, Height = 1, CanFocus = true, Visible = true, Secret = true
            };

            // Add controls to view.
            _dialog.Add(lblPassword, _txtPassword);

            // Set default control.
            _txtPassword.SetFocus();
        }
        
        public string Password
        {
            get { return _password; }
        }
    }
}