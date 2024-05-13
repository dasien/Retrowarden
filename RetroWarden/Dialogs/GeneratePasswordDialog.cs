using Terminal.Gui;
using Retrowarden.Repositories;
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
        private readonly VaultRepository _repository;
        private string? _password;
        
        public GeneratePasswordDialog(VaultRepository repository) 
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
                    if (!int.TryParse(_txtPasswordLength.Text.ToString(), out int length))
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
        
        private void GenerateButton_Clicked()
        {
            // Validate input.
            if (ValidateInput())
            {
                // Set values.
                bool includeNumbers = _chkNumbers != null && _chkNumbers.Checked;
                bool upper = _chkUpperCase != null && _chkUpperCase.Checked;
                bool lower = _chkLowerCase != null && _chkLowerCase.Checked;
                bool special = _chkSpecialChars != null && _chkSpecialChars.Checked;
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

        private void CopyButton_Clicked()
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(_password);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");
        }
    
        #region InitializeComponent
        protected override void InitializeComponent() 
        {
            _btnClose = new Button();
            _btnCopy = new Button();
            _btnGeneratePassword = new Button();
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
                IsMdiContainer = false, TextAlignment = TextAlignment.Left, Title = "Password Generator"
            };
            
            _dialog.Border.BorderStyle = BorderStyle.Single;
            _dialog.Border.Effect3D = true;
            _dialog.Border.Effect3DBrush = null;
            _dialog.Border.DrawMarginFrame = true;

            _lblPassword.Width = 39;
            _lblPassword.Height = 1;
            _lblPassword.X = 2;
            _lblPassword.Y = 1;
            _lblPassword.Visible = true;
            _lblPassword.Data = "_lblPassword";
            _lblPassword.Text = "No password generated.";
            _lblPassword.TextAlignment = TextAlignment.Centered;
            _dialog.Add(_lblPassword);
            
            _fraOptions.Width = 40;
            _fraOptions.Height = 8;
            _fraOptions.X = 1;
            _fraOptions.Y = 3;
            _fraOptions.Visible = true;
            _fraOptions.Data = "_fraOptions";
            _fraOptions.Border.BorderStyle = BorderStyle.Single;
            _fraOptions.Border.Effect3D = false;
            _fraOptions.Border.Effect3DBrush = null;
            _fraOptions.Border.DrawMarginFrame = true;
            _fraOptions.TextAlignment = TextAlignment.Left;
            _fraOptions.Title = "Options";
            _dialog.Add(_fraOptions);
            
            _chkUpperCase.Width = 6;
            _chkUpperCase.Height = 1;
            _chkUpperCase.X = 1;
            _chkUpperCase.Y = 0;
            _chkUpperCase.Visible = true;
            _chkUpperCase.Data = "_chkUpperCase";
            _chkUpperCase.Text = "Uppercase Characters (A-Z)";
            _chkUpperCase.TextAlignment = TextAlignment.Left;
            _chkUpperCase.Checked = true;
            _fraOptions.Add(_chkUpperCase);
            
            _chkLowerCase.Width = 6;
            _chkLowerCase.Height = 1;
            _chkLowerCase.X = 1;
            _chkLowerCase.Y = 1;
            _chkLowerCase.Visible = true;
            _chkLowerCase.Data = "_chkLowerCase";
            _chkLowerCase.Text = "Lowercase Characters (a-z)";
            _chkLowerCase.TextAlignment = TextAlignment.Left;
            _chkLowerCase.Checked = true;
            _fraOptions.Add(_chkLowerCase);
            
            _chkNumbers.Width = 6;
            _chkNumbers.Height = 1;
            _chkNumbers.X = 1;
            _chkNumbers.Y = 2;
            _chkNumbers.Visible = true;
            _chkNumbers.Data = "_chkNumbers";
            _chkNumbers.Text = "Numeric Characters (0-9)";
            _chkNumbers.TextAlignment = TextAlignment.Left;
            _chkNumbers.Checked = true;
            _fraOptions.Add(_chkNumbers);
            
            _chkSpecialChars.Width = 6;
            _chkSpecialChars.Height = 1;
            _chkSpecialChars.X = 1;
            _chkSpecialChars.Y = 3;
            _chkSpecialChars.Visible = true;
            _chkSpecialChars.Data = "_chkSpecialChars";
            _chkSpecialChars.Text = "Special Characters (!@#$%^&*)";
            _chkSpecialChars.TextAlignment = TextAlignment.Left;
            _chkSpecialChars.Checked = true;
            _fraOptions.Add(_chkSpecialChars);
            
            _lblLength.Width = 4;
            _lblLength.Height = 1;
            _lblLength.X = 3;
            _lblLength.Y = 4;
            _lblLength.Visible = true;
            _lblLength.Data = "_lblLength";
            _lblLength.Text = "Length";
            _lblLength.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_lblLength);
            
            _txtPasswordLength.Width = 10;
            _txtPasswordLength.Height = 1;
            _txtPasswordLength.X = 10;
            _txtPasswordLength.Y = 4;
            _txtPasswordLength.Visible = true;
            _txtPasswordLength.Secret = false;
            _txtPasswordLength.Data = "_txtPasswordLength";
            _txtPasswordLength.Text = "14";
            _txtPasswordLength.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_txtPasswordLength);
            
            _btnGeneratePassword.Width = 8;
            _btnGeneratePassword.Height = 1;
            _btnGeneratePassword.X = 1;
            _btnGeneratePassword.Y = 11;
            _btnGeneratePassword.Visible = true;
            _btnGeneratePassword.Data = "_btnGeneratePassword";
            _btnGeneratePassword.Text = "Generate";
            _btnGeneratePassword.TextAlignment = TextAlignment.Centered;
            _btnGeneratePassword.IsDefault = false;
            _btnGeneratePassword.Clicked += GenerateButton_Clicked;
            _dialog.Add(_btnGeneratePassword);
            
            _btnCopy.Width = 4;
            _btnCopy.Height = 1;
            _btnCopy.X = 18;
            _btnCopy.Y = 11;
            _btnCopy.Visible = true;
            _btnCopy.Data = "_btnCopy";
            _btnCopy.Text = "Copy";
            _btnCopy.TextAlignment = TextAlignment.Centered;
            _btnCopy.IsDefault = false;
            _btnCopy.Clicked += CopyButton_Clicked;
            _dialog.Add(_btnCopy);
            
            _btnClose.Width = 9;
            _btnClose.Height = 1;
            _btnClose.X = 32;
            _btnClose.Y = 11;
            _btnClose.Visible = true;
            _btnClose.Data = "_btnClose";
            _btnClose.Text = "Close";
            _btnClose.TextAlignment = TextAlignment.Centered;
            _btnClose.IsDefault = false;
            _btnClose.Clicked += CancelButton_Clicked;
            _dialog.Add(_btnClose);
        }
        #endregion
        
        public string? Password
        {
            get { return _password; }
        }
    }
}
