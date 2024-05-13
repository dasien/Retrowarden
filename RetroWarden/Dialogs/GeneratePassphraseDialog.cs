using System.Text.RegularExpressions;
using Terminal.Gui;
using Retrowarden.Repositories;
using Retrowarden.Workers;

namespace Retrowarden.Dialogs 
{ 
    public sealed class GeneratePassphraseDialog : BaseDialog
    {
        private Label? _lblPassphrase;
        private FrameView? _fraOptions;
        private CheckBox? _chkCapitalize;
        private CheckBox? _chkIncludeNumbers;
        private Label? _lblNumOfWords;
        private TextField? _txtNumOfWords;
        private Label? _lblSeparator;
        private TextField? _txtSeparator;
        private Button? _btnGeneratePassphrase;
        private Button? _btnCopy;
        private Button? _btnClose;
        
        // Proxy reference.
        private readonly VaultRepository _repository;
        private string? _passphrase;
        
        public GeneratePassphraseDialog(VaultRepository repository) 
        {
            // Initialize members.
            _repository = repository;
            _passphrase = "";
            
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            bool retVal = true;

            // Numeric 1-5 allowed for number of words.
            string numberPattern = "^[1-5]+$";
            
            // Check to make sure we have valid controls.
            if (_txtNumOfWords != null && _txtSeparator != null)
            {
                // Check to see if there is a valid value for number of words.
                if (!Regex.IsMatch(_txtNumOfWords.Text.ToString() ?? string.Empty, numberPattern))
                {
                    MessageBox.ErrorQuery("Values Missing", "Enter a number of words (1-5).", "Ok");
                    retVal = false;
                }

                // Check to see if there is a valid separator.
                else if (!Regex.IsMatch(_txtSeparator.Text.ToString() ?? string.Empty, "^[!@#$%^&*-=+|_]{1}$"))
                {
                    MessageBox.ErrorQuery("Values Missing", "Enter a valid separator (!@#$%^&*-=+|_).", "Ok");
                    retVal = false;
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
                bool includeNumbers = _chkIncludeNumbers != null && _chkIncludeNumbers.Checked;
                bool capitalize = _chkCapitalize != null && _chkCapitalize.Checked;
                int words = _txtNumOfWords != null ? Convert.ToInt32(_txtNumOfWords.Text == null ? 1 :_txtNumOfWords.Text) : 1;
                string? sep = _txtSeparator != null ? _txtSeparator.Text.ToString() : "-";
                
                // Show working dialog.
                GeneratePassphraseWorker worker =
                    new GeneratePassphraseWorker(_repository, capitalize, includeNumbers, words, sep, "Generating Passphrase...");
                
                // Generate the passphrase.
                worker.Run();
                
                // Check to see if we succeeded.
                if (_repository.ExitCode == "0")
                {
                    // Check to see tha the label control is initialized.
                    if (_lblPassphrase != null)
                    {
                        // Show the new passphrase.
                        _lblPassphrase.Text = worker.Passphrase;
                    }

                    // Also store it.
                    _passphrase = worker.Passphrase;
                }
            }
            
        }

        private void CopyButton_Clicked()
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(_passphrase);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Passphrase copied to clipboard.", "Ok");
        }

