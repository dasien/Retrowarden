using System.Collections;
using System.Text;
using Terminal.Gui;
using Terminal.Gui.Trees;
using Retrowarden.Dialogs;
using Retrowarden.Models;
using Retrowarden.Utils;
using Retrowarden.Repositories;
using Retrowarden.Config;
using Retrowarden.Workers;

namespace Retrowarden.Views 
{
    public partial class MainView 
    {
        // Vault repository reference.
        private readonly VaultRepository _vaultRepository;
        
        // Vault object collections.
        private SortedDictionary<string, VaultItem> _vaultItems;
        private List<VaultFolder> _folders;
        private List<VaultCollection> _collections;
        private List<Organization> _organizations;
        
        // Working variables.
        private readonly StringBuilder _aboutMessage;
        private VaultItem _tempItem;
        private bool _boomerMode;
        private bool _keepAlive;
       
         public MainView() 
        {
            // Initialize Application Stack
            Application.Init();
            
            // Set events for updating keep alive flag.
            Application.RootKeyEvent += HandleKeyEvent;
            Application.RootMouseEvent += HandleMouseEvent;
            
            // Create about message.
            _aboutMessage = ViewUtils.CreateAboutMessageAscii();
            
            // Get the configuration.
            RetrowardenConfig? config = ConfigurationManager.GetConfig();
            
            // Check to see if one was found.
            if (config != null)
            {
                // Check to see if exe location has been set.
                if (string.IsNullOrEmpty(config.CLILocation))
                {
                    // The allowed list. 
                    List<string> fileType = new List<string>();

                    // Show file dialog.
                    OpenDialog finder = new OpenDialog("Setup Retrowarden",
                        "Please locate the bw binary file to continue.", fileType);

                    finder.AllowsMultipleSelection = false;

                    Application.Run(finder);

                    if (!finder.Canceled)
                    {
                        // Check to see if a file was found.
                        if (finder.FilePath != null)
                        {
                            // Save exe location in config.
                            config.CLILocation = (string)finder.FilePath;
                            ConfigurationManager.WriteConfig(config);
                        }

                        else
                        {
                            // GTFO.
                            Environment.Exit(1);
                        }
                    }

                    else
                    {
                        // GTFO.
                        Environment.Exit(1);
                    }
                }

                // Try to get the exe location.
                string? bwExeLocation = config.CLILocation;

                // Make sure something was found.
                if (bwExeLocation != null)
                {
                    // Initialize vault repository.
                    _vaultRepository = new VaultRepository(bwExeLocation);
                }
                else
                {
                    MessageBox.ErrorQuery("Values Missing", "CLI Location not in config file.", "Ok");
                    Environment.Exit(1);
                }
            }

            else
            {
                MessageBox.ErrorQuery("Values Missing", "Configuration not found.", "Ok");
                Environment.Exit(1);
            }

            // Initialize member variables.
            _vaultItems = new SortedDictionary<string, VaultItem>();
            _folders = new List<VaultFolder>();
            _collections = new List<VaultCollection>();
            _organizations = new List<Organization>();
            _tempItem = new VaultItem();
            
            // Setup screen controls.
            InitializeComponent();
            
            // Set timer to launch splash.
            Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(80), ShowSplashScreen);
            
            // Set flag for vault lock timeout.
            _keepAlive = false;
            
            // Set timer to lock vault if no activity.
            Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(300000), LockVaultOnTimeout);

