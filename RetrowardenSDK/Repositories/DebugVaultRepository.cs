using Newtonsoft.Json;
using RetrowardenSDK.Models;

namespace RetrowardenSDK.Repositories;

public sealed class DebugVaultRepository : IVaultRepository
{
        private string _error;
        private bool _isLocked;
        private bool _isLoggedIn;
        private bool _orgEnabled;
        private string _cmdExitCode;
        private readonly JsonSerializerSettings _settings;

        public DebugVaultRepository()
        {
            // Set defaults.
            _orgEnabled = false;
            _isLocked = true;
            _cmdExitCode = "0";
            _isLoggedIn = false;
            _error = "";
            
            // Set serialization rules.
            _settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        #region Auth Methods
        public void Login(string email, string password)
        {
            // Simulate logged in.
            _isLoggedIn = true;
            _isLocked = false;
        }
        
        public void Logout()
        {
            // Check to make sure we are logged in.
            if (_isLoggedIn)
            {
                // Flip login bit.
                _isLoggedIn = false;

                // Flip locked bit.
                _isLocked = true;
            }
        }

        public void Unlock(string password)
        {
            // Simulate unlock.
            _isLocked = false;
        }

        public void Lock()
        {
            // Simulate lock.
            _isLocked = true;
        }
        #endregion
        
        #region Read Methods
        public List<VaultFolder>? ListFolders()
        {
            // Return list.
            return JsonConvert.DeserializeObject<List<VaultFolder>>(_folders, _settings);
        }

        public List<VaultCollection>? ListCollections()
        {
            // Return list.
            return JsonConvert.DeserializeObject<List<VaultCollection>>(_collections, _settings);
        }
        
        public List<Organization>? ListOrganizations()
        {
            // Return list.
            return JsonConvert.DeserializeObject<List<Organization>>(_organizations, _settings);
        }

        public List<Member>? ListMembersForOrganization(string orgId)
        {
            // Return list.
            return JsonConvert.DeserializeObject<List<Member>>(_members, _settings);
        }
        
        public SortedDictionary<string, VaultItem>? ListVaultItems()
        {
            // Return object.
            SortedDictionary<string, VaultItem>? retVal = null;
            
            // Get item list.
            List<VaultItem>? items  = JsonConvert.DeserializeObject<List<VaultItem>>(_items, _settings);

            // Update the list with helper property for sorting on item value.
            items = SetSortValueForItemList(items);
            
            // Check to make sure we have a list.
            if (items != null)
            {
                // Migrate to a dictionary.
                retVal = new SortedDictionary<string, VaultItem>(items.ToDictionary(keySelector: m => m.Id,
                    elementSelector: m => m));
            }
            
            // Return it.
            return retVal;
        }

        public bool CheckForUpdate()
        {
            // Return result.
            return false;
        }

        public VaultStatus? GetStatus()
        {
            // Return the status object.
            return JsonConvert.DeserializeObject<VaultStatus>(_status, _settings);
        }
        #endregion
        
        #region Vault Item Write Methods
        public VaultItem? CreateVaultItem(string encodedItem)
        {
            // Return saved object.
            return null;
        }

        public VaultItem? UpdateVaultItem(string id, string encodedItem)
        {
            // Return saved object.
            return null;
        }

        public void DeleteVaultItem(string itemId)
        {
    
        }

        public VaultItem? MoveVaultItem(string id, string orgId, string encodedItem)
        {
            // Return saved object.
            return null;
        }
        #endregion
        
        #region Folder Write Method
        public VaultFolder? CreateFolder(string encodedItem)
        {
            // Return saved object.
            return new VaultFolder()
            {
                Id = "123456", Name = "Created Folder"
            };
        }
        #endregion
        
        #region Collection Write Method
        public VaultCollection? CreateCollection(string encodedItem)
        {
            // The return value.
            VaultCollection? retVal = null;
            
            // Return saved object.
            return retVal;
        }
        #endregion
        
        #region Generate Methods
        public string GeneratePassword(bool useUpper, bool useLower, bool useDigits, bool useSpecial, int length)
        {
            // Return password.
            return "ABC-123!";
        }

        public string GeneratePassphrase(int numWords, string? sepChar, bool useUpper, bool useDigits)
        {
            // Return password.
            return "pyrex-pickle-blowfish";
        }
        #endregion
        
        #region Helper Methods

        private List<VaultItem>? SetSortValueForItemList(List<VaultItem>? items)
        {
            // Check to see if there is a list.
            if (items != null)
            {
                // Loop through the list of items.
                foreach (VaultItem item in items)
                {
                    // Update sort value column.
                    SetSortValueForItem(item);
                }
            }
            
            // Return the udpated list.
            return items;
        }

        private VaultItem SetSortValueForItem(VaultItem item)
        {
            // Check the item type.
            switch (item.ItemType)
            {
                // Login
                case 1:
                    if (item.Login != null)
                    {
                        item.ListSortValue = item.Login.UserName;
                    }
                    break;
                
                // Note
                case 2:
                    item.ListSortValue = string.Empty;
                    break;
                
                // Card
                case 3:
                    if (item.Card != null)
                    {
                        item.ListSortValue = item.Card.GetListViewColumnText();
                    }
                    break;
                
                // Identity
                case 4:
                    if (item.Identity != null)
                    {
                        item.ListSortValue = item.Identity.GetListViewColumnText();
                    }
                    break;
            }
            
            // Return the updated item.
            return item;
        }
        #endregion
        
        #region Properties
        public string ExitCode
        {
            get
            {
                return _cmdExitCode;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _error;
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
        }

        public bool IsLocked
        {
            get { return _isLocked; }
        }

        public bool IsOrgEngabled
        {
            get { return _orgEnabled; }
        }
        #endregion
        
        #region JSON Blobs

        private string _folders =
            "[{\"object\":\"folder\",\"id\":\"a744f925-d132-4644-94a7-b17800e38b14\",\"name\":\"Test Folder\"},{\"object\":\"folder\",\"id\":null,\"name\":\"No Folder\"}]";
        private string _collections = "[]";
        private string _organizations = "[]";
        private string _members = "[]";
        private string _items =
            "[{\"passwordHistory\":null,\"revisionDate\":\"2024-05-31T18:44:03.296Z\",\"creationDate\":\"2024-05-31T18:44:03.296Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"52b9a64d-ef4e-48c9-b4b3-b1810134bb1f\",\"organizationId\":null,\"folderId\":null,\"type\":1,\"reprompt\":0,\"name\":\"Another Login\",\"notes\":null,\"favorite\":true,\"login\":{\"fido2Credentials\":[],\"uris\":[],\"username\":\"test\",\"password\":\"bbbbb\",\"totp\":null,\"passwordRevisionDate\":null},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-06-05T13:29:16.193Z\",\"creationDate\":\"2024-06-05T13:23:01.853Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"ebe80fe2-ecce-47c4-b355-b18600dc8f1e\",\"organizationId\":null,\"folderId\":null,\"type\":1,\"reprompt\":0,\"name\":\"custom field test\",\"notes\":null,\"favorite\":false,\"fields\":[{\"name\":\"test\",\"value\":\"test\",\"type\":0,\"linkedId\":0},{\"name\":\"test2\",\"value\":\"test2\",\"type\":0,\"linkedId\":0}],\"login\":{\"fido2Credentials\":[],\"uris\":[],\"username\":null,\"password\":null,\"totp\":null,\"passwordRevisionDate\":null},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-11-19T15:17:11.686Z\",\"creationDate\":\"2024-05-22T19:20:10.390Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"15abef21-f3c4-4d5a-9be8-b178013ea6af\",\"organizationId\":null,\"folderId\":\"a744f925-d132-4644-94a7-b17800e38b14\",\"type\":3,\"reprompt\":1,\"name\":\"Test Card\",\"notes\":\"Test Note\",\"favorite\":true,\"card\":{\"cardholderName\":\"Name Name\",\"brand\":\"Visa\",\"number\":\"1111222233334444\",\"expMonth\":\"2\",\"expYear\":\"2026\",\"code\":\"456\"},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-28T19:47:38.230Z\",\"creationDate\":\"2024-05-22T19:37:09.363Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"0504716c-5108-4663-95fc-b178014350cb\",\"organizationId\":null,\"folderId\":null,\"type\":3,\"reprompt\":0,\"name\":\"Test Card 2\",\"notes\":null,\"favorite\":false,\"card\":{\"cardholderName\":\"blah\",\"brand\":\"Mastercard\",\"number\":\"4444555544445555\",\"expMonth\":\"8\",\"expYear\":\"2026\",\"code\":\"234\"},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-22T13:50:11.236Z\",\"creationDate\":\"2024-05-22T13:50:11.236Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"319f1653-c80e-4fdb-9a77-b17800e4048d\",\"organizationId\":null,\"folderId\":\"a744f925-d132-4644-94a7-b17800e38b14\",\"type\":1,\"reprompt\":0,\"name\":\"Test Login\",\"notes\":\"New test note\",\"favorite\":true,\"login\":{\"fido2Credentials\":[],\"uris\":[],\"username\":\"test\",\"password\":\"test\",\"totp\":null,\"passwordRevisionDate\":null},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-24T20:33:26.896Z\",\"creationDate\":\"2024-05-24T20:33:26.896Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"7d41fc8d-fd57-4585-9f90-b17a0152c6d8\",\"organizationId\":null,\"folderId\":null,\"type\":2,\"reprompt\":0,\"name\":\"Test Note 2\",\"notes\":\"Test\",\"favorite\":false,\"secureNote\":{\"type\":0},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-29T14:30:32.976Z\",\"creationDate\":\"2024-05-29T14:30:32.976Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"408fd995-f0da-4c4e-bc1d-b17f00ef1a87\",\"organizationId\":null,\"folderId\":null,\"type\":2,\"reprompt\":0,\"name\":\"Test Note 3\",\"notes\":\"blah\",\"favorite\":false,\"secureNote\":{\"type\":0},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-29T14:32:26.540Z\",\"creationDate\":\"2024-05-29T14:32:26.540Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"fc6ad52b-b42f-403b-90e3-b17f00ef9f9d\",\"organizationId\":null,\"folderId\":null,\"type\":2,\"reprompt\":0,\"name\":\"Test Note 4\",\"notes\":\"blah\",\"favorite\":false,\"secureNote\":{\"type\":0},\"collectionIds\":[]},{\"passwordHistory\":null,\"revisionDate\":\"2024-05-29T14:40:08.573Z\",\"creationDate\":\"2024-05-29T14:40:08.573Z\",\"deletedDate\":null,\"object\":\"item\",\"id\":\"03ab3bf2-8e65-4f27-be56-b17f00f1bd0e\",\"organizationId\":null,\"folderId\":null,\"type\":2,\"reprompt\":0,\"name\":\"Test Note 5\",\"notes\":\"more notes\",\"favorite\":false,\"secureNote\":{\"type\":0},\"collectionIds\":[]}]";
        private string _status =
            "{\"serverUrl\":null,\"lastSync\":\"2024-11-19T19:07:49.979Z\",\"userEmail\":\"rwtest@gmail.com\",\"userId\":\"d1a790f8-4453-448f-8a65-b17800dfc45d\",\"status\":\"locked\"}";
        #endregion
}