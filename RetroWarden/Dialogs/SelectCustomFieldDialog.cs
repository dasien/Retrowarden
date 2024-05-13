using NStack;
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
        
        protected override void OkButton_Clicked()
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
            Button okButton = new Button(5, 6, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(14, 6, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            ustring[] types = new ustring[] { "Text", "Hidden", "Boolean", "Linked" };
        
            // Create radio group.
            _rdoFieldType = new RadioGroup(8,1, types);
                
            // Create modal view.
            _dialog = new Dialog("Select Item Type", 30, 9, okButton, cancelButton);
            _dialog.Add(_rdoFieldType);
        }
        
        public int FieldType
        {
            get { return _fieldType; }
        }
    }
}