using Retrowarden.Models;
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

        protected override void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_txtFolderName != null && _txtFolderName.Text.TrimSpace().Length == 0)
            {
                MessageBox.ErrorQuery("Values Missing", "Folder name is required.", "Ok");
            }
            
            // Check to see if we already have a folder with that name.
            if (_folders.FindIndex(o => _txtFolderName != null && o.Name == _txtFolderName.Text.ToString()) != -1)
            {
                MessageBox.ErrorQuery("Duplicate Value", "Folder name already exists.", "Ok");
            }
            
            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                
                if (_txtFolderName != null)
                {
                    _folderName = _txtFolderName.Text.ToString();
                }

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        protected override void InitializeComponent()
        {
            // Create Ok button.
            Button okButton = new Button(8, 6, "_Save");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(24, 6, "Cance_l");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Create Folder", 40, 10, okButton, cancelButton);

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
            _dialog.Add(lblFolder, _txtFolderName);

            // Set default control.
            _txtFolderName.SetFocus();
        }

        public string? FolderName
        {
            get { return _folderName; }
        }
    }
}