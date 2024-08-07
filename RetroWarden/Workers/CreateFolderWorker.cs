using RetrowardenSDK.Repositories;
using RetrowardenSDK.Models;

namespace Retrowarden.Workers
{
    public sealed class CreateFolderWorker : RetrowardenWorker
    {
        private VaultFolder _results;
        
        public CreateFolderWorker(VaultRepository repository, string? dialogMessage, VaultFolder folder) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _results = new VaultFolder();
            
            // Run the create folder method.
            _worker.DoWork += (s, e) =>
            {
                // Get encoded item string.
                string encodedItem = folder.ToBase64EncodedString();
                
                // Execute the save.
                VaultFolder? result = _repository.CreateFolder(encodedItem);
                
                // Check to see that one was saved.
                if (result != null && _repository.ExitCode == "0")
                {
                    // Store the result.
                    _results = result;
                }
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
        
        public VaultFolder Folder
        {
            get
            {
                return _results;
            }
        }
    }    
}
