using Terminal.Gui;
using RetrowardenSDK.Models;

namespace Retrowarden.Dialogs
{
    public sealed class StatusDialog : BaseDialog
    {
        // Member variables.
        private Label? _lblStatus;
        private Label? _lblStatusValue;
        private Label? _lblUserEmail;
        private Label? _lblUserEmailValue;
        private Label? _lblLastSync;
        private Label? _lblLastSyncValue;
        private Label? _lblServerUrl;
        private Label? _lblServerUrlValue;
        private Button? _btnCancel;
        private VaultStatus _status;
        
        public StatusDialog(VaultStatus status)
        {
            // Initialize members.
            _status = status;
            
            // Initialize form.
            InitializeComponent();
            
            // Set data to value labels.
            if (_lblStatusValue != null)
            {
                _lblStatusValue.Text = status.FormattedStatus();
            }

            if (_lblLastSyncValue != null)
            {
                _lblLastSyncValue.Text = status.LastSync == null ? "Not Available" : status.LastSync.ToString();
            }

            if (_lblServerUrlValue != null)
            {
                _lblServerUrlValue.Text = status.ServerUrl == null ? "Not Available" : status.ServerUrl;
            }

            if (_lblUserEmailValue != null)
            {
                _lblUserEmailValue.Text = status.UserEmail == null ? "Not Available" : status.UserEmail;
            }
        }
        
        private void InitializeComponent()
        { 
            // Create dialog.
            _dialog = new Dialog()
            {
                Width = 40, Height = 12, X = Pos.Center(), Y = Pos.Center(), Visible = true, Modal = true, 
                TextAlignment = Alignment.Start
            };
            
           _lblStatus = new Label()
            {
                Width = 4, Height = 1, X = 2, Y = 1, Visible = true, Data = "lblStatus",
                Text = "Vault Status", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(this._lblStatus);

            _lblStatusValue = new Label()
            {
                Width = 4, Height = 1, X = 16, Y = 1, Visible = true, Data = "lblStatusValue", 
                TextAlignment = Alignment.Start, CanFocus = false
            };

            _dialog.Add(_lblStatusValue);

            _lblUserEmail = new Label()
            {
                Width = 4, Height = 1, X = 2, Y = 3, Visible = true, Data = "lblUserEmail", 
                Text = "Logged In As", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(_lblUserEmail);

            _lblUserEmailValue = new Label()
            {
                Width = 4, Height = 1, X = 16, Y = 3, Visible = true, Data = "lblUserEmailValue", 
                Text = "Heya", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(_lblUserEmailValue);

            _lblLastSync = new Label()
            {
                Width = 4, Height = 1, X = 2, Y = 5, Visible = true, Data = "lblLastSync", 
                Text = "Last Sync On", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(_lblLastSync);

            _lblLastSyncValue = new Label()
            {
                Width = 4, Height = 1, X = 16, Y = 5, Visible = true, Data = "lblLastSyncValue", 
                Text = "Heya", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(_lblLastSyncValue);

            _lblServerUrl = new Label()
            {
                Width = 4, Height = 1, X = 4, Y = 7, Visible = true, Data = "lblServerUrl", 
                Text = "Server Url", TextAlignment = Alignment.Start, CanFocus = false
            };
            
            _dialog.Add(_lblServerUrl);

            _lblServerUrlValue = new Label()
            {
                Width = 4, Height = 1, X = 16, Y = 7, Visible = true, Data = "lblServerUrlValue", 
                Text = "Heya", TextAlignment = Alignment.Start, CanFocus = false
            };
          
            _dialog.Add(_lblServerUrlValue);

            _btnCancel = new Button()
            {
                Width = 9, Height = 1, X = 14, Y = 9, Visible = true, Data = "btnCancel", 
                Text = "Close", TextAlignment = Alignment.Center, IsDefault = true
            };

            _btnCancel.Accepting += CancelButton_Clicked;
            _dialog.Add(_btnCancel);
        }
    }    
}

