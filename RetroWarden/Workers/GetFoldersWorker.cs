using RetrowardenSDK.Repositories;
using RetrowardenSDK.Models;

namespace Retrowarden.Workers;

public sealed class GetFoldersWorker : RetrowardenWorker
{
    // Member variables.
    private List<VaultFolder> _folders;
    
    public GetFoldersWorker(VaultRepository repository, string? dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _folders = new List<VaultFolder>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch folders.
            e.Result = _repository.ListFolders();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _folders = (List<VaultFolder>) e.Result;
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

    public List<VaultFolder> Folders
    {
        get { return _folders; }
    }
}