using System.ComponentModel;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public abstract class BaseDialog
    {
        protected Dialog? _dialog;
        protected bool _okPressed;
        
        protected void CancelButton_Clicked(object? sender, HandledEventArgs e)
        {
            // Set ok button flag.
            _okPressed = false;

            if (_dialog != null)
            {
                // Close dialog.
                Application.RequestStop(_dialog);
                _dialog.Dispose();
            }
        }
        
        public void Show()
        {
            if (_dialog != null)
            {
                Application.Run(_dialog);    
            }
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }
    }
}

