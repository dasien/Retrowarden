using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class WorkingDialog : BaseDialog
    {
        // Controls.
        private Label? _lblAnimation;
        private Label? _lblMessage;
        private Label? _lblProgress;
        private int _animationIndex = 0;
        private object? _timerToken;
        private readonly string? _messageText;
        
        // Other values.
        private readonly string[] _spinner = [
            "01100010", "01101001", "01110100", "01110111", "01100001", "01110010", "01100100", "01100101",
            "01101110"
        ];
        
        public WorkingDialog(string? message)
        {
            // Initialize members.
            _messageText = message;
            
            InitializeComponent();
        }

        public new void Show()
        {
            _timerToken = Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(80), UpdateAnimationLabel);
            Application.Run(_dialog);
        }

        public void Hide()
        {
            Application.MainLoop.RemoveTimeout (_timerToken);
            Application.RequestStop(_dialog);
        }
        
        private bool UpdateAnimationLabel(MainLoop arg)
        {
            // Update text.
            _dialog.Subviews[0].Subviews[0].Text = _spinner[_animationIndex];
            _dialog.Subviews[0].Subviews[0].SetNeedsDisplay();
            
            if (_animationIndex < (_spinner.Length -1))
            {
                _animationIndex++;
            }

            else
            {
                _animationIndex = 0;
            }
            
            // Return true so the timer keeps running.
            return true;
        }

        protected override void InitializeComponent()
        {
            // Create modal view.
            _dialog = new Dialog("Working...", 40, 6);

            // Create labels.
            _lblAnimation = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true,
                Text = "", Data = "lblAnimation"
            };

            _lblMessage = new Label()
            {
                X = 13, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true,
                Text = _messageText, Data = "lblMessage"
            };

            _lblProgress = new Label()
            {
                X = 13, Y = 3, Width = 10, Height = 1, CanFocus = false, Visible = true,
                Text = "", Data = "lblProgress"
            };
            
            // Add controls to view.
            _dialog.Add(_lblAnimation, _lblMessage, _lblProgress);
        }
        
        public string ProgressMessage
        {
            set
            {
                // Check to see if the control has been created.
                if (_lblProgress != null)
                {
                    _lblProgress.Text = value;
                }
            }
        }
        public bool IsCurrentTop
        {
            get
            {
                return _dialog.IsCurrentTop;
            }
        }
    }
}