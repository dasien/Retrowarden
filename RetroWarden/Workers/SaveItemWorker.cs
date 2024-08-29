using Newtonsoft.Json;
using RetrowardenSDK.Repositories;
using RetrowardenSDK.Models;

namespace Retrowarden.Workers
{
    public sealed class SaveItemWorker : RetrowardenWorker
    {
        private readonly List<VaultItem> _items;
        private List<VaultItem> _results;
        
        public SaveItemWorker(VaultRepository repository, VaultItemSaveAction action, List<VaultItem> items, 
            string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Store items list.
            _items = items;
            
            // Work results.
            _results = new List<VaultItem>();
            
            // Check to see what kind of action we are performing.
            switch (action)
            {
                case VaultItemSaveAction.Create:
                    HandleItemCreate();
                    break;
                
                case VaultItemSaveAction.MoveToOrganization:
                    HandleItemOrgMove();
                    break;
                
                case VaultItemSaveAction.Update:
                case VaultItemSaveAction.MoveToFolder:
                    HandleItemEdit();
                    break;
                
                case VaultItemSaveAction.Delete:
                    HandleItemDelete();
                    break;
            }
                        
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

        private void HandleItemCreate()
        {
            _worker.DoWork += (s, e) => 
            {
                // Get first vault item in list (should be only one for this action.
                VaultItem item = _items[0];
                
                // Get encoded item string.
                string encodedItem = item.ToBase64EncodedString();

                // Execute the create method.
                VaultItem? result = _repository.CreateVaultItem(encodedItem);
                
                // Check to see if the command worked.
                if (result != null)
                {
                    _results.Add(result);
                }
            };
        }

        private void HandleItemEdit()
        {
            // In this case, we may have multiple actions.
            _worker.WorkerReportsProgress = true;
            
            _worker.DoWork += (s, e) =>
            {
                // Loop through items (should be only one in this case.
                for (int cnt = 0; cnt < _items.Count; cnt++)
                {
                    // Get the item.
                    VaultItem item = _items[cnt];
                    
                    // Get encoded item string.
                    string encodedItem = item.ToBase64EncodedString();
                    
                    // Try to save the item.
                    VaultItem? savedItem = _repository.UpdateVaultItem(item.Id, encodedItem);
                    
                    // Check to see that the item was saved.
                    if (savedItem != null && _repository.ExitCode == "0")
                    {
                        // Execute the edit method.
                        _results.Add(savedItem);
                    }
                    
                    // Update progress.
                    _worker.ReportProgress(cnt + 1);
                }
            };

            _worker.ProgressChanged += (s, e) =>
            {
                // Update label in working dialog.
                _workingDialog.ProgressMessage = $"{e.ProgressPercentage} of {_items.Count} completed.";
            };
        }

        private void HandleItemDelete()
        {
            _worker.DoWork += (s, e) =>
            {
                // Get first vault item in list (should be only one for this action.
                VaultItem item = _items[0];

                // Execute the delete method.
                _repository.DeleteVaultItem(item.Id);
                
                // Set results to item that was deleted.
                _results.Add(item);
            };
        }

        private void HandleItemOrgMove()
        {
            // In this case, we may have multiple actions.
            _worker.WorkerReportsProgress = true;
        
            _worker.DoWork += (s, e) =>
            {
                // Loop through items.
                for (int cnt = 0; cnt < _items.Count; cnt++)
                {
                    // Get the item.
                    VaultItem item = _items[cnt];
                    
                    // Check to see if there are collections.
                    if (item.CollectionIds != null)
                    {
                        // Get encoded collection id string.
                        string itemJSON = JsonConvert.SerializeObject(item.CollectionIds);
                        byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemJSON);
                        string encodedCollection = Convert.ToBase64String(itemBytes);
                        
                        // Check to see if values are present.
                        if (item.Id != null && item.OrganizationId != null)
                        {
                            // Execute the edit method.
                            _repository.MoveVaultItem(item.Id, item.OrganizationId, encodedCollection);
                        }
                    }

                    // Add the item to the results.
                    _results.Add(item);

                    // Update progress.
                    _worker.ReportProgress(cnt + 1);
                }
            };

            _worker.ProgressChanged += (s, e) =>
            {
                // Update label in working dialog.
                _workingDialog.ProgressMessage = $"{e.ProgressPercentage} of {_items.Count} completed.";
            };
        }
        
        public List<VaultItem> Results
        {
            get
            {
                return _results;
            }
        }
    }    
}

