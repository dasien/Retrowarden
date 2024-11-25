using System;
using System.Drawing;
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
        
        private void InitializeComponent() 
        {
            this.Width = Dim.Percent(85);
            this.Height = Dim.Percent(85);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.SetContentSize(new Size(100, 40));
            this.ViewportSettings = ViewportSettings.AllowNegativeY | ViewportSettings.AllowYGreaterThanContentHeight;
            this.Visible = true;
            this.Modal = true;
            this.Border.BorderStyle = LineStyle.Single;
            this.TextAlignment = Alignment.Start;
            this.MouseEvent += (o, e) => HandleMouseEvent(o,e);
            //this.TabStop = TabBehavior.TabGroup;
            
            this.lblItemName = new Label
            {
                Width = 8, Height = 1, X = 1, Y = 0, Visible = true, Data = "lblItemName", Text = "Name",
                TextAlignment = Alignment.Start
            };

            this.lblFolder = new Label
            {
                Width = 8, Height = 1, X = 40, Y = 0, Visible = true, Data = "lblFolder", Text = "Folder",
                TextAlignment = Alignment.Start
            };

            this.txtItemName = new TextField
            {
                Width = 30, Height = 1, X = 1, Y = 1, Visible = true, Secret = false, Data = "txtItemName", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop, CanFocus = true
            };
            //this.txtItemName.Enter += (s, e) => HandleControlEnter(txtItemName);

            this.cboFolder = new ComboBox
            {
                Width = 30, Height = 5, X = 40, Y = 1, Visible = true, Data = "cboFolder", 
                Text = "", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            //this.cboFolder.Enter += (s, e) => HandleControlEnter(cboFolder);

            this.chkFavorite = new CheckBox
            {
                Width = 10, Height = 1, X = 77, Y = 0, Visible = true, Data = "chkFavorite", Text = "Favorite",
                TextAlignment = Alignment.Start, CheckedState = CheckState.UnChecked, TabStop = TabBehavior.TabStop
            };
            //this.chkFavorite.Enter += (s, e) => HandleControlEnter(chkFavorite);

            this.chkReprompt = new CheckBox
            {
                Width = 18, Height = 1, X = 77, Y = 1, Visible = true, Data = "chkReprompt", Text = "Require Reprompt",
                TextAlignment = Alignment.Start, CheckedState = CheckState.UnChecked, TabStop = TabBehavior.TabStop
            };
            //this.chkReprompt.Enter += (s, e) => HandleControlEnter(chkReprompt);

            this.fraNotes = new FrameView
            {
                Width = 97, Height = 9, X = 1, Y = 20, Visible = true, CanFocus = true, Data = "fraNotes",
                Border = { BorderStyle = LineStyle.Single }, TextAlignment = Alignment.Start, Title = "Notes",
                TabStop = TabBehavior.TabStop
            };
            //this.fraNotes.Enter += (s, e) => HandleControlEnter(fraNotes);
            
            this.tvwNotes = new TextView
            {
                Width = 95, Height = 7, X = 1, Y = 0, Visible = true, AllowsTab = true, AllowsReturn = true,
                WordWrap = false, Data = "tvwNotes", Text = "", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            //this.tvwNotes.Enter += (s, e) => HandleControlEnter(tvwNotes);

            this.fraCustomFieldList = new FrameView
            {
                Width = 97, Height = 7, X = 1, Y = 30, Visible = true, Enabled = true, CanFocus = true, Data = "fraCustomFieldList",
                Border = { BorderStyle = LineStyle.Single }, TextAlignment = Alignment.Start, Title = "Custom Fields",
                TabStop = TabBehavior.TabStop
            };
            //this.fraCustomFieldList.Enter += (s, e) => HandleControlEnter(fraCustomFieldList);

            this.btnNewCustomField = new Button
            {
                Width = 22, Height = 1, X = 1, Y = 37, Visible = true, Data = "btnNewCustomField",
                Text = "New Custom Field", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnNewCustomField.Accept += NewCustomFieldButtonClicked;
            //this.btnNewCustomField.Enter += (s, e) => HandleControlEnter(btnNewCustomField);

            this.btnSave = new Button
            {
                Width = 8, Height = 1, X = Pos.Center() - 10, Y = 39, Visible = true, Data = "btnSave",
                Text = "Save", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnSave.Accept += SaveButtonClicked;
            //this.btnSave.Enter += (s, e) => HandleControlEnter(btnSave);

            this.btnCancel = new Button
            {
                Width = 10, Height = 1, X = Pos.Center() + 2, Y = 39, Visible = true, Text = "Cancel",
                TextAlignment = Alignment.Center, IsDefault = true, TabStop = TabBehavior.TabStop
            };
            this.btnCancel.Accept += CancelButtonClicked;
            //this.btnCancel.Enter += (s, e) => HandleControlEnter(btnCancel);

            this.stbDetail = new StatusBar
            {
                Width = Dim.Fill(0), Height = 1, X = 0, Y = Pos.AnchorEnd(0), Visible = true, Data = "stbDetail",
                Text = "", TextAlignment = Alignment.Start
            };
        }
    }
}
