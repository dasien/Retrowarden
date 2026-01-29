using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace Retrowarden.Dialogs
{
    public sealed class UnlockDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtPassword;

        // Other values.
        private string _password;
        
        public UnlockDialog(IApplication app) : base(app)
        {
            _password = "";
            _okPressed = false;

            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Check to see if required values are present.
            if (_txtPassword != null &&  _txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.ErrorQuery(_app,"Value Missing", "Password is required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtPassword != null)
                {
                    _password = _txtPassword.Text ?? string.Empty;
                }

                // Close dialog.
                _app.RequestStop(_dialog);
            }
        }

        private void InitializeComponent()
        {
            // Create modal view with dynamic sizing.
            _dialog = new Dialog()
            {
                Title = "Unlock Vault",
                Width = 45,
                Height = 10,
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
            };

            // Create label with dynamic width based on content.
            Label lblPassword = new Label()
            {
                X = 2,
                Y = 1,
                Width = Dim.Auto(),
                Text = "*Password:"
            };

            // Create text input positioned relative to label.
            _txtPassword = new TextField()
            {
                X = Pos.Right(lblPassword) + 1,
                Y = Pos.Top(lblPassword),
                Width = Dim.Fill(2),
                Secret = true
            };

            // Create buttons.
            Button okButton = new Button()
            {
                Text = "_Unlock",
            };
            okButton.Accepting += OkButton_Clicked;

            Button cancelButton = new Button()
            {
                Text = "Cance_l"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Add controls to view.
            _dialog.Add(lblPassword, _txtPassword);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);

            // Set default control.
            _txtPassword.SetFocus();
        }
        
        public string Password
        {
            get { return _password; }
        }
    }
}