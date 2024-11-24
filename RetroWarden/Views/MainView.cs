using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Terminal.Gui;
using Retrowarden.Dialogs;
using RetrowardenSDK.Models;
using Retrowarden.Utils;
using RetrowardenSDK.Repositories;
using Retrowarden.Config;
using Retrowarden.Workers;

namespace Retrowarden.Views 
{
    public partial class MainView
    {
        // Vault repository reference.
        private IVaultRepository _vaultRepository;

        // Vault object collections.
        private SortedDictionary<string, VaultItem> _vaultItems;
        private List<VaultFolder> _folders;
        private List<VaultCollection> _collections;
        private List<Organization> _organizations;

        // Working variables.
        private readonly StringBuilder _aboutMessage;
        private VaultItem _tempItem;
        private bool _boomerMode;
        private bool _debugMode;
        private bool _keepAlive;
        private ITreeNode? _selectedNode;
        private string? _currentOrg;
        private bool _sortDescending;
        private bool _sortValueDescending;
        private bool _sortOwnerDescending;
        private int _sortColumn = 0;

        public MainView(bool debug)
        {
            // Initialize Application Stack
            Application.Init();

            // Set events for updating keep alive flag.
            Application.KeyDown += HandleKeyEvent;
            Application.MouseEvent += HandleMouseEvent;

            // Create about message.
            _aboutMessage = ViewUtils.CreateAboutMessageAscii();

            // Check to see if we are in debug mode.
            if (debug)
            {
                _vaultRepository = new DebugVaultRepository();
            }

            else
            {
                // Assign location.
                string? bwExeLocation = LocateCLI();

                // Make sure something was found.
                if (bwExeLocation != null)
                {
                    // Initialize vault repository.
                    _vaultRepository = new VaultRepository(bwExeLocation);
                }
            }

            // Initialize member variables.
            _vaultItems = new SortedDictionary<string, VaultItem>();
            _folders = new List<VaultFolder>();
            _collections = new List<VaultCollection>();
            _organizations = new List<Organization>();
            _tempItem = new VaultItem();
            _selectedNode = null;
            _currentOrg = null;
            _sortDescending = false;
            _sortValueDescending = false;
            _sortOwnerDescending = false;

            // Setup screen controls.
            InitializeComponent();

            // Set timer to launch splash.
            Application.AddTimeout(TimeSpan.FromMilliseconds(80), ShowSplashScreen);

            // Set flag for vault lock timeout.
            _keepAlive = false;

            // Set timer to lock vault if no activity.
            Application.AddTimeout(TimeSpan.FromMilliseconds(300000), LockVaultOnTimeout);

            // Run the application loop.
            Application.Run(this);
        }

        private bool ShowSplashScreen()
        {
            // Show splash screen.
            SplashDialog splash = new SplashDialog(_aboutMessage.ToString());
            splash.Show();

            return false;
        }

