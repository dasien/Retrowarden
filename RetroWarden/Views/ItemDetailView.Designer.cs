using System;
using System.Drawing;
using Terminal.Gui;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Retrowarden.Controls;
using Terminal.Gui.Drawing;

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
            // Dialog settings
            this.Width = Dim.Percent(85);
            this.Height = Dim.Percent(85);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            // Initial content size - will be recalculated in RelocateFooterControls() based on actual subview height
            // Width 0 means use Viewport width (no horizontal scrolling)
            this.SetContentSize(new Size(0, 40));
            // Only allow vertical scrolling past content height, not into negative Y
            this.ViewportSettings = ViewportSettingsFlags.AllowYGreaterThanContentHeight;
            this.Visible = true;
            this.Border.BorderStyle = LineStyle.Single;
            this.TextAlignment = Alignment.Start;
            this.MouseEvent += (o, e) => HandleMouseEvent(o, e);

            // Disable horizontal scrollbar - form should not scroll horizontally
            this.HorizontalScrollBar.Visible = false;
            this.HorizontalScrollBar.AutoShow = false;

            // ===== ROW 0: Labels =====
            this.lblItemName = new Label
            {
                X = 1,
                Y = 0,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblItemName",
                Text = "Name",
                TextAlignment = Alignment.Start
            };

            this.lblFolder = new Label
            {
                X = 40,
                Y = Pos.Top(lblItemName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblFolder",
                Text = "Folder",
                TextAlignment = Alignment.Start
            };

            this.chkFavorite = new CheckBox
            {
                X = 77,
                Y = Pos.Top(lblItemName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "chkFavorite",
                Text = "Favorite",
                TextAlignment = Alignment.Start,
                Value = CheckState.UnChecked,
                TabStop = TabBehavior.TabStop
            };
            this.chkFavorite.HasFocusChanged += (s, e) => HandleFocusChange(chkFavorite, e);

            // ===== ROW 1: Input controls =====
            this.txtItemName = new TextField
            {
                X = Pos.Left(lblItemName),
                Y = Pos.Bottom(lblItemName),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtItemName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop,
                CanFocus = true
            };
            this.txtItemName.HasFocusChanged += (s, e) => HandleFocusChange(txtItemName, e);

            // Note: Height=1 for just the input field; dropdown renders as overlay in Terminal.Gui v2
            this.cboFolder = new ComboBox
            {
                X = Pos.Left(lblFolder),
                Y = Pos.Top(txtItemName),
                Width = 30,
                Height = 1,
                Visible = true,
                Data = "cboFolder",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.cboFolder.HasFocusChanged += (s, e) => HandleFocusChange(cboFolder, e);

            this.chkReprompt = new CheckBox
            {
                X = Pos.Left(chkFavorite),
                Y = Pos.Top(txtItemName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "chkReprompt",
                Text = "Require Reprompt",
                TextAlignment = Alignment.Start,
                Value = CheckState.UnChecked,
                TabStop = TabBehavior.TabStop
            };
            this.chkReprompt.HasFocusChanged += (s, e) => HandleFocusChange(chkReprompt, e);

            // ===== FOOTER CONTROLS (positioned relative to _subView in RelocateFooterControls) =====
            // Initial Y positions are placeholders - will be updated by RelocateFooterControls()

            this.fraNotes = new FrameView
            {
                X = 1,
                Y = 20,  // Placeholder - updated by RelocateFooterControls
                Width = 97,
                Height = 9,
                Visible = true,
                CanFocus = true,
                Data = "fraNotes",
                Border = { BorderStyle = LineStyle.Single },
                TextAlignment = Alignment.Start,
                Title = "Notes",
                TabStop = TabBehavior.TabStop
            };

            this.tvwNotes = new TextView
            {
                X = 1,
                Y = 0,
                Width = 93,  // Fixed width instead of Dim.Fill() to avoid scrollbar calculation issues
                Height = 7,
                Visible = true,
                TabKeyAddsTab = true,
                EnterKeyAddsLine = true,
                WordWrap = false,
                Data = "tvwNotes",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };

            this.fraCustomFieldList = new FrameView
            {
                X = Pos.Left(fraNotes),
                Y = 30,  // Placeholder - updated by RelocateFooterControls
                Width = 97,
                Height = 7,
                Visible = true,
                Enabled = true,
                CanFocus = true,
                Data = "fraCustomFieldList",
                Border = { BorderStyle = LineStyle.Single },
                TextAlignment = Alignment.Start,
                Title = "Custom Fields",
                TabStop = TabBehavior.TabStop
            };

            this.btnNewCustomField = new Button
            {
                X = Pos.Left(fraCustomFieldList),
                Y = 37,  // Placeholder - updated by RelocateFooterControls
                Text = "_New Custom Field",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnNewCustomField.Accepting += NewCustomFieldButtonClicked;
            this.btnNewCustomField.HasFocusChanged += (s, e) => HandleFocusChange(btnNewCustomField, e);

            this.btnSave = new Button
            {
                X = Pos.Center() - 10,
                Y = 39,  // Placeholder - updated by RelocateFooterControls
                Text = "_Save",
                IsDefault = true,
                TabStop = TabBehavior.TabStop
            };
            this.btnSave.Accepting += SaveButtonClicked;
            this.btnSave.HasFocusChanged += (s, e) => HandleFocusChange(btnSave, e);

            this.btnCancel = new Button
            {
                X = Pos.Right(btnSave) + 2,
                Y = Pos.Top(btnSave),
                Text = "_Cancel",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnCancel.Accepting += CancelButtonClicked;
            this.btnCancel.HasFocusChanged += (s, e) => HandleFocusChange(btnCancel, e);

            this.stbDetail = new StatusBar
            {
                X = 0,
                Y = Pos.AnchorEnd(0),
                Width = Dim.Fill(),
                Height = 1,
                Visible = true,
                Data = "stbDetail",
                Text = "",
                TextAlignment = Alignment.Start
            };
        }
    }
}
