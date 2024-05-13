using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public sealed class SplashDialog : BaseDialog
    {
        // Controls.
        private Label? _message;
        private readonly string? _messageText;
        
        public SplashDialog(string? message)
        {
            // Initialize members.
            _messageText = message;
            
            InitializeComponent();
        }

        public new void Show()
        {
            Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(3000), Hide);
            Application.Run(_dialog);
        }

        private bool Hide(MainLoop arg)
        {
            Application.RequestStop(_dialog);
            return false;
        }

        protected override void InitializeComponent()
        {
            // Create modal view.
            _dialog = new Dialog("", 85, 12);
            
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