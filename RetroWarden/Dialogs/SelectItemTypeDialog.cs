using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace Retrowarden.Dialogs
{

    public sealed class SelectItemTypeDialog : BaseDialog
    {
        private OptionSelector? _rdoItemType;
        private int _itemType = 0;
        
        public SelectItemTypeDialog(IApplication app) : base(app)
        {
            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;
            _itemType = _rdoItemType?.Value ?? 1;

            // Close dialog.
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

            string[] types = ["Login", "Secure Note", "Card", "Identity"];

            // Create option selector with dynamic positioning.
            _rdoItemType = new OptionSelector()
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

            _dialog.Add(_rdoItemType);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);
        }
        
        public int ItemType
        {
            get { return _itemType; }
        }
    }
}