        #region Initialize Component
        protected override void InitializeComponent() 
        {
            _btnClose = new Button();
            _btnCopy = new Button();
            _btnGeneratePassphrase = new Button();
            _txtSeparator = new TextField();
            _lblSeparator = new Label();
            _txtNumOfWords = new TextField();
            _lblNumOfWords = new Label();
            _chkIncludeNumbers = new CheckBox();
            _chkCapitalize = new CheckBox();
            _fraOptions = new FrameView();
            _lblPassphrase = new Label();

            _dialog = new Dialog()
            {
                Width = 44, Height = 16, X = Pos.Center(), Y = Pos.Center(), Visible = true, Modal = true,
                IsMdiContainer = false, TextAlignment = TextAlignment.Left, Title = "Passphrase Generator"
            };
            
            _dialog.Border.BorderStyle = BorderStyle.Single;
            _dialog.Border.Effect3D = true;
            _dialog.Border.Effect3DBrush = null;
            _dialog.Border.DrawMarginFrame = true;

            _lblPassphrase.Width = 39;
            _lblPassphrase.Height = 1;
            _lblPassphrase.X = 2;
            _lblPassphrase.Y = 1;
            _lblPassphrase.Visible = true;
            _lblPassphrase.Data = "_lblPassphrase";
            _lblPassphrase.Text = "No Passphrase Generated";
            _lblPassphrase.TextAlignment = TextAlignment.Centered;
            _dialog.Add(_lblPassphrase);
            
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
            
            _chkCapitalize.Width = 6;
            _chkCapitalize.Height = 1;
            _chkCapitalize.X = 1;
            _chkCapitalize.Y = 0;
            _chkCapitalize.Visible = true;
            _chkCapitalize.Data = "_chkCapitalize";
            _chkCapitalize.Text = "Capitalize First Letters";
            _chkCapitalize.TextAlignment = TextAlignment.Left;
            _chkCapitalize.Checked = true;
            _fraOptions.Add(_chkCapitalize);
            
            _chkIncludeNumbers.Width = 6;
            _chkIncludeNumbers.Height = 1;
            _chkIncludeNumbers.X = 1;
            _chkIncludeNumbers.Y = 1;
            _chkIncludeNumbers.Visible = true;
            _chkIncludeNumbers.Data = "_chkIncludeNumbers";
            _chkIncludeNumbers.Text = "Include Numbers";
            _chkIncludeNumbers.TextAlignment = TextAlignment.Left;
            _chkIncludeNumbers.Checked = true;
            _fraOptions.Add(_chkIncludeNumbers);
            
            _lblNumOfWords.Width = 4;
            _lblNumOfWords.Height = 1;
            _lblNumOfWords.X = 1;
            _lblNumOfWords.Y = 2;
            _lblNumOfWords.Visible = true;
            _lblNumOfWords.Data = "_lblNumOfWords";
            _lblNumOfWords.Text = "Number of Words";
            _lblNumOfWords.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_lblNumOfWords);
            
            _txtNumOfWords.Width = 4;
            _txtNumOfWords.Height = 1;
            _txtNumOfWords.X = 17;
            _txtNumOfWords.Y = 2;
            _txtNumOfWords.Visible = true;
            _txtNumOfWords.Secret = false;
            _txtNumOfWords.Data = "_txtNumOfWords";
            _txtNumOfWords.Text = "3";
            _txtNumOfWords.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_txtNumOfWords);
            
            _lblSeparator.Width = 4;
            _lblSeparator.Height = 1;
            _lblSeparator.X = 1;
            _lblSeparator.Y = 3;
            _lblSeparator.Visible = true;
            _lblSeparator.Data = "_lblSeparator";
            _lblSeparator.Text = "Word Separator";
            _lblSeparator.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_lblSeparator);
            
            _txtSeparator.Width = 4;
            _txtSeparator.Height = 1;
            _txtSeparator.X = 17;
            _txtSeparator.Y = 3;
            _txtSeparator.Visible = true;
            _txtSeparator.Secret = false;
            _txtSeparator.Data = "_txtSeparator";
            _txtSeparator.Text = "-";
            _txtSeparator.TextAlignment = TextAlignment.Left;
            _fraOptions.Add(_txtSeparator);
            
            _btnGeneratePassphrase.Width = 8;
            _btnGeneratePassphrase.Height = 1;
            _btnGeneratePassphrase.X = 1;
            _btnGeneratePassphrase.Y = 11;
            _btnGeneratePassphrase.Visible = true;
            _btnGeneratePassphrase.Data = "_btnGeneratePassphrase";
            _btnGeneratePassphrase.Text = "Generate";
            _btnGeneratePassphrase.TextAlignment = TextAlignment.Centered;
            _btnGeneratePassphrase.IsDefault = false;
            _btnGeneratePassphrase.Clicked += GenerateButton_Clicked;
            _dialog.Add(_btnGeneratePassphrase);
            
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

        public string? Passphrase
        {
            get { return _passphrase; }
        }
    }
}
