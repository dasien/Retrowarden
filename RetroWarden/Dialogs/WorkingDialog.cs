using Terminal.Gui.App;
using Terminal.Gui.Views;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Drawing;

namespace Retrowarden.Dialogs
{
    public sealed class WorkingDialog : BaseDialog
    {
        // Controls.
        private SpinnerView? _spnAnimation;
        private Label? _lblMessage;
        private Label? _lblProgress;
        private readonly string _messageText;
        
        // Other values.
        private readonly string[] _spinFrames = [
            "01100010", "01101001", "01110100", "01110111", "01100001", "01110010", "01100100", "01100101",
            "01101110"
        ];
        
        public WorkingDialog(IApplication app, string message) : base(app)
        {
            _messageText = message;

            InitializeComponent();
        }

        public new void Show()
        {
            if (_dialog != null)
            {
                _app.Run(_dialog);
            }
        }

        public void Hide()
        {
            _app.RequestStop(_dialog);
            _dialog?.Dispose();
        }

        private void InitializeComponent()
        {
            // Create modal view with dynamic sizing.
            _dialog = new Dialog()
            {
                Width = 45,
                Height = 7,
                BorderStyle = LineStyle.Single
            };

            // Set title after creation
            _dialog.Title = "Working...";

            // Create controls with dynamic positioning.
            _spnAnimation = new SpinnerView()
            {
                X = 2,
                Y = 1,
                Width = 10,
                Height = 1,
                CanFocus = false,
                Visible = true,
                AutoSpin = true,
                SpinDelay = 80,
                Sequence = _spinFrames,
                Data = "spnAnimation"
            };

            _lblMessage = new Label()
            {
                X = Pos.Right(_spnAnimation) + 1,
                Y = Pos.Top(_spnAnimation),
                Width = Dim.Fill(2),
                Height = 1,
                CanFocus = false,
                Visible = true,
                Text = _messageText,
                Data = "lblMessage"
            };

            _lblProgress = new Label()
            {
                X = Pos.Left(_lblMessage),
                Y = Pos.Bottom(_lblMessage),
                Width = Dim.Fill(2),
                Height = 1,
                CanFocus = false,
                Visible = true,
                Text = "",
                Data = "lblProgress"
            };

            // Add controls to view.
            _dialog.Add(_spnAnimation, _lblMessage, _lblProgress);
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
                return _dialog != null && _dialog.IsCurrentTop;
            }
        }
    }
}