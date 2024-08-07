using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class LockWorker : RetrowardenWorker
    {
        public LockWorker(VaultRepository repository, string? dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Run the login method.
            _worker.DoWork += (s, e) =>
            {
                // Execute the login method.
                _repository.Lock();
            };

            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop)
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }

                _worker.Dispose();
            };
        }
    }
}

