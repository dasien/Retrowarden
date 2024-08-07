using RetrowardenSDK.Models;
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class GetOrganizationMembersWorker : RetrowardenWorker
    {
        // Member variables.
        private List<Member>? _members;
        
        public GetOrganizationMembersWorker(VaultRepository repository, string? dialogMessage, string orgId) 
            : base(repository, dialogMessage)
        {
            // Add work event handler.
            _worker.DoWork += (s, e) => 
            {
                // Try to fetch organizations.
                e.Result = _repository.ListMembersForOrganization(orgId);
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Set member variable to result.
                _members = e.Result as List<Member>;
            
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }

                _worker.Dispose();
            };    
        }

        public List<Member>? Members
        {
            get { return _members; }
        }
    }    
}
