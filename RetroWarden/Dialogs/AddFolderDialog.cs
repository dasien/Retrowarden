using System.ComponentModel;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{

    public sealed class AddFolderDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtFolderName;

        // Other values.
        private string? _folderName;
        private readonly List<VaultFolder> _folders;
        
        public AddFolderDialog(List<VaultFolder> folders)
        {
            _folderName = "";
            _okPressed = false;
            _folders = folders;
            
            InitializeComponent();
        }

        private void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Check to see if required values are present.
            if (_txtFolderName != null && _txtFolderName.Text.Trim().Length == 0)
            {
                MessageBox.ErrorQuery("Values Missing", "Folder name is required.", "Ok");
            }
            
            // Check to see if we already have a folder with that name.
            if (_folders.FindIndex(o => _txtFolderName != null && o.Name == _txtFolderName.Text) != -1)
            {
                MessageBox.ErrorQuery("Duplicate Value", "Folder name already exists.", "Ok");
            }
            
            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtFolderName != null)
                {
                    _folderName = _txtFolderName.Text;
                }

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        private void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button()
            {
                X = 8, Y =6, Text = "_Save"
            };
            okButton.Accept += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button()
            {
                X = 24, Y = 6, Title = "Cance_l"
                
            };
            cancelButton.Accept += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog()
            {
                Title = "Create Folder", Width = 40, Height =10
            };

            // Create labels.
            Label lblFolder = new Label()
            {
                X = 3, Y = 2, Width = 10, Height = 1, CanFocus = false, Visible = true, Text = "Folder Name:"
            };
            
            // Create text input.
            _txtFolderName = new TextField()
            {
                X = 15, Y = 2, Width = 20, Height = 1, CanFocus = true, Visible = true
            };
            
            // Add controls to view.
            _dialog.Add(lblFolder, _txtFolderName, okButton, cancelButton);

            // Set default control.
            _txtFolderName.SetFocus();
        }
        
        public string? FolderName
        {
            get { return _folderName; }
        }
    }
}