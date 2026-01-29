using System.ComponentModel;
using RetrowardenSDK.Models;
using Terminal.Gui.App;
using Terminal.Gui.Views;
using Terminal.Gui.ViewBase;

namespace Retrowarden.Dialogs
{

    public sealed class AddFolderDialog : BaseDialog
    {
        // Controls.
        private TextField? _txtFolderName;

        // Other values.
        private string? _folderName;
        private readonly List<VaultFolder> _folders;
        
        public AddFolderDialog(IApplication app, List<VaultFolder> folders) : base(app)
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
                MessageBox.ErrorQuery(_app,"Values Missing", "Folder name is required.", "Ok");
            }
            
            // Check to see if we already have a folder with that name.
            if (_folders.FindIndex(o => _txtFolderName != null && o.Name == _txtFolderName.Text) != -1)
            {
                MessageBox.ErrorQuery(_app,"Duplicate Value", "Folder name already exists.", "Ok");
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
                _app.RequestStop(_dialog);
            }
        }

        private void InitializeComponent()
        {
            // Create modal view with dynamic sizing.
            _dialog = new Dialog()
            {
                Title = "Create Folder",
                Width = 45,
                Height = 10,
                ButtonAlignment = Alignment.Center,
                ButtonAlignmentModes = AlignmentModes.AddSpaceBetweenItems
            };

            // Create label with dynamic width based on content.
            Label lblFolder = new Label()
            {
                X = 2,
                Y = 1,
                Width = Dim.Auto(),
                Text = "Folder Name:"
            };

            // Create text input positioned relative to label.
            _txtFolderName = new TextField()
            {
                X = Pos.Right(lblFolder) + 1,
                Y = Pos.Top(lblFolder),
                Width = Dim.Fill(2)
            };

            // Create buttons.
            Button okButton = new Button()
            {
                Text = "_Save",
            };
            okButton.Accepting += OkButton_Clicked;

            Button cancelButton = new Button()
            {
                Text = "Cance_l"
            };
            cancelButton.Accepting += CancelButton_Clicked;

            // Add controls to view.
            _dialog.Add(lblFolder, _txtFolderName);
            _dialog.AddButton(okButton);
            _dialog.AddButton(cancelButton);

            // Set default control.
            _txtFolderName.SetFocus();
        }
        
        public string? FolderName
        {
            get { return _folderName; }
        }
    }
}