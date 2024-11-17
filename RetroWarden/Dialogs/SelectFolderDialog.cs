using System.Collections.ObjectModel;
using System.ComponentModel;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public sealed class SelectFolderDialog : BaseDialog
    {
        // Controls.
        private ComboBox? _cboFolder;
        private int _folderIndex;
        private readonly List<VaultFolder> _folders;
        
        public SelectFolderDialog(List<VaultFolder> folders)
        {
            // Initialize members.
            _folders = folders;
            
            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Check to see if required values are present.
            if (_cboFolder != null && _cboFolder.SelectedItem == -1)
            {
                MessageBox.ErrorQuery("Values Missing", "Please select a folder.", "Ok");
            }

            else
            {
                // Set value.
                if (_cboFolder != null)
                {
                    _folderIndex = _cboFolder.SelectedItem;
                }

                // Set flag for ok button and values.
                _okPressed = true;

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }
        
        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 8, Y = 6, Text = "Ok", IsDefault = true
            };
            okButton.Accept += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 24, Y = 6, Text = "Cancel"
            };
            cancelButton.Accept += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Folder", Width = 50,Height = 10
            };

            // Create label.
            Label lblFolder = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true, Text = "Folder:"
            };

            // Create folder dropdown.
            _cboFolder = new ComboBox()
            {
                X = 15, Y = 2, Width = 30, Height = 5, CanFocus = true, Visible = true,
            };

            // Set source for combobox.
            ObservableCollection<VaultFolder> folders = new ObservableCollection<VaultFolder>(_folders);
            _cboFolder.SetSource(folders);
            
            // Add controls to view.
            _dialog.Add(lblFolder, _cboFolder, okButton, cancelButton);

            // Set default control.
            _cboFolder.SetFocus();
        }
        
        public int SelectedFolderIndex
        {
            get { return _folderIndex; }
        }
    }
}