using System;
using Terminal.Gui;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace Retrowarden.Views
{
    public partial class CardDetailView
    {
        private View vwCard;
        private Label lblCardholderName;
        private Label lblCardBrand;
        private TextField txtCardholderName;
        private ComboBox cboCardBrand;
        private Label lblCardNumber;
        private Label lblCVV;
        private TextField txtCardNumber;
        private Button btnShowCardNumber;
        private Button btnCopyCardNumber;
        private TextField txtCVV;
        private Button btnShowCVV;
        private Button btnCopyCVV;
        private Label lblExpMonth;
        private Label lblExpYear;
        private ComboBox cboExpMonth;
        private TextField txtExpYear;

        private void InitializeComponent()
        {
            // Container view for card-specific controls
            // Y=3 positions it below the header row (lblItemName/txtItemName at Y=0/1)
            this.vwCard = new View
            {
                X = 0,
                Y = 3,
                Width = Dim.Fill(),
                Height = 9,
                Visible = true,
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop,
                CanFocus = true
            };
            // Disable scrollbars on subview
            vwCard.HorizontalScrollBar.Visible = false;
            vwCard.HorizontalScrollBar.AutoShow = false;

            // ===== ROW 0: Cardholder Name and Card Brand labels =====
            this.lblCardholderName = new Label
            {
                X = 1,
                Y = 0,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCardholderName",
                Text = "Cardholder Name",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardholderName);

            this.lblCardBrand = new Label
            {
                X = 40,
                Y = Pos.Top(lblCardholderName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCardBrand",
                Text = "Card Brand",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardBrand);

            // ===== ROW 1: Cardholder Name and Card Brand inputs =====
            this.txtCardholderName = new TextField
            {
                X = Pos.Left(lblCardholderName),
                Y = Pos.Bottom(lblCardholderName),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtCardholderName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCardholderName.HasFocusChanged += (s, e) => HandleFocusChange(txtCardholderName, e);
            vwCard.Add(this.txtCardholderName);

            // Note: Height=1 for just the input field; dropdown renders as overlay in Terminal.Gui v2
            this.cboCardBrand = new ComboBox
            {
                X = Pos.Left(lblCardBrand),
                Y = Pos.Top(txtCardholderName),
                Width = 30,
                Height = 1,
                Visible = true,
                Data = "cboCardBrand",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.cboCardBrand.HasFocusChanged += (s, e) => HandleFocusChange(cboCardBrand, e);
            vwCard.Add(this.cboCardBrand);

            // ===== ROW 2: Card Number and CVV labels =====
            this.lblCardNumber = new Label
            {
                X = Pos.Left(lblCardholderName),
                Y = Pos.Bottom(txtCardholderName) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCardNumber",
                Text = "Card Number",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardNumber);

            this.lblCVV = new Label
            {
                X = Pos.Left(lblCardBrand),
                Y = Pos.Top(lblCardNumber),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCVV",
                Text = "Security Code",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCVV);

            // ===== ROW 3: Card Number input with buttons, CVV input with buttons =====
            this.txtCardNumber = new TextField
            {
                X = Pos.Left(lblCardNumber),
                Y = Pos.Bottom(lblCardNumber),
                Width = 18,
                Height = 1,
                Visible = true,
                Secret = true,
                Data = "txtCardNumber",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCardNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtCardNumber, e);
            vwCard.Add(this.txtCardNumber);

            this.btnShowCardNumber = new Button
            {
                X = Pos.Right(txtCardNumber) + 1,
                Y = Pos.Top(txtCardNumber),
                Text = "_Show",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnShowCardNumber.Accepting += ShowCardButtonClicked;
            this.btnShowCardNumber.HasFocusChanged += (s, e) => HandleFocusChange(btnShowCardNumber, e);
            vwCard.Add(this.btnShowCardNumber);

            this.btnCopyCardNumber = new Button
            {
                X = Pos.Right(btnShowCardNumber) + 1,
                Y = Pos.Top(txtCardNumber),
                Text = "C_opy",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnCopyCardNumber.Accepting += CopyCardButtonClicked;
            this.btnCopyCardNumber.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyCardNumber, e);
            vwCard.Add(this.btnCopyCardNumber);

            this.txtCVV = new TextField
            {
                X = Pos.Left(lblCVV),
                Y = Pos.Top(txtCardNumber),
                Width = 16,
                Height = 1,
                Visible = true,
                Secret = true,
                Data = "txtCVV",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCVV.HasFocusChanged += (s, e) => HandleFocusChange(txtCVV, e);
            vwCard.Add(this.txtCVV);

            this.btnShowCVV = new Button
            {
                X = Pos.Right(txtCVV) + 1,
                Y = Pos.Top(txtCVV),
                Text = "S_how",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnShowCVV.Accepting += ShowCVVButtonClicked;
            this.btnShowCVV.HasFocusChanged += (s, e) => HandleFocusChange(btnShowCVV, e);
            vwCard.Add(this.btnShowCVV);

            this.btnCopyCVV = new Button
            {
                X = Pos.Right(btnShowCVV) + 1,
                Y = Pos.Top(txtCVV),
                Text = "Co_py",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnCopyCVV.Accepting += CopyCVVButtonClicked;
            this.btnCopyCVV.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyCVV, e);
            vwCard.Add(this.btnCopyCVV);

            // ===== ROW 4: Expiration Month and Year labels =====
            this.lblExpMonth = new Label
            {
                X = Pos.Left(lblCardholderName),
                Y = Pos.Bottom(txtCardNumber) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblExpMonth",
                Text = "Expiration Month",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblExpMonth);

            this.lblExpYear = new Label
            {
                X = Pos.Left(lblCardBrand),
                Y = Pos.Top(lblExpMonth),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblExpYear",
                Text = "Expiration Year",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblExpYear);

            // ===== ROW 5: Expiration Month and Year inputs =====
            // Note: Height=1 for just the input field; dropdown renders as overlay in Terminal.Gui v2
            this.cboExpMonth = new ComboBox
            {
                X = Pos.Left(lblExpMonth),
                Y = Pos.Bottom(lblExpMonth),
                Width = 30,
                Height = 1,
                Visible = true,
                Data = "cboExpMonth",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.cboExpMonth.HasFocusChanged += (s, e) => HandleFocusChange(cboExpMonth, e);
            vwCard.Add(this.cboExpMonth);

            this.txtExpYear = new TextField
            {
                X = Pos.Left(lblExpYear),
                Y = Pos.Top(cboExpMonth),
                Width = 20,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtExpYear",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtExpYear.HasFocusChanged += (s, e) => HandleFocusChange(txtExpYear, e);
            vwCard.Add(this.txtExpYear);
        }
    }
}