            // Run the application loop.
            Application.Run(this);
        }

        private bool ShowSplashScreen(MainLoop arg)
        {
            // Show splash screen.
            SplashDialog splash = new SplashDialog(_aboutMessage.ToString());
            splash.Show();

            return false;
        }
        
        private bool LockVaultOnTimeout(MainLoop arg)
        {
            // Check to see if the vault is already locked.
            if (!_vaultRepository.IsLocked)
            {
                // Check to see if the keep alive flag is set.
                if (!_keepAlive)
                {
                    // Check to see if we have a detail view open.
                    if (!this.IsCurrentTop)
                    {
                        // Get the detail view reference.
                        Toplevel top = Application.Current;
                        
                        // Close it.
                        Application.RequestStop(top);
                    }
                    
                    // Lock the vault.
                    HandleLockRequest();
                }
            }
            
            // In any case, reset the keep alive flag.
            _keepAlive = false;
            
            // Keep the timer running.
            return true;
        }
        
        private void SyncVault(bool fullSync)
        {
            // Check to see if this is a full or just display sync.
            if (fullSync)
            {
                // Create new worker.
                GetItemsWorker worker = new GetItemsWorker(_vaultRepository, "Syncing Vault Items...");
            
                // Execute task.
                worker.Run();
            
                // Get vault items.
                _vaultItems = worker.Items;
                
                // Check to see if items were found.
                if (_vaultRepository.ExitCode == "0")
                {
                    // Run workers for folders, organizations and collections.
                    GetFoldersWorker folderWorker = new GetFoldersWorker(_vaultRepository, "Syncing Folders...");
            
                    // Execute task.
                    folderWorker.Run();
            
                    // Get folders.
                    _folders = folderWorker.Folders;

                    // Create new worker.
                    GetCollectionsWorker collectionsWorker = new GetCollectionsWorker(_vaultRepository, "Syncing Collections...");
            
                    // Execute task.
                    collectionsWorker.Run();
            
                    // Get collections.
                    _collections = collectionsWorker.Collections;

                    // Create new worker.
                    GetOrganizationsWorker organizationsWorker = new GetOrganizationsWorker(_vaultRepository, "Syncing Organizations...");
            
                    // Execute task.
                    organizationsWorker.Run();
            
                    // Check to see if there are any orgs.
                    if (organizationsWorker.Organizations != null)
                    {
                        // Get organizations.
                        _organizations = organizationsWorker.Organizations;
                    }

                    // Check to see if there are any organizations.
                    if (_organizations != null && _organizations.Count > 0)
                    {
                        // Get each org.
                        foreach (Organization org in _organizations)
                        {
                            // Create new worker.
                            GetOrganizationMembersWorker membersWorker = new GetOrganizationMembersWorker(
                                _vaultRepository, "Syncing Members for " + org.Name + " ...", org.Id);
                            
                            // Execute search.
                            membersWorker.Run();
                            
                            // Check to see if there are any results.
                            if (membersWorker.Members != null)
                            {
                                // Save result.
                                org.Members = membersWorker.Members;
                            }
                        }
                    }
                }
            }
            
            // Set item owner name.
            SetOwnerNameForItems();

            // Load controls with data.
            LoadTreeView();
            LoadItemListView(_vaultItems);
        }
        
        private void ShowDetailForm(VaultItemDetailViewState state)
        {
            ItemDetailView? view = CreateDetailView(state);
            
            // Make sure we have a view.
            if (view != null)
            {
                // Show the view modal.
                view.Modal = true;

                // This fixes a bug where the background was not drawing correctly.
                Application.Refresh();

                // Show the view.
                view.Show();

                // Check to see if there is anything to do.
                if (view.OkPressed)
                {
                    // Create list to hold item.
                    List<VaultItem> items = [view.Item];

                    switch (state)
                    {
                        case VaultItemDetailViewState.Create:
                            // Run the save worker.
                            RunSaveItemWorker(items, VaultItemSaveAction.Create, "Creating new vault item.");
                            break;

                        case VaultItemDetailViewState.Edit:
                            // Run the save worker.
                            RunSaveItemWorker(items, VaultItemSaveAction.Update, "Updating vault item.");
                            break;

                        case VaultItemDetailViewState.View:
                        default:
                            break;
                    }
                }

                else
                {
                    // Get the item data source.
                    ItemListDataSource items = (ItemListDataSource)lvwItems.Source;

                    // Check to see if there are any items.
                    if (items != null && items.Count > 0)
                    {
                        // Get the current item.
                        _tempItem = items.ItemList[lvwItems.SelectedItem];
                    }
                }
            }
        }

        private ItemDetailView? CreateDetailView(VaultItemDetailViewState state)
        {
            ItemDetailView? retVal = null;
            
            // Check to see what type of item we have.
            switch (_tempItem.ItemType)
            {
                // Login
                case 1:
                    retVal = new LoginDetailView(_tempItem, _folders, state, _vaultRepository);
                    break;
                
                // Note
                case 2:
                    retVal = new SecureNoteDetailView(_tempItem, _folders, state);
                    break;
                
                // Card
                case 3:
                    retVal = new CardDetailView(_tempItem, _folders, state);
                    break;
                
                // Identity
                case 4:
                    retVal = new IdentityDetailView(_tempItem, _folders, state);
                    break;
            }
            
            // Return the view.
            return retVal;
        }

        #region UI Control Helpers
        private void LoadItemListView(SortedDictionary<string, VaultItem> items)
        {
            // Clear out any existing items.
            lvwItems.Source = null;
            
            // Check to see if there are any items to show.
            if (items.Count > 0)
            {
                // Get list of vault items from dictionary.
                List<VaultItem> itemList = items.Values.ToList();
            
                // Create list data source for listview.
                ItemListDataSource listSource = new ItemListDataSource(itemList);
            
                // Create handler for the OnSetMark event.
                listSource.OnSetMark += HandleListviewItemMarked;
            
                // Set the data to the listview.
                lvwItems.Source = (listSource);
            
                // Enable visibility of column header labels.
                lblItemName.Visible = true;
                lblUserId.Visible = true;
                lblOwner.Visible = true;
            
                // Set the first row as the selected row.
                lvwItems.SelectedItem = 0;
                lvwItems.SetFocus();
            }
            
            // Redraw listview.
            lvwItems.SetNeedsDisplay();
            
            // Set statusbar menus.
            UpdateStatusBarOptions();
        }
        private void LoadTreeView()
        {
            // Clear out any items.
           tvwItems.ClearObjects();
            
            // Create root node.
            TreeNode root = new TreeNode("Bitwarden")
            {
                Tag = new NodeData()
                {
                    Id= null, NodeType = NodeType.Root, Parent = null, Text = null
                }
            };
            
            // Create personal vault node.
            TreeNode personal = new TreeNode("My Vault")
            {
                Tag = new NodeData()
                {
                    Id= null, NodeType = NodeType.Organization, Parent = root, Text = null
                }
            };
            
            // Check to see if there are any items.
            if (_vaultItems.Count > 0)
            {
                // Create item nodes.
                TreeNode itemNodes = ViewUtils.CreateAllItemsNodes(_vaultItems, personal, null);
        
                // Add nodes to root.
                personal.Children.Add(itemNodes);
            }

            // Check to see if there are any folders.
            if (_folders.Count > 0)
            {
                // Create folders node.
                TreeNode folderNode = ViewUtils.CreateFoldersNode(_folders, _vaultItems, personal, null);
                
                // Add nodes to root.
                personal.Children.Add(folderNode);
            }
            
            // Add personal vault to root.
            root.Children.Add(personal);
            
            // Loop through the organizations.
            foreach (Organization org in _organizations)
            {
                // Create a tree node for this org/vault.
                TreeNode orgVault = new TreeNode(org.Name)
                {
                    Tag = new NodeData()
                    {
                        Id = org.Id, NodeType = NodeType.Organization, Parent = root, Text = org.Name
                    }
                };
                
                // Check to see if there are any items.
                if (_vaultItems.Count > 0)
                {
                    // Create item nodes.
                    TreeNode itemNodes = ViewUtils.CreateAllItemsNodes(_vaultItems, orgVault, org);

                    // Add nodes to root.
                    orgVault.Children.Add(itemNodes);
                }
                
                // Check to see if there are any folders.
                if (_folders.Count > 0)
                {
                    // Create folders node.
                    TreeNode folderNode = ViewUtils.CreateFoldersNode(_folders, _vaultItems, orgVault, org);
                
                    // Add nodes to root.
                    orgVault.Children.Add(folderNode);
                }
            
                // Check to see if there are any collections.
                if (_collections.Count > 0)
                {
                    // Create collection node.
                    TreeNode collectionNode = ViewUtils.CreateCollectionsNode(_collections, _vaultItems, orgVault, org);

                    // Add nodes to root.
                    orgVault.Children.Add(collectionNode);
                }
                
                // Add org to root.
                root.Children.Add(orgVault);
            }
            
            // Add nodes to control.
            this.tvwItems.AddObject(root);
        }
        
        private SortedDictionary<string, VaultItem> GetVaultItemsForTreeNode(ITreeNode node)
        {
            SortedDictionary<string, VaultItem> retVal = new SortedDictionary<string, VaultItem>();
            
            // Loop through child nodes.
            foreach (ITreeNode child in node.Children)
            {
                // Get node tag.
                NodeData nodeData = (NodeData) child.Tag;
                
                // Check to see that the child node is a vault item.
                if (nodeData.NodeType == NodeType.Item)
                {
                    // Check to see if the id is present.
                    if (nodeData.Id != null)
                    {
                        // Lookup node in item dictionary.
                        VaultItem item = _vaultItems[nodeData.Id];

                        // Add to filtered list.
                        retVal.Add(item.Id, item);
                    }
                }
            }
            
            // Return filtered list.
            return retVal;
        }

        private void SetOwnerNameForItems()
        {
            // Check to see if there are any organizations.
            if (_organizations.Count > 0)
            {
                // Loop through organizations.
                foreach (Organization org in _organizations)
                {
                    // Find each item that belongs to that org.
                    List<VaultItem> orgItems = _vaultItems.Values.Where(i => i.OrganizationId == org.Id).ToList();

                    // Update the owner name.
                    orgItems.ForEach(i => i.ItemOwnerName = org.Name);
                }
            }

            // Check to see if there are any vault items.
            if (_vaultItems.Count > 0)
            {
                // Find items with no org.
                List<VaultItem> userItems = _vaultItems.Values.Where(i => i.OrganizationId == null).ToList();

                // Update to show user is the owner.
                userItems.ForEach(i => i.ItemOwnerName = "Me");
            }
        }
        
        private void UpdateStatusBarOptions()
        {
            // Check to see if we have any items.
            if (_vaultItems.Count > 0)
            {
                // Check to see if the collection move should be active.
                bool colMoveOk = ShouldShowCollectionMoveMenu();
                
                // Check to see if the copy user/password items should be active.
                bool copyOk = ShouldShowCopyMenu();
                
                // Check to see that there is a list item source.
                if (lvwItems.Source != null)
                {
                    // Get the data source for list.
                    ItemListDataSource items = (ItemListDataSource) lvwItems.Source;

                    // Check to see how many rows have been selected.
                    switch (items.MarkedItemCount)
                    {
                        // If 0 or 1, enable single item menu items.
                        case 0:
                        case 1:

                            // Only add collection move and copy options if appropriate.
                            if (colMoveOk && copyOk)
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiNew, stiView, stiEdit, stiCopyUser, stiCopyPwd, 
                                    stiFolderMove, stiCollectionMove, stiDelete];
                            }

                            else if (copyOk)
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiNew, stiView, stiEdit, stiCopyUser, 
                                    stiCopyPwd, stiFolderMove, stiDelete];
                            }
                            
                            else if (colMoveOk)
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiNew, stiView, stiEdit,  
                                    stiFolderMove, stiCollectionMove, stiDelete];
                            }

                            else
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiNew, stiView, stiEdit, stiFolderMove, stiDelete];
                            }
                            break;

                        // If > 1, enable only bulk menu items (add to folder/collection).
                        default:
                            
                            // Only add collection move option if it is allowed.
                            if (colMoveOk)
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiFolderMove, stiCollectionMove];
                            }

                            else
                            {
                                // Add them to the status bar.
                                staMain.Items = [stiFolderMove];
                            }

                            break;
                    }
                }

                else
                {
                    staMain.Items = [stiNew];
                }
            }

            else
            {
                staMain.Items = [stiNew];
            }

            // Redraw menu bar.
            staMain.SetNeedsDisplay();
        }

        private bool ShouldShowCopyMenu()
        {
            // The return value.
            bool retVal = false;
                        
            // Check to see that there is a source list.
            if (lvwItems.Source != null)
            {
                // Get the data source for list.
                ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
                IList itemList = items.ToList();
                
                // Check to see if anything has been marked.
                if (items.MarkedItemCount == 0)
                {
                    // Get the currently selected item.
                    VaultItem? item = (VaultItem?)itemList[lvwItems.SelectedItem];

                    // Check to make sure we have an item.
                    if (item != null)
                    {
                        // Check to see if this item is already in a collection.
                        retVal = (item.ItemType == 1);
                    }
                }

                else if (items.MarkedItemCount == 1)
                {
                    // Get the marked item list from the listview.
                    List<VaultItem> markedItems = ((ItemListDataSource)lvwItems.Source).MarkedItemList;

                    // Get the currently selected item.
                    VaultItem item = markedItems[0];
                    
                    // Check to see if this item is already in a collection.
                    retVal = (item.ItemType == 1);
                }

                else
                {
                    retVal = false;
                }
            }

            else
            {
                retVal = false;
            }
            
            // Return the result.
            return retVal;
        }
        
        private bool ShouldShowCollectionMoveMenu()
        {
            // The return value.
            bool retVal = true;
            
            // Check to see if this is an 'org enabled' account.
            if (_vaultRepository.IsOrgEngabled)
            {
                // Check to see if there are vault items.
                if (_vaultItems.Count > 0)
                {
                    // Check to see that there is a source list.
                    if (lvwItems.Source != null)
                    {
                        // Get the data source for list.
                        ItemListDataSource items = (ItemListDataSource)lvwItems.Source;

                        // Check to see if anything has been marked.
                        if (items.MarkedItemCount == 0)
                        {
                            // Get the currently selected item.
                            VaultItem item = _vaultItems.ElementAt(lvwItems.SelectedItem).Value;

                            // Check to see if this item is already in a collection.
                            retVal = (item.CollectionIds != null && item.CollectionIds.Count == 0);
                        }

                        else
                        {
                            // Get the marked item list from the listview.
                            List<VaultItem> markedItems = ((ItemListDataSource)lvwItems.Source).MarkedItemList;

                            // Save the marked items (ignoring the currently selected item if it is not marked)
                            foreach (VaultItem item in markedItems)
                            {
                                if (item.CollectionIds != null && item.CollectionIds.Count > 0)
                                {
                                    retVal = false;
                                    break;
                                }
                            }
                        }
                    }

                    else
                    {
                        retVal = false;
                    }
                }

                else
                {
                    retVal = false;
                }
            }

            else
            {
                retVal = false;
            }

            // Return the result.
            return retVal;
        }
        #endregion
        
        #region Main View Handlers
        private void HandleMouseEvent(MouseEvent obj)
        {
            // Flag that the user has done something.
            _keepAlive = true;
        }

        private bool HandleKeyEvent(KeyEvent arg)
        {
            // Flag that the user has done something.
            _keepAlive = true;
            
            // Allow other handlers to get key events.
            return false;
        }
        #endregion
        
        #region Top Menu Handlers
        private void HandleConnectionRequest()
        {
            // Check to make sure we are not logged in already.
            if (_vaultRepository.IsLoggedIn)
            {
                MessageBox.ErrorQuery("Action failed","You are already logged in.", "Ok");
            }

            else
            {
                // Create connection dialog.
                ConnectDialog connectDialog = new ConnectDialog();
                
                // Show it.
                connectDialog.Show();
                
                // Check to see if the user wants to try to log in.
                if (connectDialog.OkPressed)
                {
                    // Create new worker.
                    LoginWorker worker = new LoginWorker(_vaultRepository, connectDialog.UserId, 
                        connectDialog.Password, "Logging in...");
            
                    // Execute task
                    worker.Run();

                    // Check to see if the login was successful.
                    if (_vaultRepository.ExitCode == "0")
                    {
                        // Run full sync.
                        SyncVault(true);
                    }

                    else
                    {
                        // Show error dialog.
                        MessageBox.ErrorQuery("Login failed.", _vaultRepository.ErrorMessage, "Ok");
                    }
                }
            }
        }

        private void HandleQuitRequest()
        {
            int response = MessageBox.Query("Confirm Action", "Quit.  Are you Sure?", "Ok", "Cancel");
            
            // Check to see if the user confirmed action.
            if (response == 0)
            {
                // Create new worker.
                LogoutWorker worker = new LogoutWorker(_vaultRepository, "Logging out...");
            
                // Execute task.
                worker.Run();
                
                // Clear any clipboard contents.
                Clipboard.TrySetClipboardData("");
                
                // Close application.
                Application.RequestStop(this);
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void HandleFolderCreate()
        {
            // Check to make sure we are logged in.
            if (_vaultRepository.IsLoggedIn)
            {
                // Create add folder dialog.
                AddFolderDialog dialog = new AddFolderDialog(_folders);
                
                // Show it.
                dialog.Show();
                
                // Check to see if the user pressed Ok.
                if (dialog.OkPressed)
                {
                    // Check to see that we have a folder name.
                    if (dialog.FolderName != null)
                    {
                        // Create folder.
                        VaultFolder folder = new VaultFolder()
                        {
                            Name = dialog.FolderName
                        };
            
                        // Create the worker.
                        CreateFolderWorker worker = new CreateFolderWorker(_vaultRepository, 
                            "Adding Folder", folder);
            
                        // Run the worker.
                        worker.Run();
                        
                        // Check to see if the login was successful.
                        if (_vaultRepository.ExitCode == "0")
                        {
                            // Add the folder to the list.
                            _folders.Add(worker.Folder);

                            // Update folders.
                            LoadTreeView();
                        }

                        else
                        {
                            // Show error dialog.
                            MessageBox.ErrorQuery("Add folder failed.", _vaultRepository.ErrorMessage, "Ok");
                        }
                    }
                }
            }

            else
            {
                MessageBox.ErrorQuery("Action failed","You must be logged in.", "Ok");
            }
        }

        private void HandleCollectionCreate()
        {
            // Check to make sure we are logged in.
            if (_vaultRepository.IsLoggedIn)
            {
                // Check to see if this is an 'org enabled' account.
                if (_vaultRepository.IsOrgEngabled)
                {
                    // Create add collection dialog.
                    AddCollectionDialog dialog = new AddCollectionDialog(_organizations, _collections);

                    // Show it.
                    dialog.Show();

                    // Check to see if the user pressed Ok.
                    if (dialog.OkPressed)
                    {
                        // Check to see that we have a collection name.
                        if (dialog.CollectionName != null)
                        {
                            // Create collection.
                            VaultCollection collection = new VaultCollection()
                            {
                                Name = dialog.CollectionName
                            };

                            // Create the worker.
                            CreateCollectionWorker worker = new CreateCollectionWorker(_vaultRepository,
                                "Adding Collection", collection);

                            // Run the worker.
                            worker.Run();
                            
                            // Check to see if the login was successful.
                            if (_vaultRepository.ExitCode == "0")
                            {
                                // Add the collection to the list.
                                _collections.Add(worker.Collection);

                                // Update folders.
                                LoadTreeView();
                            }

                            else
                            {
                                // Show error dialog.
                                MessageBox.ErrorQuery("Add collection failed.", _vaultRepository.ErrorMessage, "Ok");
                            }
                        }
                    }
                }

                else
                {
                    // Notify user.
                    MessageBox.ErrorQuery("Action failed","Collections not available for this account.", "Ok");
                }
            }
            
            else
            {
                // Notify user.
                MessageBox.ErrorQuery("Action failed", "You must be logged in.", "Ok");
            }
        }

        private void HandlePasswordGenerate()
        {
            // Create password generator dialog.
            GeneratePasswordDialog genPass = new GeneratePasswordDialog(_vaultRepository);
            genPass.Show();
        }

        private void HandlePassphraseGenerate()
        {
            // Create passphrase generator dialog.
            GeneratePassphraseDialog genPass = new GeneratePassphraseDialog(_vaultRepository);
            genPass.Show();
        }
        
        private void HandleBoomerMode()
        {
            // Toggle boomer mode.
            _boomerMode = !_boomerMode;
            
            // Set menu state.
            mnuMain.Menus[1].Children[0].Checked = _boomerMode;
            
            // Set this to the desired state
            Application.IsMouseDisabled = _boomerMode;
        }

        private void HandleSyncRequest()
        {
            // Check to see if we are in the right state.
            if (_vaultRepository is { IsLoggedIn: true, IsLocked: false })
            {
                // Re-sync vault.
                SyncVault(true);
            }

            else
            {
                // Notify user.
                MessageBox.ErrorQuery("Action failed","You must be logged in with an unlocked vault to sync.", "Ok");
            }
        }

        private void HandleLockRequest()
        {
            // Check to see if the user is logged in.
            if (_vaultRepository.IsLoggedIn)
            {
                // Show the dialog.
                LockWorker worker = new LockWorker(_vaultRepository, "Locking vault...");

                // Execute task.
                worker.Run();

                // Clear controls.
                lvwItems.RemoveAll();
                tvwItems.ClearObjects();
                lvwItems.Source = null;
                lblItemName.Visible = false;
                lblUserId.Visible = false;
                lblOwner.Visible = false;

                // Reset internal collections.
                _folders.Clear();
                _collections.Clear();
                _organizations.Clear();
                _vaultItems.Clear();

                // Reset status bar menu options.
                UpdateStatusBarOptions();
            }

            else
            {
                // Notify user.
                MessageBox.ErrorQuery("Action failed","You must be logged in.", "Ok");

            }
        }

        private void HandleUnlockRequest()
        {
            // Check to make sure we are not logged in already.
            if (_vaultRepository is { IsLoggedIn: true, IsLocked: true })
            {
                // Create connection dialog.
                UnlockDialog unlockDialog = new UnlockDialog();
                
                // Show it.
                unlockDialog.Show();
                
                // Check to see if the user wants to try to log in.
                if (unlockDialog.OkPressed)
                {
                    // Show the dialog.
                    UnlockWorker worker = new UnlockWorker(_vaultRepository, unlockDialog.Password, "Unlocking Vault...");

                    // Execute task.
                    worker.Run();

                    // Check to see if the login was successful.
                    if (_vaultRepository.ExitCode == "0")
                    {
                        // Run full sync.
                        SyncVault(true);
                    }
                }
            }

            else
            {                
                MessageBox.ErrorQuery("Action failed","You must be logged in and the vault must be locked.", "Ok");
            }
        }
        
        private void HandleUpdateCheck()
        {
            // Create new worker.
            CheckForUpdateWorker worker = new CheckForUpdateWorker(_vaultRepository, "Checking for Update...");
            
            // Run worker
            worker.Run();
            
            // Get the result.
            bool result = worker.UpdateAvailable;
            
            // Check to see if update is available.
            if (result)
            {
                // Show a message.
                MessageBox.Query("Update Available", "There is an updated CLI version available.", "Ok");
            }

            else
            {
                // Show a message.
                MessageBox.Query("No Update Available", "You are on the current CLI version.", "Ok");
            }
        }
        
        private void HandleStatusCheck()
        {
            // Create new worker.
            CheckStatusWorker worker = new CheckStatusWorker(_vaultRepository, "Checking Status...");
            
            // Run worker
            worker.Run();
            
            // Get the result.
            VaultStatus result = worker.Status;
            
            // Show the status dialog with information.
            StatusDialog dialog = new StatusDialog(result);
            
            // Show it.
            dialog.Show();
        }
        #endregion

        #region Treeview Event Handlers
        private void HandleTreeviewSelectionChanged(object? sender, SelectionChangedEventArgs<ITreeNode> e)
        {
            // Check to make sure the value isn't null.
            if (e.NewValue != null)
            {
                // Get node that was selected.
                ITreeNode selected = e.NewValue;

                // Get the node data for this node.
                NodeData nodeData = (NodeData) selected.Tag;

                // Check to see if this node has children.
                if (nodeData.NodeType != NodeType.Item)
                {
                    // Get the list of children.
                    SortedDictionary<string, VaultItem> list = GetVaultItemsForTreeNode(selected);

                    // Update tableview with scoped list.
                    LoadItemListView(list);
                }
            }
        }

        private void HandleTreeviewNodeActivated(ObjectActivatedEventArgs<ITreeNode> obj)
        {
            // Get the node that was double-clicked.
            ITreeNode activated = obj.ActivatedObject;
            
            // Get the node data for this node.
            NodeData nodeData = (NodeData) activated.Tag;
            
            // Make sure this is a leaf node.
            if (nodeData.NodeType == NodeType.Item)
            {
                // Check to see if we have an id.
                if (nodeData.Id != null)
                {
                    // Update the selected item.
                    _tempItem = _vaultItems[nodeData.Id];

                    // Call the detail form show.
                    ShowDetailForm(VaultItemDetailViewState.Edit);
                }
            }
        }
        #endregion
        
        #region Statusbar Menu Handlers
        private void HandleCreateKeypress()
        {
            // Check to see if we are in the right state.
            if (_vaultRepository is { IsLoggedIn: true, IsLocked: false })
            { 
                // Create new temp item.
                _tempItem = new VaultItem();
                    
                // Try to establish context from the tree view.
                if (tvwItems.SelectedObject != null)
                {
                    // Get the node which is selected.
                    ITreeNode node = tvwItems.SelectedObject;

                    // Check the node type from tag.
                   NodeData nodeData = (NodeData)node.Tag;

                    // Check to see if it is an item group.
                    if (nodeData.NodeType == NodeType.ItemGroup)
                    {
                        // Based on the string we know what type of item to create.
                        switch (node.Text)
                        {
                            case "Logins":
                                _tempItem.ItemType = 1;
                                break;
                            case "Secure Notes":
                                _tempItem.ItemType = 2;
                                break;
                            case "Cards":
                                _tempItem.ItemType = 3;
                                break;
                            case "Identities":
                                _tempItem.ItemType = 4;
                                break;
                        }
                    }

                    else if (nodeData.NodeType == NodeType.Item)
                    {
                        // Check to see if we have an id.
                        if (nodeData.Id != null)
                        {
                            // Get item.
                            _tempItem = _vaultItems[nodeData.Id];
                        }
                    }

                    else
                    {
                        // Have the user pick the item type.
                        SelectItemTypeDialog dialog = new SelectItemTypeDialog();
                        dialog.Show();

                        if (dialog.OkPressed)
                        {
                            _tempItem.ItemType = dialog.ItemType + 1;
                        }

                        else
                        {
                            // Just default to Login.
                            _tempItem.ItemType = 1;
                        }
                    }
                }
                
                else
                {
                    // Have the user pick the item type.
                    SelectItemTypeDialog dialog = new SelectItemTypeDialog();
                    dialog.Show();

                    if (dialog.OkPressed)
                    {
                        _tempItem.ItemType = dialog.ItemType + 1;
                    }

                    else
                    {
                        // Just default to Login.
                        _tempItem.ItemType = 1;
                    }
                }
                
                ShowDetailForm(VaultItemDetailViewState.Create);
            }

            else
            {
                // Notify user.
                MessageBox.ErrorQuery("Action failed","You must be logged in with an unlocked vault to sync.", "Ok");
            }
        }
        private void HandleViewItemKeypress()
        {
            // Call helper method.
            ShowDetailForm(VaultItemDetailViewState.View);
        }
        
        private void HandleEditItemKeypress()
        {
            ShowDetailForm(VaultItemDetailViewState.Edit);
        }
        private void HandleUserCopyKeypress()
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            VaultItem item = items.ItemList[lvwItems.SelectedItem];
            
            // Check to see that there is a login.
            if (item.Login != null)
            {
                // Copy username to clipboard.
                Clipboard.TrySetClipboardData(item.Login.UserName);
            }

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Username copied to clipboard.", "Ok");
        }
        
        private void HandlePwdCopyKeypress()
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            VaultItem item = items.ItemList[lvwItems.SelectedItem];

            // Check to see that there is a login.
            if (item.Login != null)
            {
                // Copy password to clipboard.
                Clipboard.TrySetClipboardData(item.Login.Password);
            }
            
            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");
        }
        
        private void HandleOrganizationMoveKeypress()
        {
            // Create collection list dialog
            SelectOrganizationAndCollectionDialog sc = new SelectOrganizationAndCollectionDialog(_organizations, _collections);
            sc.Show();
            
            // Check to see what button was pressed.
            if (sc.OkPressed)
            {
                // Get the marked item list from the listview.
                List<VaultItem> markedItems = ((ItemListDataSource)lvwItems.Source).MarkedItemList;
                
                // Check to see if there are any marked items.
                if (markedItems.Count > 0)
                {
                    // Save the marked items (ignoring the currently selected item if it is not marked)
                    foreach (VaultItem item in markedItems)
                    {
                        // Check to see that we have an org.
                        if (sc.SelectedOrganization != null)
                        {
                            // Update the organization.
                            item.OrganizationId = sc.SelectedOrganization.Id;
                        }

                        // Check to see if the item has a collection list.
                        item.CollectionIds ??= new List<string>();
                        
                        // Loop through selected collections.
                        foreach (VaultCollection collection in sc.SelectedCollections)
                        {
                            // Update the collection.
                            item.CollectionIds.Add(collection.Id);
                        }
                    }
                    
                    // Run item save worker.
                    RunSaveItemWorker(markedItems, VaultItemSaveAction.MoveToOrganization, "Adding to collection.");
                }

                else
                {
                    // Create a list to hold the selected item.
                    List<VaultItem> toSave = new List<VaultItem>();
                    
                    // Get the item data source.
                    ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
                    // Get the current item.
                    VaultItem item = items.ItemList[lvwItems.SelectedItem];

                    // Check to see that we have an org.
                    if (sc.SelectedOrganization != null)
                    {
                        // Update the organization.
                        item.OrganizationId = sc.SelectedOrganization.Id;
                    }

                    // Check to see if the item has a collection list.
                    item.CollectionIds ??= new List<string>();

                    // Loop through selected collections.
                    foreach (VaultCollection collection in sc.SelectedCollections)
                    {
                        // Update the collection.
                        item.CollectionIds.Add(collection.Id);
                    }

                    // Add item to list.
                    toSave.Add(item);
                    
                    // Just save the currently selected item.
                    RunSaveItemWorker(toSave, VaultItemSaveAction.MoveToOrganization, "Adding to collection.");
                }
            }
        }

        private void HandleFolderMoveKeypress()
        {
            // Create folder list dialog
            SelectFolderDialog sfDialog = new SelectFolderDialog(_folders);
            sfDialog.Show();

            if (sfDialog.OkPressed)
            {
                // Get folder.
                VaultFolder folder = _folders[sfDialog.SelectedFolderIndex];
                
                // Get the marked item list from the listview.
                List<VaultItem> markedItems = ((ItemListDataSource)lvwItems.Source).MarkedItemList;
                
                // Check to see if there are any marked items.
                if (markedItems.Count > 0)
                {
                    // Save the marked items (ignoring the currently selected item if it is not marked)
                    foreach (VaultItem item in markedItems)
                    {
                        // Update the folder.
                        item.FolderId = folder.Id;
                    }
                    
                    // Run item save worker.
                    RunSaveItemWorker(markedItems, VaultItemSaveAction.MoveToFolder, "Moving to folder.");
                }

                else
                {
                    // Create a list to hold the selected item.
                    List<VaultItem> toSave = new List<VaultItem>();
                    
                    // Get the item data source.
                    ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
                    // Get the current item.
                    VaultItem item = items.ItemList[lvwItems.SelectedItem];
                    
                    // Update the folder.
                    item.FolderId = folder.Id;

                    // Add item to list.
                    toSave.Add(item);
                    
                    // Just save the currently selected item.
                    RunSaveItemWorker(toSave, VaultItemSaveAction.MoveToFolder, "Moving to folder.");
                }
            }
        }

        private void HandleDeleteKeypress()
        {
            // Show dialog confirming action.
            int pressed = MessageBox.ErrorQuery("Confirm Action", "Delete Item. Are you sure?", "Yes", "No");

            // Check to see if there is anything to do.
            if (pressed == 0)
            {
                // Create a list to hold the selected item.
                List<VaultItem> toDelete = new List<VaultItem>();
                    
                // Get the item data source.
                ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
                // Get the current item.
                VaultItem item = items.ItemList[lvwItems.SelectedItem];
                    
                // Add item to list.
                toDelete.Add(item);
                
                // Run the delete worker.
                RunSaveItemWorker(toDelete, VaultItemSaveAction.Delete, "Deleting vault item.");
            }
        }
        #endregion

        #region Listview Event Handlers
        private void HandleListViewOpenItem(ListViewItemEventArgs obj)
        {
            ShowDetailForm(VaultItemDetailViewState.Edit);
        }
        
        private void HandleListviewItemMarked(object? sender, EventArgs e)
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            _tempItem = items.ItemList[lvwItems.SelectedItem];
            
            // Update the status bar options.
            UpdateStatusBarOptions();
        }

        private void HandleListViewSelectedItemChanged(ListViewItemEventArgs obj)
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            _tempItem = items.ItemList[lvwItems.SelectedItem];
            
            // Update the status menu.
            UpdateStatusBarOptions();
        }
        #endregion
        
        #region Save Item Worker
        private void RunSaveItemWorker(List<VaultItem>items , VaultItemSaveAction saveType, string? dialogMessage)
        {
            SaveItemWorker worker = new SaveItemWorker(_vaultRepository, saveType, items, dialogMessage);
            
            // Run the worker.
            worker.Run();
            
            // Post save actions.
            switch (saveType)
            {
                case VaultItemSaveAction.Create:
                
                    // For create, put new item in item list.
                    _vaultItems.Add(worker.Results[0].Id, worker.Results[0]);
                    break;
                
                case VaultItemSaveAction.Update:
                        
                    // For update, loop through items to replace existing items in list.
                    foreach (VaultItem item in worker.Results)
                    {
                        // Replace existing item.
                        _vaultItems[item.Id] = item;
                    }
                    break;
                
                case VaultItemSaveAction.Delete:
                    
                    // For delete, remove item from list.
                    _vaultItems.Remove(worker.Results[0].Id);
                    break;
            } 
            
            // Update controls with new source data.
            ITreeNode node = tvwItems.SelectedObject;
            
            // Update the main controls.
            SyncVault(false);
            
            // Reset the current node.
            tvwItems.SelectedObject = node;
            tvwItems.Expand();
            tvwItems.SetNeedsDisplay();
        }
        #endregion
    }
}
