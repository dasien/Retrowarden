using System.ComponentModel;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public abstract class BaseDialog
    {
        protected Dialog? _dialog;
        protected bool _okPressed;

        protected abstract void InitializeComponent();

        protected virtual void OkButton_Clicked(object? sender, HandledEventArgs e)
        {
            _okPressed = true;
        }

        protected virtual void CancelButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set ok button flag.
            _okPressed = false;
            
            // Close dialog.
            Application.RequestStop(_dialog);
        }
        
        public virtual void Show()
        {
            Application.Run(_dialog);
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }
    }
}

