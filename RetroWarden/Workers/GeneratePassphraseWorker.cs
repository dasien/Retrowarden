using Retrowarden.Repositories;

namespace Retrowarden.Workers
{
    public sealed class GeneratePassphraseWorker : RetrowardenWorker
    {
        private string? _passphrase;
        
        public GeneratePassphraseWorker(VaultRepository repository, bool capitalize, bool useNumbers, 
            int words, string? sep, string? dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Run the login method.
            _worker.DoWork += (s, e) => 
            {
                // Execute the method.
                e.Result = _repository.GeneratePassphrase(words, sep, capitalize, useNumbers);
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Get the generated passphrase.
                _passphrase = (string?) e.Result;
                
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }

                _worker.Dispose();
            };    
        }

        public string? Passphrase
        {
            get { return _passphrase; }
        }
    }    
}