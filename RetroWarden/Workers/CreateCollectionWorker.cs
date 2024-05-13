using Retrowarden.Models;
using Retrowarden.Repositories;

namespace Retrowarden.Workers
{
    public sealed class CreateCollectionWorker : RetrowardenWorker
    {
        private VaultCollection _results;
        
        public  CreateCollectionWorker(VaultRepository repository, string? dialogMessage, VaultCollection collection) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _results = new VaultCollection();
            
            // Run the create folder method.
            _worker.DoWork += (s, e) =>
            {
                // Get encoded item string.
                string encodedItem = collection.ToBase64EncodedString();
                
                // Execute the save.
                VaultCollection? result = _repository.CreateCollection(encodedItem);
                
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
        
        public VaultCollection Collection
        {
            get
            {
                return _results;
            }
        }
    }    
}

