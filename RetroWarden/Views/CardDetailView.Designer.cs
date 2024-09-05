using System;
using Terminal.Gui;

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
            this.vwCard = new View();
            this.txtExpYear = new TextField();
            this.cboExpMonth = new ComboBox();
            this.lblExpYear = new Label();
            this.lblExpMonth = new Label();
            this.btnCopyCVV = new Button();
            this.btnShowCVV = new Button();
            this.txtCVV = new TextField();
            this.btnCopyCardNumber = new Button();
            this.btnShowCardNumber = new Button();
            this.txtCardNumber = new TextField();
            this.lblCVV = new Label();
            this.lblCardNumber = new Label();
            this.cboCardBrand = new ComboBox();
            this.txtCardholderName = new TextField();
            this.lblCardBrand = new Label();
            this.lblCardholderName = new Label();
            
            vwCard = new View
            {
                Width = 99, Height = 9, X = 0, Y = 3, Visible = true, TextAlignment = Alignment.Start
            };                      
            
            this.lblCardholderName = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 0, Visible = true, Data = "lblCardholderName", Text = "Cardholder Name",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardholderName);

            this.lblCardBrand = new Label
            {
                Width = 12, Height = 1, X = 40, Y = 0, Visible = true, Data = "lblCardBrand", Text = "Card Brand",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardBrand);

            this.txtCardholderName = new TextField
            {
                Width = 30, Height = 1, X = 1, Y = 1, Visible = true, Secret = false, Data = "txtCardholderName", Text = "",
                TextAlignment = Alignment.Start
            };
            this.txtCardholderName.Enter += (s, e) => HandleControlEnter(txtCardholderName);
            vwCard.Add(this.txtCardholderName);

            this.cboCardBrand = new ComboBox
            {
                Width = 30, Height = 5, X = 40, Y = 1, Visible = true, Data = "cboCardBrand", Text = "",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.cboCardBrand);

            this.lblCardNumber = new Label
            {
                Width = 12, Height = 1, X = 1, Y = 3, Visible = true, Data = "lblCardNumber", Text = "Card Number",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCardNumber);

            this.lblCVV = new Label
            {
                Width = 14, Height = 1, X = 40, Y = 3, Visible = true, Data = "lblCVV", Text = "Security Code",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblCVV);

            this.txtCardNumber = new TextField
            {
                Width = 18, Height = 1, X = 1, Y = 4, Visible = true, Secret = true, Data = "txtCardNumber", Text = "",
                TextAlignment = Alignment.Start
            };
            this.txtCardNumber.Enter += (s, e) => HandleControlEnter(txtCardNumber);
            vwCard.Add(this.txtCardNumber);

            this.btnShowCardNumber = new Button
            {
                Width = 8, Height = 1, X = 20, Y = 4, Visible = true, Data = "btnShowCardNumber", Text = "Show",
                TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnShowCardNumber.Accept += ShowCardButtonClicked;
            vwCard.Add(this.btnShowCardNumber);

            this.btnCopyCardNumber = new Button
            {
                Width = 8, Height = 1, X = 29, Y = 4, Visible = true, Data = "btnCopyCardNumber", Text = "Copy",
                TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnCopyCardNumber.Accept += CopyCardButtonClicked;
            vwCard.Add(this.btnCopyCardNumber);

            this.txtCVV = new TextField
            {
                Width = 16, Height = 1, X = 40, Y = 4, Visible = true, Secret = true, Data = "txtCVV", Text = "",
                TextAlignment = Alignment.Start
            };
            this.txtCVV.Enter += (s, e) => HandleControlEnter(txtCVV);
            vwCard.Add(this.txtCVV);

            this.btnShowCVV = new Button
            {
                Width = 8, Height = 1, X = 57, Y = 4, Visible = true, Data = "btnShowCVV", Text = "Show",
                TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnShowCVV.Accept += ShowCVVButtonClicked;
            vwCard.Add(this.btnShowCVV);

            this.btnCopyCVV = new Button
            {
                Width = 8, Height = 1, X = 66, Y = 4, Visible = true, Data = "btnCopyCVV", Text = "Copy",
                TextAlignment = Alignment.Center, IsDefault = false
            };
            this.btnCopyCVV.Accept += CopyCVVButtonClicked;
            vwCard.Add(this.btnCopyCVV);

            this.lblExpMonth = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 6, Visible = true, Data = "lblExpMonth", Text = "Expiration Month",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblExpMonth);

            this.lblExpYear = new Label
            {
                Width = 16, Height = 1, X = 40, Y = 6, Visible = true, Data = "lblExpYear", Text = "Expiration Year",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.lblExpYear);

            this.cboExpMonth = new ComboBox
            {
                Width = 30, Height = 5, X = 1, Y = 7, Visible = true, Data = "cboExpMonth", Text = "",
                TextAlignment = Alignment.Start
            };
            vwCard.Add(this.cboExpMonth);

            this.txtExpYear = new TextField
            {
                Width = 20, Height = 1, X = 40, Y = 7, Visible = true, Secret = false, Data = "txtExpYear", Text = "",
                TextAlignment = Alignment.Start
            };
            this.txtExpYear.Enter += (s, e) => HandleControlEnter(txtExpYear);
            vwCard.Add(this.txtExpYear);        }
    }
}
