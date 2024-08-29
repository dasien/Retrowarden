using RetrowardenSDK.Models;
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class CheckStatusWorker : RetrowardenWorker
    {
        // Member variables.
        private VaultStatus _status;
        
        public CheckStatusWorker(VaultRepository repository, string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _status = new VaultStatus();
        
            // Add work event handler.
            _worker.DoWork += (s, e) => 
            {
                // Try to fetch folders.
                e.Result = _repository.GetStatus();
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see if we have a result.
                if (e.Result != null)
                {
                    // Set member variable to result.
                    _status = (VaultStatus) e.Result;
                }
            
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }

                _worker.Dispose();
            };    
        }

        public VaultStatus Status
        {
            get { return _status; }
        }
    }    
}

