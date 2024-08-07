using Retrowarden.Controls;
using Retrowarden.Dialogs;
using Terminal.Gui;
using RetrowardenSDK.Models;

namespace Retrowarden.Views 
{
    public abstract partial class ItemDetailView
    {
        private View? _subView;
        protected VaultItem _item;
        private readonly List<VaultFolder> _folders;
        protected readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;
        
        protected ItemDetailView(VaultItem? item, List<VaultFolder> folders, VaultItemDetailViewState state, int scrollBottom) 
        {
            // Set private variables.
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            
            InitializeComponent(scrollBottom);
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
                    
                    // Create empty custom field scroll.
                    scrCustomFields = new CustomFieldScrollView(null, _item.ItemType)
                    {
                        X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true,
                        ContentSize = new Size(95, 10), Data = "scrCustomFields", TextAlignment = TextAlignment.Left
                    };
                    
                    // Add scroll to view.
                    fraCustomFieldList.Add(scrCustomFields);

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
            chkReprompt.Checked = _item.Reprompt == 1;
            stbDetail.Items = new StatusItem[]
            {
                new StatusItem(Key.Null, 
                    "Created On: " + _item.CreationDate + " | Last Updated On: " + _item.RevisionDate, 
                    null)
            };
            
            // Set the folder to the current folder (or "No Folder" as a null default.
            cboFolder.SelectedItem = _folders.FindIndex(o => o.Id == _item.FolderId);
            
            // Create new field scroll control.
            scrCustomFields = new CustomFieldScrollView(_item.CustomFields, _item.ItemType)
            {
                X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true,
                ContentSize = new Size(95, 10), Data = "scrCustomFields", TextAlignment = TextAlignment.Left
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
            fraNotes.Y = Pos.Bottom(_subView) + 1;
            
            // Update the custom fields frame.
            fraCustomFieldList.Y = Pos.Bottom(fraNotes) + 1;
            btnNewCustomField.Y = Pos.Bottom(fraCustomFieldList);
            
            // Update the save and cancel button locations relative to the new notes frame.
            btnSave.Y = Pos.Bottom(btnNewCustomField) + 1;
            btnCancel.Y = Pos.Bottom(btnNewCustomField) + 1;
            
            // Refresh display
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
                _item.FolderId = null;
            }

            else
            {
                _item.FolderId = _folders.ElementAt(cboFolder.SelectedItem).Id;
            }
            
            _item.IsFavorite = chkFavorite.Checked;
            _item.Notes = tvwNotes.Text.ToString();
            _item.Reprompt = chkReprompt.Checked ? 1 : 0;
            
            // Get the custom fields.
            _item.CustomFields = scrCustomFields.Fields;
        }

        protected virtual void SetTabOrder()
        {
            // Set tab order for common controls. 
            txtItemName.TabIndex = 0;
            cboFolder.TabIndex = 1;
            chkFavorite.TabIndex = 2;
            chkReprompt.TabIndex = 3;
            
            if (_subView != null)
            {
                _subView.TabIndex = 4;
            }
            
            tvwNotes.TabIndex = 95;
            scrCustomFields.TabIndex = 96;
            btnNewCustomField.TabIndex = 97;
            btnSave.TabIndex = 98;
            btnCancel.TabIndex = 99;
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
            // Get Y values for controal and scroll view
            int controlY = sender.Frame.Y;
            int viewY = scrMain.Frame.Bottom;
            
            // Check to see if we are lower than view.
            if (controlY > viewY)
            {
                // Loop until we are in view.
                while (controlY > viewY)
                {
                    scrMain.ScrollDown(5);
                    controlY--;
                }
            }
            
            // Scroll back up if out of view.
            else if (controlY < viewY)
            {
                // Loop until we are in view.
                while (controlY < viewY)
                {
                    scrMain.ScrollUp((5));
                    controlY++;
                }
            }

            // If this is a textfield, select the text in it.
            if (sender is TextField)
            {
                ((TextField)sender).SelectAll();
            }
        }

        private void NewCustomFieldButtonClicked()
        {
            // Show the dialog for field type choice.
            SelectCustomFieldDialog dialog = new SelectCustomFieldDialog();
            dialog.Show();
            
            // Check to see what was selected.
            if (dialog.OkPressed)
            {
                // Create a new control row.
                scrCustomFields.CreateControlRow(dialog.FieldType);
            }
        }
        #endregion
    }
}
