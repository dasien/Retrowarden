using System.Collections.ObjectModel;
using System.ComponentModel;
using Retrowarden.Controls;
using Retrowarden.Dialogs;
using RetrowardenSDK.Models;
using Terminal.Gui.App;
using Terminal.Gui.Views;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Drivers;

namespace Retrowarden.Views 
{
    public abstract partial class ItemDetailView
    {
        protected IApplication _app;
        private View? _subView;
        protected VaultItem _item;
        private readonly List<VaultFolder> _folders;
        protected readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;

        protected ItemDetailView(IApplication app, VaultItem? item, List<VaultFolder> folders, VaultItemDetailViewState state)
        {
            _app = app;
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;

            InitializeComponent();
        }

        public void Show()
        {
            _app.Run(this);
        }

        protected void SetupView()
        {
            AssembleView();
            
            // Check to make sure we have a sub-view to add (Secure Note doesn't have one)
            if (_subView != null)
            {
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
                    scrCustomFields = new CustomFieldScrollView(_app,null, _item.ItemType)
                    {
                        X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true, 
                        Data = "scrCustomFields", TextAlignment = Alignment.Start
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
            chkFavorite.Value = _item.IsFavorite ? CheckState.Checked : CheckState.UnChecked;
            tvwNotes.Text = _item.Notes ?? "";
            chkReprompt.Value = _item.Reprompt == 1 ? CheckState.Checked : CheckState.UnChecked;
            stbDetail.Add(new Shortcut(KeyCode.Null, "Created On: " + _item.CreationDate + 
                                                     " | Last Updated On: " + _item.RevisionDate, null));
            
            // Set the folder to the current folder (or "No Folder" as a null default.
            cboFolder.SelectedItem = _folders.FindIndex(o => o.Id == _item.FolderId);
            
            // Create new field scroll control.
            scrCustomFields = new CustomFieldScrollView(_app, _item.CustomFields, _item.ItemType)
            {
                X = 0, Y = 0, Width = 95, Height = 6, Visible = true, CanFocus = true, Enabled = true,
                Data = "scrCustomFields", TextAlignment = Alignment.Start
            };
            
            // Add scroll to view.
            fraCustomFieldList.Add(scrCustomFields);
        }

        protected void SetItemNameControlFocus()
        {
            // This is part of a bug fix that allows focus in the URI list frame.
            //txtItemName.HasFocus = true;
        }
        
        private void DisableView()
        {
            btnSave.Enabled = false;
        }

        private void AssembleView()
        {
            // Add header controls.
            this.Add(this.lblItemName);
            this.Add(this.lblFolder);
            this.Add(this.txtItemName);
            this.Add(this.cboFolder);
            this.Add(this.chkFavorite);
            this.Add(this.chkReprompt);

            // Add custom view.
            // Check to make sure we have a sub-view to add (Secure Note doesn't have one)
            if (_subView != null)
            {
                // Add subview to view.
                Add(_subView);
            }
            
            // Add footer controls.
            this.fraNotes.Add(this.tvwNotes);
            this.Add(this.fraNotes);
            this.Add(this.fraCustomFieldList);
            this.Add(this.btnNewCustomField);
            this.Add(this.btnSave);
            this.Add(this.btnCancel);
            this.Add(this.stbDetail);

        }
        
        private void InitializeLists()
        {
            // Set folder comboxbox source.
            ObservableCollection<VaultFolder> folderCollection = new ObservableCollection<VaultFolder>(_folders);
            cboFolder.SetSource(folderCollection);
        }

        private void RelocateFooterControls()
        {
            // Update Y of the notes frame to detail view bottom + 1
            fraNotes.Y = Pos.Bottom(_subView) + 1;

            // Update the custom fields frame.
            fraCustomFieldList.Y = Pos.Bottom(fraNotes) + 1;
            btnNewCustomField.Y = Pos.Bottom(fraCustomFieldList);

            // Update the save and cancel button locations relative to the new notes frame.
            btnSave.Y = Pos.Bottom(btnNewCustomField) + 1;
            btnCancel.Y = Pos.Bottom(btnNewCustomField) + 1;

            // Calculate and set the content height dynamically based on subview height.
            // Formula: subview.Y(3) + subview.Height + gap(1) + fraNotes.Height(9) + gap(1) +
            //          fraCustomFieldList.Height(7) + btnNewCustomField(1) + gap(1) + btnSave(1) + buffer(1)
            // = 3 + subviewHeight + 22
            int subviewHeight = _subView?.Frame.Height ?? 0;
            if (subviewHeight == 0)
            {
                // If Frame isn't computed yet, use a default based on the Height property
                // This handles the case where layout hasn't run yet
                subviewHeight = 15; // Default fallback
            }
            int contentHeight = 3 + subviewHeight + 22;
            
            // Width 0 means use Viewport width (no horizontal scrolling)
            SetContentSize(new System.Drawing.Size(0, contentHeight));

            // Refresh display
            SetNeedsDraw();
        }
        
        protected virtual void UpdateItem()
        {
            // Set item properties.
            _item.ItemName = txtItemName.Text ?? "";
            
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
            
            _item.IsFavorite = chkFavorite.Value == CheckState.Checked;
            _item.Notes = tvwNotes.Text;
            _item.Reprompt = chkReprompt.Value == CheckState.Checked ? 1 : 0;
            
            // Get the custom fields.
            _item.CustomFields = scrCustomFields.Fields;
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
        protected abstract void SaveButtonClicked(object? sender, HandledEventArgs e);
        
        private void CancelButtonClicked(object? sender, HandledEventArgs e)
        {
            // Close dialog.
            _app.RequestStop();
        }

        protected void HandleFocusChange(View? sender, HasFocusEventArgs eventArgs)
        {
            // This offset is necessary because of labels being over controls vertically.
            int offset = 1;
            
            // Get the view which is receiving focus.
            View gettingFocus = eventArgs.NewFocused;
            
            // Check to see if the view below the viewport.
            if (gettingFocus.Frame.Y + gettingFocus.Frame.Height > Viewport.Bottom)
            {
                // Scroll down until control is in the frame.
                ScrollVertical(gettingFocus.Frame.Y + gettingFocus.Frame.Height - Viewport.Bottom);
            }
            
            // Check to see if the view is above the viewport.
            else if (gettingFocus.Frame.Top < Viewport.Top)
            {
                // Scroll up until control is in the frame.
                ScrollVertical(gettingFocus.Frame.Top - Viewport.Top - offset);
            }
            
            // Check to see if the view is too far right.
            if (gettingFocus.Frame.X + gettingFocus.Frame.Width > Viewport.Right)
            {
                // Scroll right until control is in frame.
                ScrollHorizontal(gettingFocus.Frame.X + gettingFocus.Frame.Width - Viewport.Right);
            }
            
            // Check to see if the view is too far left.
            if (gettingFocus.Frame.Left < Viewport.Left)
            {
                // Scroll left until control is in frame.
                ScrollHorizontal(gettingFocus.Frame.Left - Viewport.Left);
            }

            // If this is a textfield, select the text in it.
            if (sender is TextField)
            {
                ((TextField)sender).SelectAll();
            }
        }

        private void NewCustomFieldButtonClicked(object? sender, HandledEventArgs e)
        {
            // Show the dialog for field type choice.
            SelectCustomFieldDialog dialog = new SelectCustomFieldDialog(_app);
            dialog.Show();

            // Check to see what was selected.
            if (dialog.OkPressed)
            {
                // Check to see that the field type is set.
                if (dialog.FieldType != null)
                {
                    // Create a new control row.
                    scrCustomFields.CreateControlRow((int)dialog.FieldType);
                }
            }

            e.Handled = true;
        }
        
        private void HandleMouseEvent(object? sender, Mouse MouseEventArgs)
        {
            switch (MouseEventArgs.Flags)
            {
                // Check to see if this is a wheel scroll down.
                case MouseFlags.WheeledDown:

                    // Allow scroll down if there's content below the visible area.
                    if (Viewport.Y + Viewport.Height < GetContentSize().Height)
                    {
                        ScrollVertical(1);
                    }
                    break;

                case MouseFlags.WheeledUp:

                    // Only allow scroll up if we're not already at the top (Y > 0).
                    if (Viewport.Y > 0)
                    {
                        ScrollVertical(-1);
                    }
                    break;

                case MouseFlags.WheeledLeft:

                    // Only allow scroll left if we're not already at the left edge.
                    if (Viewport.X > 0)
                    {
                        ScrollHorizontal(-1);
                    }
                    break;

                case MouseFlags.WheeledRight:

                    // Allow scroll right if there's content to the right of the visible area.
                    if (Viewport.X + Viewport.Width < GetContentSize().Width)
                    {
                        ScrollHorizontal(1);
                    }
                    break;
            }
        }
        
        #endregion
    }
}
