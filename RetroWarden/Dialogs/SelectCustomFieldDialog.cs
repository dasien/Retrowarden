using System.ComponentModel;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{

    public sealed class SelectCustomFieldDialog : BaseDialog
    {
        private RadioGroup? _rdoFieldType;
        private int _fieldType = 0;
        
        public SelectCustomFieldDialog()
        {
            InitializeComponent();
        }
        
        protected override void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;
            
            if (_rdoFieldType != null)
            {
                _fieldType = _rdoFieldType.SelectedItem;
            }

            // Close dialog.
            Application.RequestStop(_dialog);
        }
        
        protected override void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 5, Y = 6, Text = "Ok"
            };
            okButton.Accept += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 14, Y = 6, Text = "Cancel"
            };
            cancelButton.Accept += CancelButton_Clicked;

            string[] types = new string[] { "Text", "Hidden", "Boolean", "Linked" };
        
            // Create radio group.
            _rdoFieldType = new RadioGroup()
            {
                X = 8, Y = 1, RadioLabels = types
            };
                
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Item Type", Width = 30, Height = 9, Buttons = [okButton, cancelButton]
            };
            _dialog.Add(_rdoFieldType);
        }
        
        public int FieldType
        {
            get { return _fieldType; }
        }
    }
}