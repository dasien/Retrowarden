using Terminal.Gui;
using RetrowardenSDK.Models;

namespace Retrowarden.Views 
{
    public partial class SecureNoteDetailView : ItemDetailView
    {
        // This sizes the underlying view appropriately.
        private const int scrollBottom = 25;

        public SecureNoteDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state, scrollBottom)
        {
            InitializeComponent();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private new void SetupView()
        {
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View || _viewState == VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }
            
            // Create an empty view so that the base view can resize.
            View empty = new View(new Rect(1, 3, 1, 1));

            // Set to base.
            DetailView = empty;
            
            // Setup common view parts.
            base.SetupView();
            
            // Set focus to first field.
            SetItemNameControlFocus();
        }
        
        #region Event Handlers

        protected override void SaveButtonClicked()
        {
            // Check to see that an item name is present (it is required).
            if (ItemName.Text == null)
            {
                MessageBox.ErrorQuery("Action failed.", "Item name must have a value.", "Ok");
            }

            else
            {
                // Check to see if the sub object is null (create mode).
                _item.SecureNote ??= new SecureNote();
                
                // Set the note type (currently always 0).
                _item.SecureNote.Type = 0;

                // Update item to current control values.
                UpdateItem();

                // Indicate Save was pressed.
                OkPressed = true;

                // Close dialog.
                Application.RequestStop();
            }
        }
        #endregion
    }
}
