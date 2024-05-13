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
            
            vwCard.Width = 99;
            vwCard.Height = 9;
            vwCard.X = 0;
            vwCard.Y = 3;
            vwCard.Visible = true;
            vwCard.TextAlignment = TextAlignment.Left;
                       
            this.lblCardholderName.Width = 4;
            this.lblCardholderName.Height = 1;
            this.lblCardholderName.X = 1;
            this.lblCardholderName.Y = 0;
            this.lblCardholderName.Visible = true;
            this.lblCardholderName.Data = "lblCardholderName";
            this.lblCardholderName.Text = "Cardholder Name";
            this.lblCardholderName.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblCardholderName);
            
            this.lblCardBrand.Width = 4;
            this.lblCardBrand.Height = 1;
            this.lblCardBrand.X = 40;
            this.lblCardBrand.Y = 0;
            this.lblCardBrand.Visible = true;
            this.lblCardBrand.Data = "lblCardBrand";
            this.lblCardBrand.Text = "Card Brand";
            this.lblCardBrand.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblCardBrand);
           
            this.txtCardholderName.Width = 30;
            this.txtCardholderName.Height = 1;
            this.txtCardholderName.X = 1;
            this.txtCardholderName.Y = 1;
            this.txtCardholderName.Visible = true;
            this.txtCardholderName.Secret = false;
            this.txtCardholderName.Data = "txtCardholderName";
            this.txtCardholderName.Text = "";
            this.txtCardholderName.TextAlignment = TextAlignment.Left;
            this.txtCardholderName.Enter += (_) => HandleControlEnter(txtCardholderName);
            vwCard.Add(this.txtCardholderName);
            
            this.cboCardBrand.Width = 30;
            this.cboCardBrand.Height = 5;
            this.cboCardBrand.X = 40;
            this.cboCardBrand.Y = 1;
            this.cboCardBrand.Visible = true;
            this.cboCardBrand.Data = "cboCardBrand";
            this.cboCardBrand.Text = "";
            this.cboCardBrand.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.cboCardBrand);
            
            this.lblCardNumber.Width = 4;
            this.lblCardNumber.Height = 1;
            this.lblCardNumber.X = 1;
            this.lblCardNumber.Y = 3;
            this.lblCardNumber.Visible = true;
            this.lblCardNumber.Data = "lblCardNumber";
            this.lblCardNumber.Text = "Card Number";
            this.lblCardNumber.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblCardNumber);
            
            this.lblCVV.Width = 4;
            this.lblCVV.Height = 1;
            this.lblCVV.X = 40;
            this.lblCVV.Y = 3;
            this.lblCVV.Visible = true;
            this.lblCVV.Data = "lblCVV";
            this.lblCVV.Text = "Security Code";
            this.lblCVV.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblCVV);
           
            this.txtCardNumber.Width = 18;
            this.txtCardNumber.Height = 1;
            this.txtCardNumber.X = 1;
            this.txtCardNumber.Y = 4;
            this.txtCardNumber.Visible = true;
            this.txtCardNumber.Secret = true;
            this.txtCardNumber.Data = "txtCardNumber";
            this.txtCardNumber.Text = "";
            this.txtCardNumber.TextAlignment = TextAlignment.Left;
            this.txtCardNumber.Enter += (_) => HandleControlEnter(txtCardNumber);
            vwCard.Add(this.txtCardNumber);
            
            this.btnShowCardNumber.Width = 8;
            this.btnShowCardNumber.Height = 1;
            this.btnShowCardNumber.X = 20;
            this.btnShowCardNumber.Y = 4;
            this.btnShowCardNumber.Visible = true;
            this.btnShowCardNumber.Data = "btnShowCardNumber";
            this.btnShowCardNumber.Text = "Show";
            this.btnShowCardNumber.TextAlignment = TextAlignment.Centered;
            this.btnShowCardNumber.IsDefault = false;
            this.btnShowCardNumber.Clicked += ShowCardButtonClicked;
            vwCard.Add(this.btnShowCardNumber);
            
            this.btnCopyCardNumber.Width = 8;
            this.btnCopyCardNumber.Height = 1;
            this.btnCopyCardNumber.X = 29;
            this.btnCopyCardNumber.Y = 4;
            this.btnCopyCardNumber.Visible = true;
            this.btnCopyCardNumber.Data = "btnCopyCardNumber";
            this.btnCopyCardNumber.Text = "Copy";
            this.btnCopyCardNumber.TextAlignment = TextAlignment.Centered;
            this.btnCopyCardNumber.IsDefault = false;
            this.btnCopyCardNumber.Clicked += CopyCardButtonClicked;
            vwCard.Add(this.btnCopyCardNumber);
            
            this.txtCVV.Width = 16;
            this.txtCVV.Height = 1;
            this.txtCVV.X = 40;
            this.txtCVV.Y = 4;
            this.txtCVV.Visible = true;
            this.txtCVV.Secret = true;
            this.txtCVV.Data = "txtCVV";
            this.txtCVV.Text = "";
            this.txtCVV.TextAlignment = TextAlignment.Left;
            this.txtCVV.Enter += (_) => HandleControlEnter(txtCVV);
            vwCard.Add(this.txtCVV);
            
            this.btnShowCVV.Width = 8;
            this.btnShowCVV.Height = 1;
            this.btnShowCVV.X = 57;
            this.btnShowCVV.Y = 4;
            this.btnShowCVV.Visible = true;
            this.btnShowCVV.Data = "btnShowCVV";
            this.btnShowCVV.Text = "Show";
            this.btnShowCVV.TextAlignment = TextAlignment.Centered;
            this.btnShowCVV.IsDefault = false;
            this.btnShowCVV.Clicked += ShowCVVButtonClicked;
            vwCard.Add(this.btnShowCVV);
            
            this.btnCopyCVV.Width = 8;
            this.btnCopyCVV.Height = 1;
            this.btnCopyCVV.X = 66;
            this.btnCopyCVV.Y = 4;
            this.btnCopyCVV.Visible = true;
            this.btnCopyCVV.Data = "btnCopyCVV";
            this.btnCopyCVV.Text = "Copy";
            this.btnCopyCVV.TextAlignment = TextAlignment.Centered;
            this.btnCopyCVV.IsDefault = false;
            this.btnCopyCVV.Clicked += CopyCVVButtonClicked;
            vwCard.Add(this.btnCopyCVV);
            
            this.lblExpMonth.Width = 4;
            this.lblExpMonth.Height = 1;
            this.lblExpMonth.X = 1;
            this.lblExpMonth.Y = 6;
            this.lblExpMonth.Visible = true;
            this.lblExpMonth.Data = "lblExpMonth";
            this.lblExpMonth.Text = "Expiration Month";
            this.lblExpMonth.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblExpMonth);
            
            this.lblExpYear.Width = 4;
            this.lblExpYear.Height = 1;
            this.lblExpYear.X = 40;
            this.lblExpYear.Y = 6;
            this.lblExpYear.Visible = true;
            this.lblExpYear.Data = "lblExpYear";
            this.lblExpYear.Text = "Expiration Year";
            this.lblExpYear.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.lblExpYear);
            
            this.cboExpMonth.Width = 30;
            this.cboExpMonth.Height = 5;
            this.cboExpMonth.X = 1;
            this.cboExpMonth.Y = 7;
            this.cboExpMonth.Visible = true;
            this.cboExpMonth.Data = "cboExpMonth";
            this.cboExpMonth.Text = "";
            this.cboExpMonth.TextAlignment = TextAlignment.Left;
            vwCard.Add(this.cboExpMonth);
            
            this.txtExpYear.Width = 20;
            this.txtExpYear.Height = 1;
            this.txtExpYear.X = 40;
            this.txtExpYear.Y = 7;
            this.txtExpYear.Visible = true;
            this.txtExpYear.Secret = false;
            this.txtExpYear.Data = "txtExpYear";
            this.txtExpYear.Text = "";
            this.txtExpYear.TextAlignment = TextAlignment.Left;
            this.txtExpYear.Enter += (_) => HandleControlEnter(txtExpYear);
            vwCard.Add(this.txtExpYear);
        }
    }
}
