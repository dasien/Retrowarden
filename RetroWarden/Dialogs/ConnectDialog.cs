using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

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
        
        public ConnectDialog(IApplication app) : base(app)
        {
            _userId = "";
            _password = "";
            _okPressed = false;

            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Check to see if required values are present.
            if (_txtPassword != null && _txtUserId != null
               && (_txtUserId.Text.Trim().Length == 0 || _txtPassword.Text.Trim().Length == 0))
            {
                MessageBox.ErrorQuery(_app,"Values Missing", "Both User Id and Password are required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;

                if (_txtUserId != null)
                {
                    _userId = _txtUserId.Text ?? string.Empty;
                }

                if (_txtPassword != null)
                {
                    _password = _txtPassword.Text ?? string.Empty;
                }

                // Close dialog.
                _app.RequestStop(_dialog);
            }

            e.Handled = true;
        }

        private void InitializeComponent()
        {
            // Create modal view with dynamic sizing.
            _dialog = new Dialog()
            {
                Title = "Connect to Vault",
                Width = 45,
                Height = 12,
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
            };

            // Create labels with dynamic width based on content.
            Label lblUserId = new Label()
            {
                X = 2,
                Y = 1,
                Width = Dim.Auto(),
                Text = "*User Id:"
            };

            Label lblPassword = new Label()
            {
                X = Pos.Left(lblUserId),
                Y = Pos.Bottom(lblUserId) + 1,
                Width = Dim.Auto(),
                Text = "*Password:"
            };

            // Create text inputs positioned relative to labels.
            _txtUserId = new TextField()
            {
                X = Pos.Right(lblUserId) + 2,
                Y = Pos.Top(lblUserId),
                Width = Dim.Fill(2)
            };

            _txtPassword = new TextField()
            {
                X = Pos.Left(_txtUserId),
                Y = Pos.Top(lblPassword),
                Width = Dim.Width(_txtUserId),
                Secret = true
            };

            // Create buttons centered at bottom with dynamic positioning.
            Button okButton = new Button()
            {
                Text = "_Connect",
            };
            okButton.Accepting += OkButton_Clicked;

            Button cancelButton = new Button()
            {
                Text = "Cance_l"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            _dialog.Add(lblUserId, lblPassword, _txtUserId, _txtPassword);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);

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