using Retrowarden.Repositories;
using Retrowarden.Models;

namespace Retrowarden.Workers;

public sealed class GetItemsWorker : RetrowardenWorker
{
    // Member variables.
    private SortedDictionary<string, VaultItem> _items;
    
    public GetItemsWorker(VaultRepository repository, string? dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _items = new SortedDictionary<string, VaultItem>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch vault items.
            e.Result = _repository.ListVaultItems();
        };
        
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _items = (SortedDictionary<string, VaultItem>) e.Result;
            }
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
            
            // Get rid of worker.
            _worker.Dispose();
        };    
    }
    
    public SortedDictionary<string, VaultItem> Items
    {
        get
        {
            return _items;
        }
    }
}