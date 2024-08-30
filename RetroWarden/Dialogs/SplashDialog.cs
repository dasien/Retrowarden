using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class SplashDialog : BaseDialog
    {
        // Controls.
        private Label? _message;
        private readonly string? _messageText;
        private object? _timerToken;
        
        public SplashDialog(string? message)
        {
            // Initialize members.
            _messageText = message;
            
            InitializeComponent();
        }

        public new void Show()
        {
            _timerToken = Application.AddTimeout (TimeSpan.FromMilliseconds(3000), Hide);
            Application.Run(_dialog);
        }

        private bool Hide()
        {
            Application.RemoveTimeout(_timerToken);
            Application.RequestStop(_dialog);
            _dialog.Dispose();
            return false;
        }

        private void InitializeComponent()
        {
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "", Width = 85, Height = 12
            };
            
            // Create labels.
            _message = new Label()
            {
                X = 0, Y = 0, Width = 85, Height = 9, CanFocus = false, Visible = true,
                Text = _messageText, Data = "lblMessage"
            };
            
            // Add controls to view.
            _dialog.Add(_message);
        }
    }
}