using System.ComponentModel;
using Retrowarden.Workers;
using RetrowardenSDK.Repositories;
using Terminal.Gui;

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
        
        public GeneratePasswordDialog(IVaultRepository repository) 
        {
            // Initialize members.
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
                    MessageBox.ErrorQuery("Values Missing", "Passwords length must be at least 12 characters.", "Ok");
                    retVal = false;
                }

                else
                {
                    if (!int.TryParse(_txtPasswordLength.Text, out int length))
                    {
                        MessageBox.ErrorQuery("Values Missing", "Passwords length must be a numeric value.", "Ok");
                        retVal = false;
                    }

                    else
                    {
                        if (length < 12)
                        {
                            MessageBox.ErrorQuery("Value Error", "Passwords length must be at least 12.", "Ok");
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
                bool includeNumbers = _chkNumbers != null && _chkNumbers.CheckedState == CheckState.Checked;
                bool upper = _chkUpperCase != null && _chkUpperCase.CheckedState == CheckState.Checked;
                bool lower = _chkLowerCase != null && _chkLowerCase.CheckedState == CheckState.Checked;
                bool special = _chkSpecialChars != null && _chkSpecialChars.CheckedState == CheckState.Checked;
                int length = _txtPasswordLength != null ? Convert.ToInt32(_txtPasswordLength.Text == null ? 1 :_txtPasswordLength.Text) : 1 ;
                
                // Show working dialog.
                GeneratePasswordWorker worker =
                    new GeneratePasswordWorker(_repository, upper, lower, 
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
            Clipboard.TrySetClipboardData(_password);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");
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
                Width = 44, Height = 16, X = Pos.Center(), Y = Pos.Center(), Visible = true, Modal = true, 
                TextAlignment = Alignment.Start, Title = "Password Generator"
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
                Width = 6, Height = 1, X = 1, Y = 0, Visible = true, Data = "_chkUpperCase",
                Text = "Uppercase Characters (A-Z)", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
            _fraOptions.Add(_chkUpperCase);

            _chkLowerCase = new CheckBox()
            {
                Width = 6, Height = 1, X = 1, Y = 1, Visible = true, Data = "_chkLowerCase",
                Text = "Lowercase Characters (a-z)", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
            _fraOptions.Add(_chkLowerCase);

            _chkNumbers = new CheckBox()
            {
                Width = 6, Height = 1, X = 1, Y = 2, Visible = true, Data = "_chkNumbers",
                Text = "Numeric Characters (0-9)", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
            _fraOptions.Add(_chkNumbers);

            _chkSpecialChars = new CheckBox()
            {
                Width = 6, Height = 1, X = 1, Y = 3, Visible = true, Data = "_chkSpecialChars",
                Text = "Special Characters (!@#$%^&*)", TextAlignment = Alignment.Start, CheckedState = CheckState.Checked
            };
            _fraOptions.Add(_chkSpecialChars);

            _lblLength = new Label()
            {
                Width = 4, Height = 1, X = 3, Y = 4, Visible = true, Data = "_lblLength",
                Text = "Length", CanFocus = false, TextAlignment = Alignment.Start
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
                Width = 8, Height = 1, X = 1, Y = 11, Visible = true, Data = "_btnGeneratePassphrase",
                Text = "Generate", TextAlignment = Alignment.Center, IsDefault = false
            };
            _btnGeneratePassword.Accept += GenerateButton_Clicked;
            _dialog.Add(_btnGeneratePassword);
            
            _btnCopy = new Button()
            {
                Width = 4, Height = 1, X = 18, Y = 11, Visible = true, Data = "_btnCopy",
                Text = "Copy", TextAlignment = Alignment.Center, IsDefault = false
            };
            _btnCopy.Accept += CopyButton_Clicked;
            _dialog.Add(_btnCopy);

            _btnClose = new Button()
            {
                Width = 9, Height = 1, X = 32, Y = 11, Visible = true, Data = "_btnClose",
                Text = "Close", TextAlignment = Alignment.Center, IsDefault = false
            };
            _btnClose.Accept += CancelButton_Clicked;
            _dialog.Add(_btnClose);
        }
        #endregion
        
        public string? Password
        {
            get { return _password; }
        }
    }
}
