using RetrowardenSDK.Repositories;
using Retrowarden.Dialogs;
using System.ComponentModel;

namespace Retrowarden.Workers
{
    public class RetrowardenWorker
    {
        // Member variables.
        protected readonly IVaultRepository _repository;
        protected readonly BackgroundWorker _worker;
        protected readonly WorkingDialog _workingDialog;

        protected RetrowardenWorker(IVaultRepository repository, string dialogMessage)
        {
            _repository = repository;
            _worker = new BackgroundWorker();
            _workingDialog = new WorkingDialog(dialogMessage);
        }
        public void Run()
        {
            // Run the worker.
            _worker.RunWorkerAsync();
            
            // Show working dialog.
            _workingDialog.Show();
        }
    }    
}

