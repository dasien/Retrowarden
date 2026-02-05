using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace Retrowarden.Dialogs
{

    public sealed class SelectCustomFieldDialog : BaseDialog
    {
        private OptionSelector? _rdoFieldType;
        private int? _fieldType = 0;
        
        public SelectCustomFieldDialog(IApplication app) : base(app)
        {
            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;

            if (_rdoFieldType != null)
            {
                _fieldType = _rdoFieldType.Value;
            }

            _app.RequestStop(_dialog);
            e.Handled = true;
        }
        
        private void InitializeComponent()
        {
            // Create modal view with dynamic sizing.
            _dialog = new Dialog()
            {
                Title = "Select Item Type",
                Width = 30,
                Height = 11,
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
            };

            string[] types = new string[] { "Text", "Hidden", "Boolean", "Linked" };

            // Create option selector with dynamic positioning.
            _rdoFieldType = new OptionSelector()
            {
                X = Pos.Center(),
                Y = 1,
                Labels = types
            };

            // Create buttons.
            Button okButton = new Button()
            {
                Text = "Ok",
            };
            okButton.Accepting += OkButton_Clicked;

            Button cancelButton = new Button()
            {
                Text = "Cancel"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            _dialog.Add(_rdoFieldType);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);
        }
        
        public int? FieldType
        {
            get { return _fieldType; }
        }
    }
}