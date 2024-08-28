using System.ComponentModel;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{

    public sealed class SelectItemTypeDialog : BaseDialog
    {
        private RadioGroup? _rdoItemType;
        private int _itemType = 0;
        
        public SelectItemTypeDialog()
        {
            InitializeComponent();
        }
        
        protected override void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set flag for ok button and values.
            _okPressed = true;
            _itemType = _rdoItemType == null ? 1 :_rdoItemType.SelectedItem;
            
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

            string[] types = ["Login", "Secure Note", "Card", "Identity"];
        
            // Create radio group.
            _rdoItemType = new RadioGroup()
            {
                X = 8, Y = 1, RadioLabels = types
            };
                
            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Item Type", Width = 30, Height = 9, Buttons = [okButton, cancelButton]
            };
            _dialog.Add(_rdoItemType);
        }
        
        public int ItemType
        {
            get { return _itemType; }
        }
    }
}