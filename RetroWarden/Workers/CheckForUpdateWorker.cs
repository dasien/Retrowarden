using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public class CheckForUpdateWorker : RetrowardenWorker
    {
        // Member variables.
        private bool _updateAvailable;
        
        public  CheckForUpdateWorker(VaultRepository repository, string dialogMessage) : base(repository, dialogMessage)
        {
            // Create members.
            _updateAvailable = false;
            
            // Run the login method.
            _worker.DoWork += (s, e) =>
            {
                // Execute the login method.
                e.Result = _repository.CheckForUpdate();
            };

            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see that we have a result.
                if (e.Result != null)
                {
                    // Get the generated password.
                    _updateAvailable = (bool) e.Result;
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

        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
        }
    }    
}

