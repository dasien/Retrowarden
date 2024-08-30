using System;
using Terminal.Gui;
using Retrowarden.Controls;

namespace Retrowarden.Views 
{
    public partial class ItemDetailView : Dialog 
    {
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
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.fraCustomFieldList = new FrameView();
            this.btnNewCustomField = new Button();
            this.stbDetail = new StatusBar();
            
            this.Width = Dim.Percent(85);
            this.Height = Dim.Percent(85);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Visible = true;
            this.Modal = true;
            this.Border.BorderStyle = LineStyle.Single;
            this.TextAlignment = Alignment.Start;
            
            this.lblItemName.Width = 4;
            this.lblItemName.Height = 1;
            this.lblItemName.X = 1;
            this.lblItemName.Y = 0;
            this.lblItemName.Visible = true;
            this.lblItemName.Data = "lblItemName";
            this.lblItemName.Text = "Name";
            this.lblItemName.TextAlignment = Alignment.Start;
            this.Add(this.lblItemName);
            
            this.lblFolder.Width = 4;
            this.lblFolder.Height = 1;
            this.lblFolder.X = 40;
            this.lblFolder.Y = 0;
            this.lblFolder.Visible = true;
            this.lblFolder.Data = "lblFolder";
            this.lblFolder.Text = "Folder";
            this.lblFolder.TextAlignment = Alignment.Start;
            this.Add(this.lblFolder);
            
            this.txtItemName.Width = 30;
            this.txtItemName.Height = 1;
            this.txtItemName.X = 1;
            this.txtItemName.Y = 1;
            this.txtItemName.Visible = true;
            this.txtItemName.Secret = false;
            this.txtItemName.Data = "txtItemName";
            this.txtItemName.Text = "";
            this.txtItemName.TextAlignment = Alignment.Start;
            this.txtItemName.Enter += (s,e) => HandleControlEnter(txtItemName);
            this.Add(this.txtItemName);
            
            this.cboFolder.Width = 30;
            this.cboFolder.Height = 5;
            this.cboFolder.X = 40;
            this.cboFolder.Y = 1;
            this.cboFolder.Visible = true;
            this.cboFolder.Data = "cboFolder";
            this.cboFolder.Text = "";
            this.cboFolder.TextAlignment = Alignment.Start;
            this.cboFolder.Enter += (s,e) => HandleControlEnter(cboFolder);
            this.Add(this.cboFolder);
            
            this.chkFavorite.Width = 10;
            this.chkFavorite.Height = 1;
            this.chkFavorite.X = 77;
            this.chkFavorite.Y = 0;
            this.chkFavorite.Visible = true;
            this.chkFavorite.Data = "chkFavorite";
            this.chkFavorite.Text = "Favorite";
            this.chkFavorite.TextAlignment = Alignment.Start;
            this.chkFavorite.CheckedState = CheckState.UnChecked;
            this.chkFavorite.Enter += (s,e) => HandleControlEnter(chkFavorite);
            this.Add(this.chkFavorite);

            this.chkReprompt.Width = 10;
            this.chkReprompt.Height = 1;
            this.chkReprompt.X = 77;
            this.chkReprompt.Y = 1;
            this.chkReprompt.Visible = true;
            this.chkReprompt.Data = "chkReprompt";
            this.chkReprompt.Text = "Require Reprompt";
            this.chkReprompt.TextAlignment = Alignment.Start;
            this.chkReprompt.CheckedState = CheckState.UnChecked;
            this.chkReprompt.Enter += (s,e) => HandleControlEnter(chkReprompt);
            this.Add(this.chkReprompt);

            this.fraNotes.Width = 97;
            this.fraNotes.Height = 9;
            this.fraNotes.X = 1;
            this.fraNotes.Y = 20;
            this.fraNotes.Visible = true;
            this.fraNotes.CanFocus = true;
            this.fraNotes.Data = "fraNotes";
            this.fraNotes.Border.BorderStyle = LineStyle.Single;
            this.fraNotes.TextAlignment = Alignment.Start;
            this.fraNotes.Title = "Notes";
            this.fraNotes.Enter += (s,e) => HandleControlEnter(fraNotes);
            this.Add(this.fraNotes);
            
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
            this.tvwNotes.TextAlignment = Alignment.Start;
            this.tvwNotes.Enter += (s,e) => HandleControlEnter(tvwNotes);
            this.fraNotes.Add(this.tvwNotes);
            
            this.fraCustomFieldList.Width = 97;
            this.fraCustomFieldList.Height = 7;
            this.fraCustomFieldList.X = 1;
            this.fraCustomFieldList.Y = 30;
            this.fraCustomFieldList.Visible = true;
            this.fraCustomFieldList.Enabled = true;
            this.fraCustomFieldList.CanFocus = true;
            this.fraCustomFieldList.Data = "fraCustomFieldList";
            this.fraCustomFieldList.Border.BorderStyle = LineStyle.Single;
            this.fraCustomFieldList.TextAlignment = Alignment.Start;
            this.fraCustomFieldList.Title = "Custom Fields";
            this.fraCustomFieldList.Enter += (s,e) => HandleControlEnter(fraCustomFieldList);
            this.Add(this.fraCustomFieldList);
            
            this.btnNewCustomField.Width = 18;
            this.btnNewCustomField.Height = 1;
            this.btnNewCustomField.X = 1;
            this.btnNewCustomField.Y = 37;
            this.btnNewCustomField.Visible = true;
            this.btnNewCustomField.Data = "btnNewCustomField";
            this.btnNewCustomField.Text = "New Custom Field";
            this.btnNewCustomField.TextAlignment = Alignment.Center;
            this.btnNewCustomField.IsDefault = false;
            this.btnNewCustomField.Accept += NewCustomFieldButtonClicked;
            this.btnNewCustomField.Enter += (s,e) => HandleControlEnter(btnNewCustomField);
            this.Add(this.btnNewCustomField);

            this.btnSave.Width = 8;
            this.btnSave.Height = 1;
            this.btnSave.X = Pos.Center() - 10;
            this.btnSave.Y = 39;
            this.btnSave.Visible = true;
            this.btnSave.Data = "btnSave";
            this.btnSave.Text = "Save";
            this.btnSave.TextAlignment = Alignment.Center;
            this.btnSave.IsDefault = false;
            this.btnSave.Accept += SaveButtonClicked;
            this.btnSave.Enter += (s,e) => HandleControlEnter(btnSave);
            this.Add(btnSave);
            
            this.btnCancel.Width = 8;
            this.btnCancel.Height = 1;
            this.btnCancel.X = Pos.Center() + 2;
            this.btnCancel.Y = 39;
            this.btnCancel.Visible = true;
            this.btnCancel.Data = "";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlignment = Alignment.Center;
            this.btnCancel.IsDefault = false;
            this.btnCancel.Accept += CancelButtonClicked;
            this.btnCancel.Enter += (s,e) => HandleControlEnter(btnCancel);
            this.Add(btnCancel);
            
            this.stbDetail.Width = Dim.Fill(0);
            this.stbDetail.Height = 1;
            this.stbDetail.X = 0;
            this.stbDetail.Y = Pos.AnchorEnd(1);
            this.stbDetail.Visible = true;
            this.stbDetail.Data = "stbDetail";
            this.stbDetail.Text = "";
            this.stbDetail.TextAlignment = Alignment.Start;
            this.Add(stbDetail);
        }
    }
}
