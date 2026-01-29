using RetrowardenSDK.Repositories;
using Retrowarden.Dialogs;
using System.ComponentModel;

namespace Retrowarden.Workers
{
    public class RetrowardenWorker
    {
        protected readonly Terminal.Gui.App.IApplication _app;
        protected readonly IVaultRepository _repository;
        protected readonly BackgroundWorker _worker;
        protected readonly WorkingDialog _workingDialog;

        protected RetrowardenWorker(Terminal.Gui.App.IApplication app, IVaultRepository repository, string dialogMessage)
        {
            _app = app;
            _repository = repository;
            _worker = new BackgroundWorker();
            _workingDialog = new WorkingDialog(app, dialogMessage);
        }
        public void Run()
        {
            _app.AddTimeout(TimeSpan.FromMilliseconds(10), () =>
            {
                _worker.RunWorkerAsync();
                return false;
            });

            _workingDialog.Show();
        }
    }    
}

