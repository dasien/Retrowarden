using System.Diagnostics;
using Newtonsoft.Json;
using RetrowardenSDK.Models;

namespace RetrowardenSDK.Repositories
{
    public sealed class VaultRepository
    {
        private readonly Process _bwcli;
        private string _response;
        private string _error;
        private string _sessionKey;
        private bool _isLocked;
        private bool _isLoggedIn;
        private bool _orgEnabled;
        private string _cmdExitCode;
        private readonly JsonSerializerSettings _settings;

        public VaultRepository(string bwExeLocation)
        {
            // Set defaults.
            _response = "";
            _orgEnabled = false;
            _isLocked = true;
            _cmdExitCode = "";
            _isLoggedIn = false;
            _sessionKey = "";
            _error = "";
            
            // Set serialization rules.
            _settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            
            // Create exe caller.
            _bwcli = new Process();
            _bwcli.StartInfo.FileName = bwExeLocation;

            // Set call options
            _bwcli.EnableRaisingEvents = true;
            _bwcli.StartInfo.UseShellExecute = false;
            _bwcli.StartInfo.RedirectStandardError = true;
            _bwcli.StartInfo.RedirectStandardOutput = true;

            // Attach events.
            _bwcli.OutputDataReceived += proxyCmd_DidReceiveData;
            _bwcli.ErrorDataReceived += proxyCmd_DidReceiveError;
            _bwcli.Exited += proxyCmd_DidExit;
        }

        #region Auth Methods
        public void Login(string email, string password)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("login");
            _bwcli.StartInfo.ArgumentList.Add(email);
            _bwcli.StartInfo.ArgumentList.Add(password);

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get session key from response.
                string marker = "--session ";

