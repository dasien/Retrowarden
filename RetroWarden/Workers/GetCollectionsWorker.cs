using RetrowardenSDK.Repositories;
using RetrowardenSDK.Models;

namespace Retrowarden.Workers;

public sealed class GetCollectionsWorker : RetrowardenWorker
{
    private List<VaultCollection> _collections;

    public GetCollectionsWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _collections = new List<VaultCollection>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch collections.
            e.Result = _repository.ListCollections();
        };

        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _collections = (List<VaultCollection>) e.Result;
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
    
    public List<VaultCollection> Collections
    {
        get { return _collections; }
    }
}