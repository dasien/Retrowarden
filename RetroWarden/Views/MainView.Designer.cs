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
    public partial class MainView : Toplevel 
    {
        private FrameView fraItems;
        private TreeView tvwItems;
        private FrameView fraVault;
        private Label lblItemName;
        private Label lblUserId;
        private Label lblOwner;
        private ListView lvwItems;
        private MenuBarv2 mnuMain;
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
            
            this.mnuMain = new MenuBarv2()
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = 0,
                Visible = true, Data = "mnuMain", TextAlignment = Alignment.Start,
                Menus = new MenuBarItemv2[]
                {
                    new MenuBarItemv2("_Vault", new MenuItemv2[]
                    {
                        new MenuItemv2("_Connect...", "Connect to vault", HandleConnectionRequest, null),
                        new MenuItemv2("_Sync", "Sync vault", HandleSyncRequest, null),
                        new MenuItemv2("_Lock", "Lock vault", HandleLockRequest, null),
                        new MenuItemv2("_Unlock", "Unlock vault", HandleUnlockRequest, null),
                        new MenuItemv2("_Quit", "Quit Application", HandleQuitRequest,null)
                    }),
                    new MenuBarItemv2("_Folder", new MenuItemv2[]
                    {
                        new MenuItemv2("_New", "", HandleFolderCreate, null)
                    }),
                    new MenuBarItemv2("_Collection", new MenuItemv2[]
                    {
                        new MenuItemv2("_New", "", HandleCollectionCreate,null)
                    }),
                    new MenuBarItemv2("_Tools", new MenuItemv2[]
                    {
                        new MenuItemv2("_Generate Password", "", HandlePasswordGenerate, null),
                        new MenuItemv2("Generate _Passphrase", "", HandlePassphraseGenerate, null)
                    }),
                    new MenuBarItemv2("_Options", new MenuItemv2[]
                    {
                        new MenuItemv2("_Boomer Mode!","Disable/Enable Mouse Use", HandleBoomerMode,null)
                        {
                            //Checked = false,
                            //CheckType = MenuItemCheckStyle.Checked
                        },
                        new MenuItemv2("_Debug Mode", "Use Dummy Data Offline", HandleDebugMode, null)
                        {
                            //Checked = false,
                            //CheckType = MenuItemCheckStyle.Checked
                        }
                    }),
                    new MenuBarItemv2 ("_Help", new MenuItemv2 [] {
                        new MenuItemv2("Check Status", "", HandleStatusCheck, null),
                        new MenuItemv2("Check for _Update", "", HandleUpdateCheck,null),
                        new MenuItemv2 ("_About...",
                            "", () =>  MessageBox.Query ("About Retrowarden", _aboutMessage.ToString(), 
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
