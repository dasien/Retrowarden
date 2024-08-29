using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class GeneratePasswordWorker : RetrowardenWorker
    {
        private string _password;
        
        public GeneratePasswordWorker(VaultRepository repository, bool useUpper, bool useLower, bool useDigits, 
            bool useSpecial, int length, string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _password = "";
            
            // Run the login method.
            _worker.DoWork += (s, e) => 
            {
                // Execute the method.
                e.Result = _repository.GeneratePassword(useUpper, useLower, useDigits, useSpecial, length);
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) => 
            {
                // Check to see that we have a result.
                if (e.Result != null)
                {
                    // Get the generated password.
                    _password = (string) e.Result;
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

        public string Password
        {
            get { return _password; }
        }
    }    
}

