using System;
using Terminal.Gui;
using Retrowarden.Controls;

namespace Retrowarden.Views 
{
    public partial class ItemDetailView : Dialog 
    {
        private ScrollView scrMain;
        private Label lblItemName;
        private Label lblFolder;
        private TextField txtItemName;
        private ComboBox cboFolder;
        private CheckBox chkFavorite;
        private CheckBox chkReprompt;
        private FrameView fraNotes;
        private TextView tvwNotes;
        private Button btnSave;
        private Button btnCancel;
        private FrameView fraCustomFieldList;
        private Button btnNewCustomField;
        private CustomFieldScrollView scrCustomFields;
        private Label lblCreateDate;
        private Label lblUpdateDate;
        private StatusBar stbDetail;
        
        private void InitializeComponent(int scrollBottom) 
        {
            this.tvwNotes = new TextView();
            this.fraNotes = new FrameView();
            this.chkFavorite = new CheckBox();
            this.chkReprompt = new CheckBox();
            this.cboFolder = new ComboBox();
            this.txtItemName = new TextField();
            this.lblFolder = new Label();
            this.lblItemName = new Label();
            this.scrMain = new ScrollView();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.fraCustomFieldList = new FrameView();
            this.btnNewCustomField = new Button();
            this.lblCreateDate = new Label();
            this.lblUpdateDate = new Label();
            this.stbDetail = new StatusBar();
            
            this.Width = Dim.Percent(85f);
            this.Height = Dim.Percent(85f);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Visible = true;
            this.Modal = true;
            this.IsMdiContainer = false;
            this.Border.BorderStyle = BorderStyle.Single;
            this.Border.Effect3D = true;
            this.Border.Effect3DBrush = null;
            this.Border.DrawMarginFrame = true;
            this.TextAlignment = TextAlignment.Left;
            
            this.scrMain.Width = Dim.Fill(0);
            this.scrMain.Height = Dim.Fill(1);
            this.scrMain.X = 0;
            this.scrMain.Y = 1;
            this.scrMain.Visible = true;
            this.scrMain.ContentSize = new Size(99,scrollBottom);
            this.scrMain.Data = "scrMain";
            this.scrMain.TextAlignment = TextAlignment.Left;
            this.scrMain.ShowHorizontalScrollIndicator = false;
            this.scrMain.AutoHideScrollBars = true;
            this.Add(this.scrMain);
            
            this.lblItemName.Width = 4;
            this.lblItemName.Height = 1;
            this.lblItemName.X = 1;
            this.lblItemName.Y = 0;
            this.lblItemName.Visible = true;
            this.lblItemName.Data = "lblItemName";
            this.lblItemName.Text = "Name";
            this.lblItemName.TextAlignment = TextAlignment.Left;
            this.scrMain.Add(this.lblItemName);
            
            this.lblFolder.Width = 4;
            this.lblFolder.Height = 1;
            this.lblFolder.X = 40;
            this.lblFolder.Y = 0;
            this.lblFolder.Visible = true;
            this.lblFolder.Data = "lblFolder";
            this.lblFolder.Text = "Folder";
            this.lblFolder.TextAlignment = TextAlignment.Left;
            this.scrMain.Add(this.lblFolder);
            
            this.txtItemName.Width = 30;
            this.txtItemName.Height = 1;
            this.txtItemName.X = 1;
            this.txtItemName.Y = 1;
            this.txtItemName.Visible = true;
            this.txtItemName.Secret = false;
            this.txtItemName.Data = "txtItemName";
            this.txtItemName.Text = "";
            this.txtItemName.TextAlignment = TextAlignment.Left;
            this.txtItemName.Enter += (_) => HandleControlEnter(txtItemName);
            this.scrMain.Add(this.txtItemName);
            
            this.cboFolder.Width = 30;
            this.cboFolder.Height = 5;
            this.cboFolder.X = 40;
            this.cboFolder.Y = 1;
            this.cboFolder.Visible = true;
            this.cboFolder.Data = "cboFolder";
            this.cboFolder.Text = "";
            this.cboFolder.TextAlignment = TextAlignment.Left;
            this.scrMain.Add(this.cboFolder);
            
            this.chkFavorite.Width = 10;
            this.chkFavorite.Height = 1;
            this.chkFavorite.X = 77;
            this.chkFavorite.Y = 0;
            this.chkFavorite.Visible = true;
            this.chkFavorite.Data = "chkFavorite";
            this.chkFavorite.Text = "Favorite";
            this.chkFavorite.TextAlignment = TextAlignment.Left;
            this.chkFavorite.Checked = false;
            this.scrMain.Add(this.chkFavorite);

            this.chkReprompt.Width = 10;
            this.chkReprompt.Height = 1;
            this.chkReprompt.X = 77;
            this.chkReprompt.Y = 1;
            this.chkReprompt.Visible = true;
            this.chkReprompt.Data = "chkReprompt";
            this.chkReprompt.Text = "Rrequire Reprompt";
            this.chkReprompt.TextAlignment = TextAlignment.Left;
            this.chkReprompt.Checked = false;
            this.scrMain.Add(this.chkReprompt);

            this.fraNotes.Width = 97;
            this.fraNotes.Height = 9;
            this.fraNotes.X = 1;
            this.fraNotes.Y = 20;
            this.fraNotes.Visible = true;
            this.fraNotes.Data = "fraNotes";
            this.fraNotes.Border.BorderStyle = BorderStyle.Single;
            this.fraNotes.Border.Effect3D = false;
            this.fraNotes.Border.Effect3DBrush = null;
            this.fraNotes.Border.DrawMarginFrame = true;
            this.fraNotes.TextAlignment = TextAlignment.Left;
            this.fraNotes.Title = "Notes";
            this.scrMain.Add(this.fraNotes);
            
            this.tvwNotes.Width = 95;
            this.tvwNotes.Height = 7;
            this.tvwNotes.X = 1;
            this.tvwNotes.Y = 0;
            this.tvwNotes.Visible = true;
            this.tvwNotes.AllowsTab = true;
            this.tvwNotes.AllowsReturn = true;
            this.tvwNotes.WordWrap = false;
            this.tvwNotes.Data = "tvwNotes";
            this.tvwNotes.Text = "";
            this.tvwNotes.AllowsTab = false;
            this.tvwNotes.TextAlignment = TextAlignment.Left;
            this.fraNotes.Add(this.tvwNotes);
            
            this.fraCustomFieldList.Width = 97;
            this.fraCustomFieldList.Height = 7;
            this.fraCustomFieldList.X = 1;
            this.fraCustomFieldList.Y = 30;
            this.fraCustomFieldList.Visible = true;
            this.fraCustomFieldList.Enabled = true;
            this.fraCustomFieldList.CanFocus = true;
            this.fraCustomFieldList.Data = "fraCustomFieldList";
            this.fraCustomFieldList.Border.BorderStyle = BorderStyle.Single;
            this.fraCustomFieldList.Border.Effect3D = false;
            this.fraCustomFieldList.Border.Effect3DBrush = null;
            this.fraCustomFieldList.Border.DrawMarginFrame = true;
            this.fraCustomFieldList.TextAlignment = TextAlignment.Left;
            this.fraCustomFieldList.Title = "Custom Fields";
            this.scrMain.Add(this.fraCustomFieldList);
            
            this.btnNewCustomField.Width = 18;
            this.btnNewCustomField.Height = 1;
            this.btnNewCustomField.X = 1;
            this.btnNewCustomField.Y = 37;
            this.btnNewCustomField.Visible = true;
            this.btnNewCustomField.Data = "btnNewCustomField";
            this.btnNewCustomField.Text = "New Custom Field";
            this.btnNewCustomField.TextAlignment = TextAlignment.Centered;
            this.btnNewCustomField.IsDefault = false;
            this.btnNewCustomField.Clicked += NewCustomFieldButtonClicked;
            this.scrMain.Add(this.btnNewCustomField);

            this.btnSave.Width = 8;
            this.btnSave.Height = 1;
            this.btnSave.X = Pos.Center() - 10;
            this.btnSave.Y = 39;
            this.btnSave.Visible = true;
            this.btnSave.Data = "btnSave";
            this.btnSave.Text = "Save";
            this.btnSave.TextAlignment = TextAlignment.Centered;
            this.btnSave.IsDefault = false;
            this.btnSave.Clicked += SaveButtonClicked;
            this.scrMain.Add(btnSave);
            
            this.btnCancel.Width = 8;
            this.btnCancel.Height = 1;
            this.btnCancel.X = Pos.Center() + 2;
            this.btnCancel.Y = 39;
            this.btnCancel.Visible = true;
            this.btnCancel.Data = "";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlignment = TextAlignment.Centered;
            this.btnCancel.IsDefault = false;
            this.btnCancel.Clicked += CancelButtonClicked;
            this.scrMain.Add(btnCancel);
            
            /*this.lblCreateDate.Width = 10;
            this.lblCreateDate.Height = 1;
            this.lblCreateDate.X = 1;
            this.lblCreateDate.Y = 40;
            this.lblCreateDate.Visible = true;
            this.lblCreateDate.Data = "lblCreateDate";
            this.lblCreateDate.Text = "";
            this.lblCreateDate.TextAlignment = TextAlignment.Left;
            this.scrMain.Add(this.lblCreateDate);
            
            this.lblUpdateDate.Width = 10;
            this.lblUpdateDate.Height = 1;
            this.lblUpdateDate.X = 47;
            this.lblUpdateDate.Y = 40;
            this.lblUpdateDate.Visible = true;
            this.lblUpdateDate.Data = "lblUpdateDate";
            this.lblUpdateDate.Text = "";
            this.lblUpdateDate.TextAlignment = TextAlignment.Left;
            this.scrMain.Add(this.lblUpdateDate);*/

            this.stbDetail.Width = Dim.Fill(0);
            this.stbDetail.Height = 1;
            this.stbDetail.X = 0;
            this.stbDetail.Y = Pos.AnchorEnd(1);
            this.stbDetail.Visible = true;
            this.stbDetail.Data = "stbDetail";
            this.stbDetail.Text = "";
            this.stbDetail.TextAlignment = TextAlignment.Left;
            this.Add(stbDetail);
        }
    }
}
