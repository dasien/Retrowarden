using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public abstract class BaseDialog
    {
        protected Dialog? _dialog;
        protected bool _okPressed;

        protected abstract void InitializeComponent();

        protected virtual void OkButton_Clicked()
        {
            _okPressed = true;
        }

        protected virtual void CancelButton_Clicked()
        {
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

