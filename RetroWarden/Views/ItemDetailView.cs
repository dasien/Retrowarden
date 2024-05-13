using Retrowarden.Controls;
using Retrowarden.Dialogs;
using Terminal.Gui;
using Retrowarden.Models;

namespace Retrowarden.Views 
{
    public abstract partial class ItemDetailView
    {
        private View _subView;
        protected VaultItem _item;
        private readonly List<VaultFolder> _folders;
        protected readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;
        
        protected ItemDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            
            InitializeComponent();
        }
        
        public void Show()
        {
            Application.Run(this);
        }

        protected void SetupView()
        {
            // Check to make sure we have a sub-view to add (Secure Note doesn't have one)
            if (_subView != null)
            {
                // Add subview to view.
                scrMain.Add(_subView);

                // Update location of 'footer' controls to below the particular detail view.
                RelocateFooterControls();
            }

            // Initialize any list controls.
            InitializeLists();
            
            // Load form based on action.
            switch (_viewState)
            {
                case VaultItemDetailViewState.Create:
                    
                    // Set title.
                    this.Title = "Create New Item";
                    break;

                case VaultItemDetailViewState.Edit:
                    
                    // Load controls with current data only.
                    LoadView();
                    
                    // Set title.
                    this.Title = "Edit Item - " + _item.ItemName;
                    break;

                case VaultItemDetailViewState.View:
                    
                    // Load controls with current data only.
                    LoadView();
                    
                    // Disable control state.
                    DisableView();
                    
                    // Set title.
                    this.Title = "View Item - " + _item.ItemName;
                    break;
            }
        }

        protected void LoadView()
        {
            // Set current item values to controls.
            txtItemName.Text = _item.ItemName;
            chkFavorite.Checked = _item.IsFavorite;
            tvwNotes.Text = _item.Notes ?? "";
            
            // Set the folder to the current folder (or "No Folder" as a null default.
            cboFolder.SelectedItem = _folders.FindIndex(o => o.Id == _item.FolderId);
            
            // Create new field scroll control.
            scrCustomFields = new CustomFieldScrollView(_item.CustomFields, _item.ItemType)
            {
                X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true,
                ContentSize = new Size(95, 10), Data = "scrURIList", TextAlignment = TextAlignment.Left
            };
            
            // Add scroll to view.
            fraCustomFieldList.Add(scrCustomFields);
        }

        protected void SetItemNameControlFocus()
        {
            // This is part of a bug fix that allows focus in the URI list frame.
            txtItemName.FocusFirst();
        }
        
        private void DisableView()
        {
            btnSave.Enabled = false;
        }
        
        private void InitializeLists()
        {
            // Set folder comboxbox source.
            cboFolder.SetSource(_folders);
        }

        private void RelocateFooterControls()
        {
            // Update Y of the notes frame to detail view bottom + 2
            fraNotes.Y = Pos.Bottom(_subView) + 2;
            
            // Update the custom fields frame.
            fraCustomFieldList.Y = Pos.Bottom(fraNotes) + 2;
            btnNewCustomField.Y = Pos.Bottom(fraCustomFieldList) + 1;
            
            // Update the save and cancel button locations relative to the new notes frame.
            btnSave.Y = Pos.Bottom(btnNewCustomField) + 2;
            btnCancel.Y = Pos.Bottom(btnNewCustomField) + 2;
            scrMain.SetNeedsDisplay();
        }
        
        protected virtual void UpdateItem()
        {
            // Set item properties.
            _item.ItemName = txtItemName.Text.ToString() ?? "";
            
            // Check to see if there is a selected item.
            if (cboFolder.SelectedItem == -1)
            {
                // Set to "No Folder"
                _item.FolderId = _folders.ElementAt(0).Id;
            }

            else
            {
                _item.FolderId = _folders.ElementAt(cboFolder.SelectedItem).Id;
            }
            
            _item.IsFavorite = chkFavorite.Checked;
            _item.Notes = tvwNotes.Text.ToString();
            
            // Get the custom fields.
            _item.CustomFields = scrCustomFields.Fields;
        }

        protected virtual void SetTabOrder()
        {
            // Set tab order for common controls.
            txtItemName.TabIndex = 0;
            cboFolder.TabIndex = 1;
            chkFavorite.TabIndex = 2;
            _subView.TabIndex = 3;
            tvwNotes.TabIndex = 96;
            scrCustomFields.TabIndex = 97;
            btnNewCustomField.TabIndex = 98;
            btnSave.TabIndex = 99;
            btnCancel.TabIndex = 100;
        }
        
        #region  Properties
        public bool OkPressed
        {
            get { return _okPressed; }
            set { _okPressed = value; }
        }

        public VaultItem Item
        {
            get { return _item; }
        }

        protected TextField ItemName
        {
            get { return this.txtItemName; }
        }

        protected View DetailView
        {
            set { _subView = value; }
        }
        #endregion
        
        #region Event Handlers
        protected abstract void SaveButtonClicked();
        
        private void CancelButtonClicked()
        {
            // Close dialog.
            Application.RequestStop();
        }
        
        protected void HandleControlEnter(View sender)
        {
            ((TextField)sender).SelectAll();
        }

        private void NewCustomFieldButtonClicked()
        {
            // Show the dialog for field type choice.
            SelectCustomFieldDialog dialog = new SelectCustomFieldDialog();
            dialog.Show();
            
            // Default to a text type of field.
            int fieldType = 0;
            
            // Check to see what was selected.
            if (dialog.OkPressed)
            {
                fieldType = dialog.FieldType;
            }
            
            // Create a new control row.
            scrCustomFields.CreateControlRow(fieldType);
        }
        #endregion
    }
}
