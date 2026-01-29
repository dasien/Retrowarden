using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using RetrowardenSDK.Repositories;
using Retrowarden.Workers;

namespace Retrowarden.Dialogs 
{
    public sealed class GeneratePasswordDialog : BaseDialog 
    {
        private Label? _lblPassword;
        private FrameView? _fraOptions;
        private CheckBox? _chkUpperCase;
        private CheckBox? _chkLowerCase;
        private CheckBox? _chkNumbers;
        private CheckBox? _chkSpecialChars;
        private Label? _lblLength;
        private TextField? _txtPasswordLength;
        private Button? _btnGeneratePassword;
        private Button? _btnCopy;
        private Button? _btnClose;

         // Proxy reference.
        private readonly IVaultRepository _repository;
        private string? _password;
        
        public GeneratePasswordDialog(IApplication app, IVaultRepository repository) : base(app)
        {
            _repository = repository;
            _password = "";

            InitializeComponent();
        }

        private bool ValidateInput()
        {
            bool retVal = true;
            
            // Check to see if password length control is initialized.
            if (_txtPasswordLength != null)
            {
                // Check to see if there is a valid value for number of words.
                if (_txtPasswordLength.Text.Length == 0)
                {
                    MessageBox.ErrorQuery(_app,"Values Missing", "Passwords length must be at least 12 characters.", "Ok");
                    retVal = false;
                }

                else
                {
                    if (!int.TryParse(_txtPasswordLength.Text, out int length))
                    {
                        MessageBox.ErrorQuery(_app,"Values Missing", "Passwords length must be a numeric value.", "Ok");
                        retVal = false;
                    }

                    else
                    {
                        if (length < 12)
                        {
                            MessageBox.ErrorQuery(_app,"Value Error", "Passwords length must be at least 12.", "Ok");
                            retVal = false;
                        }
                    }
                }
            }

            else
            {
                retVal = false;
            }

            // Return value.
            return retVal;
        }
        
        private void GenerateButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Validate input.
            if (ValidateInput())
            {
                // Set values.
                bool includeNumbers = _chkNumbers != null && _chkNumbers.Value == CheckState.Checked;
                bool upper = _chkUpperCase != null && _chkUpperCase.Value == CheckState.Checked;
                bool lower = _chkLowerCase != null && _chkLowerCase.Value == CheckState.Checked;
                bool special = _chkSpecialChars != null && _chkSpecialChars.Value == CheckState.Checked;
                int length = _txtPasswordLength != null ? Convert.ToInt32(_txtPasswordLength.Text == null ? 1 :_txtPasswordLength.Text) : 1 ;
                
                // Show working dialog.
                GeneratePasswordWorker worker =
                    new GeneratePasswordWorker(_app, _repository, upper, lower,
                        includeNumbers, special, length, "Generating Password...");
                
                // Generate the passphrase.
                worker.Run();
                
                // Check to see if we succeeded.
                if (_repository.ExitCode == "0")
                {
                    // Check to see tha the label control is initialized.
                    if (_lblPassword != null)
                    {
                        // Show the new passphrase.
                        _lblPassword.Text = worker.Password;
                    }
                    
                    // Also store it.
                    _password = worker.Password;
                }
            }
        }

        private void CopyButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Copy password to clipboard.
            Clipboard.Contents = _password;

            // Indicate data copied.
            MessageBox.Query(_app,"Action Completed", "Password copied to clipboard.", "Ok");
        }
    
        #region InitializeComponent
        private void InitializeComponent() 
        {
            _txtPasswordLength = new TextField();
            _lblLength = new Label();
            _chkSpecialChars = new CheckBox();
            _chkNumbers = new CheckBox();
            _chkLowerCase = new CheckBox();
            _chkUpperCase = new CheckBox();
            _fraOptions = new FrameView();
            _lblPassword = new Label();
            
            _dialog = new Dialog()
            {
                Width = 44, Height = 16, X = Pos.Center(), Y = Pos.Center(), Visible = true,
                TextAlignment = Alignment.Start, Title = "Password Generator",
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
            };

            _lblPassword = new Label()
            {
                Width = 39, Height = 1, X = 2, Y = 1, Visible = true, Data = "_lblPassword",
                Text = "No password generated.", CanFocus = false, TextAlignment = Alignment.Center
            };
            _dialog.Add(_lblPassword);

            _fraOptions = new FrameView()
            {
                Width = 40, Height = 8, X = 1, Y = 3, Visible = true, Data = "_fraOptions",
                TextAlignment = Alignment.Start, Title = "Options"
            };
            _dialog.Add(_fraOptions);

            _chkUpperCase = new CheckBox()
            {
                X = 1, Y = 0,
                Width = Dim.Auto(),
                Text = "Uppercase Characters (A-Z)",
                Value = CheckState.Checked
            };
            _fraOptions.Add(_chkUpperCase);

            _chkLowerCase = new CheckBox()
            {
                X = 1, Y = 1,
                Width = Dim.Auto(),
                Text = "Lowercase Characters (a-z)",
                Value = CheckState.Checked
            };
            _fraOptions.Add(_chkLowerCase);

            _chkNumbers = new CheckBox()
            {
                X = 1, Y = 2,
                Width = Dim.Auto(),
                Text = "Numeric Characters (0-9)",
                Value = CheckState.Checked
            };
            _fraOptions.Add(_chkNumbers);

            _chkSpecialChars = new CheckBox()
            {
                X = 1, Y = 3,
                Width = Dim.Auto(),
                Text = "Special Characters (!@#$%^&*)",
                Value = CheckState.Checked
            };
            _fraOptions.Add(_chkSpecialChars);

            _lblLength = new Label()
            {
                X = 3, Y = 4,
                Width = Dim.Auto(),
                Text = "Length"
            };
            _fraOptions.Add(_lblLength);

            _txtPasswordLength = new TextField()
            {
                Width = 10, Height = 1, X = 10, Y = 4, Visible = true, Secret = false,
                Data = "_txtPasswordLength", Text = "14", TextAlignment = Alignment.Start
            };
            _fraOptions.Add(_txtPasswordLength);
            
            _btnGeneratePassword = new Button()
            {
                Text = "_Generate",
            };
            _btnGeneratePassword.Accepting += GenerateButton_Clicked;
            _dialog.AddButton(_btnGeneratePassword);

            _btnCopy = new Button()
            {
                Text = "C_opy"
            };
            _btnCopy.Accepting += CopyButton_Clicked;
            _dialog.AddButton(_btnCopy);

            _btnClose = new Button()
            {
                Text = "C_lose"
            };
            _btnClose.Accepting += CancelButton_Clicked;
            _dialog.AddButton(_btnClose);
        }
        #endregion
        
        public string? Password
        {
            get { return _password; }
        }
    }
}
