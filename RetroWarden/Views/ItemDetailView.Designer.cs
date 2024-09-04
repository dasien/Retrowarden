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
            this.Width = Dim.Percent(85);
            this.Height = Dim.Percent(85);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Visible = true;
            this.Modal = true;
            this.Border.BorderStyle = LineStyle.Single;
            this.TextAlignment = Alignment.Start;

            this.lblItemName = new Label
            {
                Width = 4, Height = 1, X = 1, Y = 0, Visible = true, Data = "lblItemName", Text = "Name",
                TextAlignment = Alignment.Start
            };
            this.Add(this.lblItemName);

            this.lblFolder = new Label
            {
                Width = 4, Height = 1, X = 40, Y = 0, Visible = true, Data = "lblFolder", Text = "Folder",
                TextAlignment = Alignment.Start
            };
            this.Add(this.lblFolder);

            this.txtItemName = new TextField
            {
                Width = 30, Height = 1, X = 1, Y = 1, Visible = true, Secret = false, Data = "txtItemName", Text = "",
                TextAlignment = Alignment.Start
            };
            this.txtItemName.Enter += (s, e) => HandleControlEnter(txtItemName);
            this.Add(this.txtItemName);

            this.cboFolder = new ComboBox
            {
                Width = 30, Height = 5, X = 40, Y = 1, Visible = true, Data = "cboFolder", Text = "", TextAlignment = Alignment.Start
            };
            this.cboFolder.Enter += (s, e) => HandleControlEnter(cboFolder);
            this.Add(this.cboFolder);

            this.chkFavorite = new CheckBox
            {
                Width = 10, Height = 1, X = 77, Y = 0, Visible = true, Data = "chkFavorite", Text = "Favorite",
                TextAlignment = Alignment.Start, CheckedState = CheckState.UnChecked
            };
            this.chkFavorite.Enter += (s, e) => HandleControlEnter(chkFavorite);
            this.Add(this.chkFavorite);

            this.chkReprompt = new CheckBox
            {
                Width = 10, Height = 1, X = 77, Y = 1, Visible = true, Data = "chkReprompt", Text = "Require Reprompt",
                TextAlignment = Alignment.Start, CheckedState = CheckState.UnChecked
            };
            this.chkReprompt.Enter += (s, e) => HandleControlEnter(chkReprompt);
            this.Add(this.chkReprompt);

            this.fraNotes = new FrameView
            {
                Width = 97, Height = 9, X = 1, Y = 20, Visible = true, CanFocus = true, Data = "fraNotes",
                Border = { BorderStyle = LineStyle.Single }, TextAlignment = Alignment.Start, Title = "Notes"
            };
            this.fraNotes.Enter += (s, e) => HandleControlEnter(fraNotes);
            this.Add(this.fraNotes);

            this.tvwNotes = new TextView
            {
                Width = 95, Height = 7, X = 1, Y = 0, Visible = true, AllowsTab = true, AllowsReturn = true,
                WordWrap = false, Data = "tvwNotes", Text = "", TextAlignment = Alignment.Start
            };
            this.tvwNotes.Enter += (s, e) => HandleControlEnter(tvwNotes);
            this.fraNotes.Add(this.tvwNotes);

            this.fraCustomFieldList = new FrameView
            {
                Width = 97, Height = 7, X = 1, Y = 30, Visible = true, Enabled = true, CanFocus = true, Data = "fraCustomFieldList",
                Border = { BorderStyle = LineStyle.Single }, TextAlignment = Alignment.Start, Title = "Custom Fields"
            };
            this.fraCustomFieldList.Enter += (s, e) => HandleControlEnter(fraCustomFieldList);
            this.Add(this.fraCustomFieldList);

            this.btnNewCustomField = new Button
            {
                Width = 18, Height = 1, X = 1, Y = 37, Visible = true, Data = "btnNewCustomField",
                Text = "New Custom Field", TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnNewCustomField.Accept += NewCustomFieldButtonClicked;
            this.btnNewCustomField.Enter += (s, e) => HandleControlEnter(btnNewCustomField);
            this.Add(this.btnNewCustomField);

            this.btnSave = new Button
            {
                Width = 8, Height = 1, X = Pos.Center() - 10, Y = 39, Visible = true, Data = "btnSave",
                Text = "Save", TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnSave.Accept += SaveButtonClicked;
            this.btnSave.Enter += (s, e) => HandleControlEnter(btnSave);
            this.Add(this.btnSave);

            this.btnCancel = new Button
            {
                Width = 8, Height = 1, X = Pos.Center() + 2, Y = 39, Visible = true, Text = "Cancel",
                TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnCancel.Accept += CancelButtonClicked;
            this.btnCancel.Enter += (s, e) => HandleControlEnter(btnCancel);
            this.Add(this.btnCancel);

            this.stbDetail = new StatusBar
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = Pos.AnchorEnd(1), Visible = true, Data = "stbDetail",
                Text = "", TextAlignment = Alignment.Start
            };
            this.Add(this.stbDetail);
        }
    }
}