                // Make sure the marker is in the response.
                if (_response.Contains(marker))
                {
                    // Find the start of the key
                    int pos = _response.IndexOf(marker, 0, StringComparison.Ordinal) + marker.Length;

                    // Store the session key for future calls.
                    _sessionKey = _response.Substring(pos);

                    // Set env variable.
                    _bwcli.StartInfo.Environment.Add("BW_SESSION", _sessionKey);

                    // At this point we are logged in.
                    _isLoggedIn = true;
                    _isLocked = false;
                }
            }
        }
        
        public void Logout()
        {
            // Check to make sure we are logged in.
            if (_isLoggedIn)
            {
                // Add parameters for call.
                _bwcli.StartInfo.ArgumentList.Add("logout");
                
                // Execute.
                ExecuteCommand();

                // Check to see if the command worked.
                if (_cmdExitCode == "0")
                {
                    // Flip login bit.
                    _isLoggedIn = false;

                    // Flip locked bit.
                    _isLocked = true;
                }
            }
        }

        public void Unlock(string password)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("unlock");
            _bwcli.StartInfo.ArgumentList.Add(password);

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get session key from response.
                string marker = "--session ";

                // Make sure the marker is in the response.
                if (_response.Contains(marker))
                {
                    // Find the start of the key
                    int pos = _response.IndexOf(marker, 0, StringComparison.Ordinal) + marker.Length;

                    // Store the session key for future calls.
                    _sessionKey = _response.Substring(pos);

                    // Set env variable.
                    _bwcli.StartInfo.Environment.Add("BW_SESSION", _sessionKey);

                    // At this point we are unlocked.
                    _isLocked = false;
                }
            }
        }

        public void Lock()
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("lock");

            // Execute.
            ExecuteCommand();
            
            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Flip locked bit.
                _isLocked = true;
            }
        }
        #endregion
        
        #region Read Methods
        public List<VaultFolder>? ListFolders()
        {
            // Return object.
            List<VaultFolder>? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("folders");

            // Execute.
            ExecuteCommand();
          
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                 // Get item list.
                retVal = JsonConvert.DeserializeObject<List<VaultFolder>>(_response, _settings);
            }
            
            // Return list.
            return retVal;
        }

        public List<VaultCollection>? ListCollections()
        {
            // Return value.
            List<VaultCollection>? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("collections");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<VaultCollection>>(_response, _settings);
            }
            
            // Return list.
            return retVal;
        }
        
        public List<Organization>? ListOrganizations()
        {
            // Return value.
            List<Organization>? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("organizations");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<Organization>>(_response, _settings);
                
                // Check to see if there are any orgs.
                _orgEnabled = retVal != null && retVal.Count != 0;
            }
            
            // Return list.
            return retVal;
        }

        public List<Member>? ListMembersForOrganization(string orgId)
        {
            // Return value.
            List<Member>? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("org-members");
            _bwcli.StartInfo.ArgumentList.Add("--organizationid");
            _bwcli.StartInfo.ArgumentList.Add(orgId);
            
            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<Member>>(_response, _settings);
            }
            
            // Return list.
            return retVal;
        }
        
        public SortedDictionary<string, VaultItem>? ListVaultItems()
        {
            // Return object.
            SortedDictionary<string, VaultItem>? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("items");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                List<VaultItem>? items  = JsonConvert.DeserializeObject<List<VaultItem>>(_response, _settings);

                // Update the list with helper property for sorting on item value.
                items = SetSortValueForItemList(items);
                
                // Check to make sure we have a list.
                if (items != null)
                {
                    // Migrate to a dictionary.
                    retVal = new SortedDictionary<string, VaultItem>(items.ToDictionary(keySelector: m => m.Id,
                        elementSelector: m => m));
                }
            }
            
            // Return it.
            return retVal;
        }

        public bool CheckForUpdate()
        {
            // The return value.
            bool retVal = false;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("update");

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get session key from response.
                string marker = "A new version is available:";

                // Make sure the marker is in the response.
                if (_response.Contains(marker))
                {
                    // Flag that an update exists.
                    retVal = true;
                }
            }

            // Return result.
            return retVal;
        }

        public VaultStatus? GetStatus()
        {
            // The return value.
            VaultStatus? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("status");

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get status.
                retVal = JsonConvert.DeserializeObject<VaultStatus>(_response, _settings);

            }
            
            // Return the status object.
            return retVal;
        }
        #endregion
        
        #region Vault Item Write Methods
        public VaultItem? CreateVaultItem(string encodedItem)
        {
            // The return value.
            VaultItem? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("create");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultItem>(_response, _settings);
                
                // Check to see if we have an object.
                if (retVal != null)
                {
                    // Update the sort value.
                    retVal = SetSortValueForItem(retVal);
                }
            }
            
            // Return saved object.
            return retVal;
        }

        public VaultItem? UpdateVaultItem(string id, string encodedItem)
        {
            // The return value.
            VaultItem? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("edit");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(id);
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultItem>(_response, _settings);
                
                // Check to see if we have an object.
                if (retVal != null)
                {
                    // Update the sort value.
                    retVal = SetSortValueForItem(retVal);
                }
            }
            
            // Return saved object.
            return retVal;
        }

        public void DeleteVaultItem(string itemId)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("delete");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(itemId);

            // Execute.
            ExecuteCommand();
    
        }

        public VaultItem? MoveVaultItem(string id, string orgId, string encodedItem)
        {
            // The return value.
            VaultItem? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("move");
            _bwcli.StartInfo.ArgumentList.Add(id);
            _bwcli.StartInfo.ArgumentList.Add(orgId);
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item.
                retVal = JsonConvert.DeserializeObject<VaultItem>(_response, _settings);
                
                // Check to see if we have an object.
                if (retVal != null)
                {
                    // Update the sort value.
                    retVal = SetSortValueForItem(retVal);
                }
            }
            
            // Return saved object.
            return retVal;
        }
        #endregion
        
        #region Folder Write Method
        public VaultFolder? CreateFolder(string encodedItem)
        {
            // The return value.
            VaultFolder? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("create");
            _bwcli.StartInfo.ArgumentList.Add("folder");
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultFolder>(_response, _settings);
            }
            
            // Return saved object.
            return retVal;
        }
        #endregion
        
        #region Collection Write Method
        public VaultCollection? CreateCollection(string encodedItem)
        {
            // The return value.
            VaultCollection? retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("create");
            _bwcli.StartInfo.ArgumentList.Add("collection");
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultCollection>(_response, _settings);
            }
            
            // Return saved object.
            return retVal;
        }
        #endregion
        
        #region Generate Methods
        public string GeneratePassword(bool useUpper, bool useLower, bool useDigits, bool useSpecial, int length)
        {
            // The return value.
            string retVal = "";
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("generate");
            _bwcli.StartInfo.ArgumentList.Add("--length");
            _bwcli.StartInfo.ArgumentList.Add(length.ToString());
            
            // Check to see if we are using uppercase characters.
            if (useUpper)
            {
                _bwcli.StartInfo.ArgumentList.Add("--uppercase");
            }

            // Check to see if we are using lowercase characters.
            if (useLower)
            {
                _bwcli.StartInfo.ArgumentList.Add("--lowercase");
            }

            // Check to see if we are using numbers.
            if (useDigits)
            {
                _bwcli.StartInfo.ArgumentList.Add("--number");
            }

            // Check to see if we are using numbers.
            if (useSpecial)
            {
                _bwcli.StartInfo.ArgumentList.Add("--special");
            }

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set the response.
                retVal = _response;
            }

            // Return password.
            return retVal;
        }

        public string GeneratePassphrase(int numWords, string? sepChar, bool useUpper, bool useDigits)
        {
            // The return value.
            string retVal = "";

            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("generate");
            _bwcli.StartInfo.ArgumentList.Add("--passphrase");
            _bwcli.StartInfo.ArgumentList.Add("--words");
            _bwcli.StartInfo.ArgumentList.Add(numWords.ToString());
            _bwcli.StartInfo.ArgumentList.Add("--separator");
            _bwcli.StartInfo.ArgumentList.Add(sepChar ?? " ");
            
            // Check to see if we are capitalizing the first character of each word.
            if (useUpper)
            {
                _bwcli.StartInfo.ArgumentList.Add("--capitalize");
            }
            
            // Check to see if we are adding numbers to the words.
            if (useDigits)
            {
                _bwcli.StartInfo.ArgumentList.Add("--includeNumber");
            }
            
            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set the response.
                retVal = _response;
            }

            // Return password.
            return retVal;
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
        
        #region CLI Execute Wrapper
        private void ExecuteCommand()
        {
            // Reset command flags & response.
            _cmdExitCode = "";
            _response = "";
            _error = "";

            // Execute.
            _bwcli.Start();
            
            // Event starting.
            _bwcli.BeginErrorReadLine();
            _bwcli.BeginOutputReadLine();

            // Block until finished.
            _bwcli.WaitForExit();
            
            // Close streams.
            _bwcli.CancelErrorRead();
            _bwcli.CancelOutputRead();
            
            // Reset arguments.
            _bwcli.StartInfo.ArgumentList.Clear();
        }
        #endregion
        
        #region Event Handlers
        private void proxyCmd_DidExit(object? sender, EventArgs e)
        {
            // Flag that data has all been received.
            _cmdExitCode = _bwcli.ExitCode.ToString();
        }

        private void proxyCmd_DidReceiveError(object sender, DataReceivedEventArgs e)
        {
            _error += e.Data;
        }

        private void proxyCmd_DidReceiveData(object sender, DataReceivedEventArgs e)
        {
            // Append to response.
            _response += e.Data;
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
    }
}