using NStack;
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
        
        protected override void OkButton_Clicked()
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
            Button okButton = new Button(5, 6, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(14, 6, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            ustring[] types = ["Login", "Secure Note", "Card", "Identity"];
        
            // Create radio group.
            _rdoItemType = new RadioGroup(8,1, types);
                
            // Create modal view.
            _dialog = new Dialog("Select Item Type", 30, 9, okButton, cancelButton);
            _dialog.Add(_rdoItemType);
        }
        
        public int ItemType
        {
            get { return _itemType; }
        }
    }
}