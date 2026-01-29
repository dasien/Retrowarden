using System.Collections.ObjectModel;
using System.ComponentModel;
using RetrowardenSDK.Models;
using Terminal.Gui.App;
using Terminal.Gui.Views;
using Terminal.Gui.ViewBase;

namespace Retrowarden.Dialogs
{
    public sealed class SelectFolderDialog : BaseDialog
    {
        // Controls.
        private ComboBox? _cboFolder;
        private int _folderIndex;
        private readonly List<VaultFolder> _folders;
        
        public SelectFolderDialog(IApplication app, List<VaultFolder> folders) : base(app)
        {
            _folders = folders;

            InitializeComponent();
        }
        
        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Check to see if required values are present.
            if (_cboFolder != null && _cboFolder.SelectedItem == -1)
            {
                MessageBox.ErrorQuery(_app,"Values Missing", "Please select a folder.", "Ok");
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
                _app.RequestStop(_dialog);
            }
        }
        
        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                Text = "Ok",
            };
            okButton.Accepting += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                Text = "Cancel"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Select Folder", Width = 50, Height = 10,
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
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
            _dialog.Add(lblFolder, _cboFolder);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);

            // Set default control.
            _cboFolder.SetFocus();
        }
        
        public int SelectedFolderIndex
        {
            get { return _folderIndex; }
        }
    }
}