        private bool LockVaultOnTimeout()
        {
            // Check to see if the vault is already locked.
            if (!_vaultRepository.IsLocked)
            {
                // Check to see if the keep alive flag is set.
                if (!_keepAlive)
                {
                    // Check to see if we have a detail view open.
                    if (!IsCurrentTop)
                    {
                        // Get the detail view reference.
                        Toplevel? top = Application.Top;

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
                    GetCollectionsWorker collectionsWorker =
                        new GetCollectionsWorker(_vaultRepository, "Syncing Collections...");

                    // Execute task.
                    collectionsWorker.Run();

                    // Get collections.
                    _collections = collectionsWorker.Collections;

                    // Create new worker.
                    GetOrganizationsWorker organizationsWorker =
                        new GetOrganizationsWorker(_vaultRepository, "Syncing Organizations...");

                    // Execute task.
                    organizationsWorker.Run();

                    // Check to see if there are any organization.
                    if (organizationsWorker.Organizations != null)
                    {
                        // Get organizations.
                        _organizations = organizationsWorker.Organizations;
                    }
/*
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
*/
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

        private void ResetUI()
        {
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

        private string? LocateCLI()
        {
            // The location of CLI file.
            string? retVal = null;
    
            // Get the configuration.
            RetrowardenConfig? config = Config.ConfigurationManager.GetConfig();
    
            // Check to see if one was found.
            if (config != null)
            {
                // Check to see if exe location has been set.
                if (string.IsNullOrEmpty(config.CLILocation))
                {
                    // Show file dialog.
                    OpenDialog finder = new OpenDialog()
                    {
                        Title = "Setup Retrowarden", Text = "Please locate the bw binary file to continue."
                    };
    
                    finder.AllowsMultipleSelection = false;
    
                    Application.Run(finder);
    
                    if (!finder.Canceled)
                    {
                        // Check to see if a file was found.
                        if (finder.FilePaths != null)
                        {
                            // Save exe location in config.
                            config.CLILocation = (string)finder.FilePaths.Single();
                            Config.ConfigurationManager.WriteConfig(config);
                        }
                    }
                }
    
                // Assign location.
                retVal = config.CLILocation;
    
                // Make sure something was found.
                if (retVal == null)
                {
                    MessageBox.ErrorQuery("Values Missing", "CLI Location not in config file.", "Ok");
                }
            }
    
            else
            {
                MessageBox.ErrorQuery("Values Missing", "Configuration not found.", "Ok");
            }

            return retVal;
        }

        private void Shutdown(int exitCode)
        {
            // Clear any clipboard contents.
            Clipboard.TrySetClipboardData("");
                
            // Close application.
            Application.RequestStop(this);
            
            // Call finializer.
            Dispose();
            
            // Exit application.
            Environment.Exit(exitCode);
        }
        
        #region UI Control Helpers
        private void SortListByName(object? sender, MouseEventEventArgs e)
        {
            // Set current sort column.
            _sortColumn = 0;
            
            // Flip flag for sort order.
            _sortDescending ^= true;
            
            // Reload the list.
            LoadItemListView(_vaultItems);
        }

        private void SortListByValue(object? sender, MouseEventEventArgs e)
        {
            // Set current sort column.
            _sortColumn = 1;

            // Flip flag for sort order.
            _sortValueDescending ^= true;
            
            // Reload the list.
            LoadItemListView(_vaultItems);
        }

        private void SortListByOwner(object? sender, MouseEventEventArgs e)
        {
            // Set current sort column.
            _sortColumn = 2;
            
            // Flip flag for sort order.
            _sortOwnerDescending ^= true;
            
            // Reload the list.
            LoadItemListView(_vaultItems);
        }

        private void LoadItemListView(SortedDictionary<string, VaultItem> items)
        {
            // Clear out any existing items.
            lvwItems.Source = null;
            
            // Check to see if there are any items to show.
            if (items.Count > 0)
            {
                // Get sorted item list.
                List<VaultItem> itemList = GetSortedList(items);
                
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
        
        private List<VaultItem> GetSortedList(SortedDictionary<string, VaultItem> items)
        {
            List<VaultItem> retVal;

            switch (_sortColumn)
            {
                case 0:
                default:
                    // Check to see which way we are sorting.
                    if (_sortDescending)
                    {
                        // Get the list sorted ascending.
                        retVal = items.Values.ToList().OrderByDescending(i=>i.ItemName).ToList();
                    }

                    else
                    {
                        // Get the list sorted descending.
                        retVal = items.Values.ToList().OrderBy(i=>i.ItemName).ToList();    
                    }
                    break;
                
                case 1:
                    // Check to see which way we are sorting.
                    if (_sortValueDescending)
                    {
                        // Get the list sorted ascending.
                        retVal = items.Values.ToList().OrderByDescending(i=>i.ListSortValue).ToList();
                    }

                    else
                    {
                        // Get the list sorted descending.
                        retVal = items.Values.ToList().OrderBy(i=>i.ListSortValue).ToList();    
                    }
                    break;
                    
                case 2:
                    // Check to see which way we are sorting.
                    if (_sortOwnerDescending)
                    {
                        // Get the list sorted ascending.
                        retVal = items.Values.ToList().OrderByDescending(i=>i.ItemOwnerName).ToList();
                    }

                    else
                    {
                        // Get the list sorted descending.
                        retVal = items.Values.ToList().OrderBy(i=>i.ItemOwnerName).ToList();    
                    }
                    break;
            }
            
            // Return the sorted list.
            return retVal;
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
                    Id= "Bitwarden", NodeType = NodeType.Root, Parent = null, Text = null
                }
            };
            
            // Create personal vault node.
            TreeNode personal = new TreeNode("My Vault")
            {
                Tag = new NodeData()
                {
                    Id= null, NodeType = NodeType.Organization, Parent = root, Text = "My Vault"
                }
            };
            
            // Add the personal vault.
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
                
                // Add org to root.
                root.Children.Add(orgVault);
            }
            
            // Add nodes to control.
            this.tvwItems.AddObject(root);
        }
        
        private SortedDictionary<string, VaultItem> GetVaultItemsForTreeNode(ITreeNode? node)
        {
            SortedDictionary<string, VaultItem> retVal = new SortedDictionary<string, VaultItem>();
            
            // Check to see if we have a node.
            if (node != null)
            {
                // Get the node data for this node.
                NodeData data = (NodeData)node.Tag;
                
                // Loop through all vault items.
                foreach (KeyValuePair<string, VaultItem> item in _vaultItems)
                {
                    // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                    if (item.Value.OrganizationId == _currentOrg)
                    {
                        // Based on the node type, get the appropriate items.
                        switch (data.NodeType)
                        {
                            case NodeType.ItemGroup:
                                
                                // Check to see if the node item group type is 0 (all items).
                                if (data.ItemGroupType == NodeItemGroupType.AllItems 
                                    || (int)data.ItemGroupType == item.Value.ItemType)
                                {
                                    // Add the item to the return list.
                                    retVal.Add(item.Key, item.Value);
                                }
                                break;
                            
                            case NodeType.FavoriteGroup:
                                
                                // Check to see if this item is a favorite.
                                if (item.Value.IsFavorite)
                                {
                                    // Add the item to the return list.
                                    retVal.Add(item.Key, item.Value);
                                }
                                break;

                            case NodeType.Folder:
                                
                                // Check to see if the item is either in a folder or this is the 'No Folder' folder.
                                if (item.Value.FolderId == data.Id)
                                {
                                    // Add the item to the return list.
                                    retVal.Add(item.Key, item.Value);
                                }
                                break;
                            
                            case NodeType.Collection:
                                
                                // Check to see if this item has any collection ids.
                                if (item.Value.CollectionIds != null)
                                {
                                    // Check to see if this item is in this collection.
                                    string? collectionId = item.Value.CollectionIds.Find(c => c == data.Id);

                                    // Check to see if one was found.
                                    if (collectionId != null)
                                    {
                                        // Add the item to the return list.
                                        retVal.Add(item.Key, item.Value);
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            // Return filtered list.
            return retVal;
        }

        private void GetCurrentOrgForNode(ITreeNode node)
        {
            // Get the node tag.
            NodeData data = (NodeData) node.Tag;
            
            // Check to see the current node type.
            if (data.NodeType != NodeType.Organization || data.NodeType != NodeType.Organization)
            {
                // Get the parent.
                ITreeNode? parent = data.Parent;
                
                // Check to see if parent is present.
                if (parent != null)
                {
                    NodeData parentData = (NodeData) parent.Tag;
                    
                    // Loop until we get the org id.
                    while (parentData.NodeType != NodeType.Organization)
                    {
                        // Update the parent reference and node type. to this node's parent.
                        parent = parentData.Parent;
                        
                        // Check to see if it is null.
                        if (parent != null)
                        {
                            // Update data reference.
                            parentData = (NodeData) parent.Tag;    
                        }
                    }
                    
                    // We should have found the org node.  Set new org id.
                    _currentOrg = parentData.Id;
                }
            }
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
                    
                    // Remove all subviews from the statusbar.
                    staMain.RemoveAll();
                    
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
                                staMain.Add([stiNew, stiView, stiEdit, stiCopyUser, stiCopyPwd, 
                                    stiFolderMove, stiCollectionMove, stiDelete]);
                            }

                            else if (copyOk)
                            {
                                // Add them to the status bar.
                                staMain.Add([stiNew, stiView, stiEdit, stiCopyUser, 
                                    stiCopyPwd, stiFolderMove, stiDelete]);
                            }
                            
                            else if (colMoveOk)
                            {
                                // Add them to the status bar.
                                staMain.Add([stiNew, stiView, stiEdit,  
                                    stiFolderMove, stiCollectionMove, stiDelete]);
                            }

                            else
                            {
                                // Add them to the status bar.
                                staMain.Add([stiNew, stiView, stiEdit, stiFolderMove, stiDelete]);
                            }
                            break;

                        // If > 1, enable only bulk menu items (add to folder/collection).
                        default:
                            
                            // Only add collection move option if it is allowed.
                            if (colMoveOk)
                            {
                                // Add them to the status bar.
                                staMain.Add([stiFolderMove, stiCollectionMove]);
                            }

                            else
                            {
                                // Add them to the status bar.
                                staMain.Add(stiFolderMove);
                            }

                            break;
                    }
                }

                else
                {
                    staMain.Add(stiNew);
                }
            }

            else
            {
                staMain.Add(stiNew);
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
        private void HandleMouseEvent(object? sender, MouseEvent mouseEvent)
        {
            // Flag that the user has done something.
            _keepAlive = true;
        }

        private void HandleKeyEvent(object? sender, Key e)
        {
            // Flag that the user has done something.
            _keepAlive = true;
            
            // Allow other handlers to get key events.
            e.Handled = false;
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
                
                // Close application.
                Shutdown(0);
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
            MessageBox.Query("Not Implemented",  "This feature not yet implemented.", "Ok");
            
            /*
            // Check to make sure we are logged in.
            if (_vaultRepository.IsLoggedIn)
            {
                // Check to see if this is an 'org enabled' account.
                if (_vaultRepository.IsOrgEnabled)
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
            }*/
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
                
                // Reset controls to empty vault state.
                ResetUI();
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

        private void HandleDebugMode()
        {
            // Check to see if we are leaving debug mode.
            if (_debugMode)
            {
                // Change View title to remove Debug warning.
                
                // Switch VaultRepository implementations.
                
                // Reset the UI
                ResetUI();

                // Toggle debug mode.
                _debugMode = !_debugMode;
            
                // Set menu state.
                mnuMain.Menus[1].Children[1].Checked = _debugMode;
            }

            else
            {
                // Check to see if the user is logged in.
                if (_vaultRepository.IsLoggedIn)
                {
                    int response = MessageBox.Query("Confirm Action", "This will Log you out of your session.  Are you Sure?", "Ok", "Cancel");
            
                    // Check to see if the user confirmed action.
                    if (response == 0)
                    {
                        // Create new worker.
                        LogoutWorker worker = new LogoutWorker(_vaultRepository, "Logging out...");

                        // Execute task.
                        worker.Run();
                        
                        // Switch VaultRepository implementations.
                        _vaultRepository = new DebugVaultRepository();

                        // Reset the UI
                        ResetUI();

                        // Toggle debug mode.
                        _debugMode = !_debugMode;
            
                        // Set menu state.
                        mnuMain.Menus[1].Children[1].Checked = _debugMode;
                    }
                }
            }
        }
        #endregion

        #region Treeview Event Handlers
        private void HandleTreeviewSelectionChanged(object? sender, SelectionChangedEventArgs<ITreeNode> e)
        {
            // Check to make sure the value isn't null.
            if (e.NewValue != null)
            {
                // Store the node that was selected.
                _selectedNode = e.NewValue;
                
                // Update current org for tree node.
                GetCurrentOrgForNode(_selectedNode);
                
                // Get the node data for this node.
                NodeData nodeData = (NodeData) _selectedNode.Tag;
                
                // Handle event based on node type.
                switch (nodeData.NodeType)
                {
                    case NodeType.Organization:
                        
                        // Get the org object.
                        Organization? current = _organizations.Find(o => o.Id == _currentOrg);
                        
                        // Check to see if we have an org.
                        if (current != null)
                        {
                            // Set the current vault name on listview.
                            fraVault.Title = current.Name;
                        }

                        else
                        {
                            // Set the current vault name on listview.
                            fraVault.Title = "My Vault";
                        }
                        
                        // Check to see if there are already static nodes for this org.
                        if (_selectedNode.Children.Count == 0)
                        {
                            // Load base level folders for the organization.
                            ViewUtils.CreateStaticNodesForOrg(_selectedNode, _currentOrg);
                        }
                        break;
                    
                    case NodeType.CollectionGroup:
                        
                        // Load collection nodes.
                        _selectedNode = ViewUtils.CreateCollectionNodes(_collections, _vaultItems, _selectedNode, _currentOrg);
                        break;
                    
                    case NodeType.FolderGroup:
                        
                        // Load folder nodes.
                        _selectedNode = ViewUtils.CreateFolderNodes(_folders, _vaultItems, _selectedNode, _currentOrg);
                        break;
                    
                    case NodeType.FavoriteGroup:
                    case NodeType.Collection:
                    case NodeType.Folder:
                    case NodeType.ItemGroup:
                        
                        // Get the vault items for this item group.
                        SortedDictionary<string, VaultItem> items = GetVaultItemsForTreeNode(_selectedNode);
                        
                        // Load the list view.
                        LoadItemListView(items);
                        break;
                    
                    case NodeType.Root:
                    case NodeType.Item:
                    default:
                        break;
                }
                
                // This causes the treeview to clear its cache of child nodes for the selected node.
                tvwItems.RefreshObject(_selectedNode);
                tvwItems.Expand();
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
        private void HandleListViewOpenItem(object? sender, ListViewItemEventArgs e)
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

        private void HandleListViewSelectedItemChanged(object? sender, ListViewItemEventArgs e)
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
        private void RunSaveItemWorker(List<VaultItem>items , VaultItemSaveAction saveType, string dialogMessage)
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
                case VaultItemSaveAction.MoveToFolder:   
                case VaultItemSaveAction.MoveToOrganization:
                    
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
            
            // Check to see if we have a selected node.
            if (_selectedNode != null)
            {
                // Try to get vault items for the current node.
                SortedDictionary<string, VaultItem> listItems = GetVaultItemsForTreeNode(_selectedNode);
                
                // Update listview contents.
                LoadItemListView(listItems);

            }
            else
            {
                // Update listview contents.
                LoadItemListView(_vaultItems);
            }
        }
        #endregion
        
    }
}
