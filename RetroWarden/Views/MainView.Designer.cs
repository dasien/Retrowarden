using System;
using System.Xml;
using System.Text;
using Terminal.Gui;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Retrowarden.Utils;
using Terminal.Gui.Input;
using Terminal.Gui.Drawing;
using Terminal.Gui.Drivers;

namespace Retrowarden.Views
{
    public partial class MainView : Window 
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
            this.lblItemName.MouseEvent += SortListByName;
            
            this.lblUserId = new Label()
            {
                X = 33, Y = 0, Width = 30, Height = 1, Enabled = true, Visible = false,
                Text = "User Id", TextAlignment = Alignment.Center
            };
            this.lblUserId.MouseEvent += SortListByValue;

            this.lblOwner = new Label()
            {
                X = 64, Y = 0, Width = 20, Height = 1, Enabled = true, Visible = false,
                Text = "Owner", TextAlignment = Alignment.Center
            };
            this.lblOwner.MouseEvent += SortListByOwner;

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
                TextAlignment = Alignment.Start, MarkMultiple = true, ShowMarks = true
            };
            this.lvwItems.Accepted += HandleListViewOpenItem;
            this.lvwItems.ValueChanged += HandleListViewSelectedItemChanged;
            
            this.mnuMain = new MenuBar()
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = 0,
                Visible = true, Data = "mnuMain", TextAlignment = Alignment.Start,
                Menus = new MenuBarItem[]
                {
                    new MenuBarItem("_Vault", new MenuItem[]
                    {
                        new MenuItem("_Connect...", "Connect to vault", HandleConnectionRequest, null),
                        new MenuItem("_Sync", "Sync vault", HandleSyncRequest, null),
                        new MenuItem("_Lock", "Lock vault", HandleLockRequest, null),
                        new MenuItem("_Unlock", "Unlock vault", HandleUnlockRequest, null),
                        new MenuItem("_Quit", "Quit Application", HandleQuitRequest,null)
                    }),
                    new MenuBarItem("_Folder", new MenuItem[]
                    {
                        new MenuItem("_New", "", HandleFolderCreate, null)
                    }),
                    new MenuBarItem("_Collection", new MenuItem[]
                    {
                        new MenuItem("_New", "", HandleCollectionCreate,null)
                    }),
                    new MenuBarItem("_Tools", new MenuItem[]
                    {
                        new MenuItem("_Generate Password", "", HandlePasswordGenerate, null),
                        new MenuItem("Generate _Passphrase", "", HandlePassphraseGenerate, null)
                    }),
                    new MenuBarItem("_Options", new MenuItem[]
                    {
                        new MenuItem("_Boomer Mode!","Disable/Enable Mouse Use", HandleBoomerMode,null)
                        {
                            //Checked = false,
                            //CheckType = MenuItemCheckStyle.Checked
                        },
                        new MenuItem("_Debug Mode", "Use Dummy Data Offline", HandleDebugMode, null)
                        {
                            //Checked = false,
                            //CheckType = MenuItemCheckStyle.Checked
                        }
                    }),
                    new MenuBarItem ("_Help", new MenuItem [] {
                        new MenuItem("Check Status", "", HandleStatusCheck, null),
                        new MenuItem("Check for _Update", "", HandleUpdateCheck,null),
                        new MenuItem ("_About...",
                            "", () =>  MessageBox.Query (_app, "About Retrowarden", _aboutMessage.ToString(), 
                                "_Ok"), KeyCode.CtrlMask | KeyCode.A)
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
