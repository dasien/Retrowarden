using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace Retrowarden.Dialogs
{
    public abstract class BaseDialog
    {
        protected IApplication _app;
        protected Dialog? _dialog;
        protected bool _okPressed;

        protected BaseDialog(IApplication app)
        {
            _app = app;
        }

        protected void CancelButton_Clicked(object? sender, HandledEventArgs e)
        {
            _okPressed = false;

            if (_dialog != null)
            {
                _app.RequestStop(_dialog);
            }

            e.Handled = true;
        }

        public void Show()
        {
            if (_dialog != null)
            {
                _app.Run(_dialog);
            }
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }
    }
}

