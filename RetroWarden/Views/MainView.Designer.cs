using System;
using System.Xml;
using Terminal.Gui;
using Retrowarden.Utils;
using Attribute = Terminal.Gui.Attribute;

namespace Retrowarden.Views 
{
    public partial class MainView : Toplevel 
    {
        private Window winMain;
        private FrameView fraItems;
        private TreeView tvwItems;
        private FrameView fraVault;
        private Label lblItemName;
        private Label lblUserId;
        private Label lblOwner;
        private ListView lvwItems;
        private MenuBar mnuMain;
        private StatusBar staMain;
        private StatusItem stiNew;
        private StatusItem stiView;
        private StatusItem stiEdit; 
        private StatusItem stiCopyUser;
        private StatusItem stiCopyPwd;
        private StatusItem stiFolderMove; 
        private StatusItem stiCollectionMove;
        private StatusItem stiDelete;
        
        private void InitializeComponent() {
            
            // Allocate controls.
            this.lblItemName = new Label();
            this.lblUserId = new Label();
            this.lblOwner = new Label();
            this.staMain = new StatusBar();
            this.mnuMain = new MenuBar();
            this.fraVault = new FrameView();
            this.tvwItems = new TreeView();
            this.fraItems = new FrameView();
            this.winMain = new Window();
            this.lvwItems = new ListView();
            
            // Top level window settings.
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Visible = true;
            this.Modal = false;
            this.IsMdiContainer = false;
            this.TextAlignment = TextAlignment.Left; 
            
            // Window settings.
            this.winMain.Width = 120;
            this.Height = 29;
            this.winMain.X = 0;
            this.winMain.Y = 0;
            this.winMain.Visible = true;
            this.winMain.Modal = false;
            this.winMain.IsMdiContainer = false;
            this.winMain.Data = "winMain";
            this.winMain.Border.BorderStyle = BorderStyle.Single;
            this.winMain.Border.Effect3D = false;
            this.winMain.Border.Effect3DBrush = null;
            this.winMain.Border.DrawMarginFrame = true;
            this.winMain.TextAlignment = TextAlignment.Left;
            this.winMain.Title = "Retrowarden";
            this.Add(this.winMain);
            
            this.fraItems.Width = 31;
            this.fraItems.Height = 26;
            this.fraItems.X = 0;
            this.fraItems.Y = 1;
            this.fraItems.Visible = true;
            this.fraItems.Data = "fraItems";
            this.fraItems.Border.BorderStyle = BorderStyle.Single;
            this.fraItems.Border.Effect3D = false;
            this.fraItems.Border.Effect3DBrush = null;
            this.fraItems.Border.DrawMarginFrame = true;
            this.fraItems.TextAlignment = TextAlignment.Left;
            this.fraItems.Title = "Items";
            this.winMain.Add(this.fraItems);
            
            this.tvwItems.Width = 29;
            this.tvwItems.Height = 25;
            this.tvwItems.X = 0;
            this.tvwItems.Y = 0;
            this.tvwItems.Visible = true;
            this.tvwItems.Enabled = true;
            this.tvwItems.Data = "tvwItems";
            this.tvwItems.TextAlignment = TextAlignment.Left;
            this.tvwItems.Style.CollapseableSymbol = '-';
            this.tvwItems.Style.ColorExpandSymbol = false;
            this.tvwItems.Style.ExpandableSymbol = '+';
            this.tvwItems.Style.InvertExpandSymbolColors = false;
            this.tvwItems.Style.LeaveLastRow = false;
            this.tvwItems.Style.ShowBranchLines = true;
            this.tvwItems.SelectionChanged += HandleTreeviewSelectionChanged;
            this.fraItems.Add(this.tvwItems);
            
            this.fraVault.Width = 86;
            this.fraVault.Height = 26;
            this.fraVault.X = 31;
            this.fraVault.Y = 1;
            this.fraVault.Visible = true;
            this.fraVault.Data = "fraVault";
            this.fraVault.Border.BorderStyle = BorderStyle.Single;
            this.fraVault.Border.Effect3D = false;
            this.fraVault.Border.Effect3DBrush = null;
            this.fraVault.Border.DrawMarginFrame = true;
            this.fraVault.TextAlignment = TextAlignment.Left;
            this.fraVault.Title = "All Vaults";
            this.winMain.Add(this.fraVault);

            this.lblItemName.X = 0;
            this.lblItemName.Y = 0;
            this.lblItemName.Width = 30;
            this.lblItemName.Height = 1;
            this.lblItemName.Enabled = true;
            this.lblItemName.Visible = false;
            this.lblItemName.Text = "Item Name";
            this.lblItemName.Clicked += SortListByName;
            this.lblItemName.TextAlignment = TextAlignment.Centered;
            this.fraVault.Add(lblItemName);
            
            this.lblUserId.X = 33;
            this.lblUserId.Y = 0;
            this.lblUserId.Width = 30;
            this.lblUserId.Height = 1;
            this.lblUserId.Enabled = true;
            this.lblUserId.Visible = false;
            this.lblUserId.Text = "User Id";
            this.lblUserId.Clicked += SortListByValue;
            this.lblUserId.TextAlignment = TextAlignment.Centered;
            this.fraVault.Add(lblUserId);
            
            this.lblOwner.X = 64;
            this.lblOwner.Y = 0;
            this.lblOwner.Width = 20;
            this.lblOwner.Height = 1;
            this.lblOwner.Enabled = true;
            this.lblOwner.Visible = false;
            this.lblOwner.Text = "Owner";
            this.lblOwner.Clicked += SortListByOwner;
            this.lblOwner.TextAlignment = TextAlignment.Centered;
            this.fraVault.Add(lblOwner);
            
            this.lvwItems.Width = 84;
            this.lvwItems.Height = 25;
            this.lvwItems.X = 0;
            this.lvwItems.Y = 1;
            this.lvwItems.Visible = true;
            this.lvwItems.Enabled = true;
            this.lvwItems.TextAlignment = TextAlignment.Left;
            this.lvwItems.AllowsMultipleSelection = true;
            this.lvwItems.AllowsMarking = true;
            this.lvwItems.OpenSelectedItem += HandleListViewOpenItem;
            this.lvwItems.SelectedItemChanged += HandleListViewSelectedItemChanged;
            this.fraVault.Add(lvwItems);
            
            this.mnuMain = new MenuBar(new MenuBarItem[]
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
                    }
                }),
                new MenuBarItem ("_Help", new MenuItem [] {
                    new MenuItem("Check Status", "", HandleStatusCheck, null, 
                        null),
                    new MenuItem("Check for _Update", "", HandleUpdateCheck, null, 
                        null),
                    new MenuItem ("_About...",
                        "", () =>  MessageBox.Query ("About Retrowarden", _aboutMessage.ToString(), 
                            "_Ok"), null, null, Key.CtrlMask | Key.A)
                })
            });
            
            this.mnuMain.Width = Dim.Fill(0);
            this.mnuMain.Height = 1;
            this.mnuMain.X = 0;
            this.mnuMain.Y = 0;
            this.mnuMain.Visible = true;
            this.mnuMain.Data = "mnuMain";
            this.mnuMain.TextAlignment = TextAlignment.Left;
            this.winMain.Add(this.mnuMain);
            
            this.staMain.Width = Dim.Fill(0);
            this.staMain.Height = 1;
            this.staMain.X = 0;
            this.staMain.Y = Pos.AnchorEnd(1);
            this.staMain.Visible = true;
            this.staMain.Data = "staMain";
            this.staMain.Text = "";
            this.staMain.TextAlignment = TextAlignment.Left;
            this.staMain.Items = new StatusItem[]{};
            
            // Create new status bar items.  These will get added as context functions when items are selected.
            this.stiNew = new StatusItem(Key.F1, "~F1~ New", HandleCreateKeypress);
            stiView = new StatusItem(Key.F2, "~F2~ View", HandleViewItemKeypress);
            stiEdit = new StatusItem(Key.F3, "~F3~ Edit", HandleEditItemKeypress);
            stiCopyUser = new StatusItem(Key.F4, "~F4~ Copy UserName", HandleUserCopyKeypress);
            stiCopyPwd = new StatusItem(Key.F5, "~F5~ Copy Password", HandlePwdCopyKeypress);
            stiFolderMove = new StatusItem(Key.F6, "~F6~ Move To Folder", HandleFolderMoveKeypress);
            stiCollectionMove = new StatusItem(Key.F7, "~F7~ Move To Org", HandleOrganizationMoveKeypress);
            stiDelete = new StatusItem(Key.F8, "~F8~ Delete", HandleDeleteKeypress);

            this.Add(this.staMain);
        }
    }
}
