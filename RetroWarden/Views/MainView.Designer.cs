using System;
using System.Xml;
using System.Text;
using Terminal.Gui;
using Retrowarden.Utils;
using Attribute = Terminal.Gui.Attribute;

namespace Retrowarden.Views 
{
    public partial class MainView : Toplevel 
    {
        private FrameView fraItems;
        private TreeView tvwItems;
        private FrameView fraVault;
        private Label lblItemName;
        private Label lblUserId;
        private Label lblOwner;
        private ListView lvwItems;
        private MenuBar mnuMain;
        private StatusBar staMain;
        private Shortcut stiNew;
        private Shortcut stiView;
        private Shortcut stiEdit; 
        private Shortcut stiCopyUser;
        private Shortcut stiCopyPwd;
        private Shortcut stiFolderMove; 
        private Shortcut stiCollectionMove;
        private Shortcut stiDelete;
        
        private void InitializeComponent() {
            
            // Top level window settings.
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Visible = true;
            this.Modal = false;
            this.TextAlignment = Alignment.Start; 

            // Create new status bar items.  These will get added as context functions when items are selected.
            stiNew = new Shortcut(Key.F1, "New", HandleCreateKeypress);
            stiView = new Shortcut(Key.F2, "View", HandleViewItemKeypress);
            stiEdit = new Shortcut(Key.F3, "Edit", HandleEditItemKeypress);
            stiCopyUser = new Shortcut(Key.F4, "Copy UserName", HandleUserCopyKeypress);
            stiCopyPwd = new Shortcut(Key.F5, "Copy Password", HandlePwdCopyKeypress);
            stiFolderMove = new Shortcut(Key.F6, "Move To Folder", HandleFolderMoveKeypress);
            stiCollectionMove = new Shortcut(Key.F7, "Move To Org", HandleOrganizationMoveKeypress);
            stiDelete = new Shortcut(Key.F8, "Delete", HandleDeleteKeypress);

            // Allocate controls.
            this.lblItemName = new Label()
            {
                X = 0, Y = 0, Width = 30, Height = 1, Enabled = true, Visible = false,
                Text = "Item Name", TextAlignment = Alignment.Center,
            };
            this.lblItemName.MouseClick += SortListByName;
            
            this.lblUserId = new Label()
            {
                X = 33, Y = 0, Width = 30, Height = 1, Enabled = true, Visible = false,
                Text = "User Id", TextAlignment = Alignment.Center
            };
            this.lblUserId.MouseClick += SortListByValue;

            this.lblOwner = new Label()
            {
                X = 64, Y = 0, Width = 20, Height = 1, Enabled = true, Visible = false,
                Text = "Owner", TextAlignment = Alignment.Center
            };
            this.lblOwner.MouseClick += SortListByOwner;

            this.staMain = new StatusBar()
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = Pos.AnchorEnd(1), Visible = true,
                Data = "staMain", Text = "", TextAlignment = Alignment.Start
            };
            
            this.fraVault = new FrameView()
            {
                Width = Dim.Fill(), Height = Dim.Fill(1), X = 31, Y = 1, Visible = true, Data = "fraVault",
                BorderStyle = LineStyle.Single, TextAlignment = Alignment.Start, Title = "All Vaults"
            };
            
            this.tvwItems = new TreeView()
            {
                Width = 29, Height = Dim.Fill(), X = 0, Y = 0, Visible = true, Enabled = true, Data = "tvwItems",
                TextAlignment = Alignment.Start, Style = new TreeStyle()
                {
                    CollapseableSymbol = new Rune('-'), ColorExpandSymbol = false, ExpandableSymbol = new Rune('+'),
                    InvertExpandSymbolColors = false, LeaveLastRow = false, ShowBranchLines = true
                }
            };
            this.tvwItems.SelectionChanged += HandleTreeviewSelectionChanged;
            
            this.fraItems = new FrameView()
            {
                Width = 31, Height = Dim.Fill(1), X = 0, Y = 1, Visible = true, Data = "fraItems",
                BorderStyle = LineStyle.Single, TextAlignment = Alignment.Start, Title = "Items"
            };
            
            this.lvwItems = new ListView()
            {
                Width = Dim.Fill(), Height = Dim.Fill(), X = 0, Y = 1, Visible = true, Enabled = true,
                TextAlignment = Alignment.Start, AllowsMultipleSelection = true, AllowsMarking = true
            };
            this.lvwItems.OpenSelectedItem += HandleListViewOpenItem;
            this.lvwItems.SelectedItemChanged += HandleListViewSelectedItemChanged;
            
            this.mnuMain = new MenuBar()
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = 0,
                Visible = true, Data = "mnuMain", TextAlignment = Alignment.Start,
                Menus = new MenuBarItem[]
                {
                    new MenuBarItem("_Vault", new MenuItem[]
                    {
                        new MenuItem("_Connect...", "Connect to vault", HandleConnectionRequest, null, 
                            null),
                        new MenuItem("_Sync", "Sync vault", HandleSyncRequest, null, 
                            null),
                        new MenuItem("_Lock", "Lock vault", HandleLockRequest, null, 
                            null),
                        new MenuItem("_Unlock", "Unlock vault", HandleUnlockRequest, null, 
                            null),
                        new MenuItem("_Quit", "Quit Application", HandleQuitRequest, null, 
                            null)
                    }),
                    new MenuBarItem("_Folder", new MenuItem[]
                    {
                        new MenuItem("_New", "", HandleFolderCreate, null, 
                            null)
                    }),
                    new MenuBarItem("_Collection", new MenuItem[]
                    {
                        new MenuItem("_New", "", HandleCollectionCreate, null, 
                            null)
                    }),
                    new MenuBarItem("_Tools", new MenuItem[]
                    {
                        new MenuItem("_Generate Password", "", HandlePasswordGenerate, null, 
                            null),
                        new MenuItem("Generate _Passphrase", "", HandlePassphraseGenerate, null, 
                            null)
                    }),
                    new MenuBarItem("_Options", new MenuItem[]
                    {
                        new MenuItem("_Boomer Mode!","Disable/Enable Mouse Use", HandleBoomerMode,null,null)
                        {
                            Checked = false,
                            CheckType = MenuItemCheckStyle.Checked
                        },
                        new MenuItem("_Debug Mode", "Use Dummy Data Offline", HandleDebugMode, null, null)
                        {
                            Checked = false,
                            CheckType = MenuItemCheckStyle.Checked
                        }
                    }),
                    new MenuBarItem ("_Help", new MenuItem [] {
                        new MenuItem("Check Status", "", HandleStatusCheck, null, 
                            null),
                        new MenuItem("Check for _Update", "", HandleUpdateCheck, null, 
                            null),
                        new MenuItem ("_About...",
                            "", () =>  MessageBox.Query ("About Retrowarden", _aboutMessage.ToString(), 
                                "_Ok"), null, null, KeyCode.CtrlMask | KeyCode.A)
                    })
                }
            };
            
            // Add controls to containers.
            this.fraItems.Add(tvwItems);
            this.fraVault.Add(lblItemName);
            this.fraVault.Add(lblUserId);
            this.fraVault.Add(lblOwner);
            this.fraVault.Add(lvwItems);
            this.Add(this.fraItems);
            this.Add(this.fraVault);
            this.Add(this.mnuMain);
            this.Add(this.staMain);
        }
    }
}
