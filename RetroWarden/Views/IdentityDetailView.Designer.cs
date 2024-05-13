using System;
using Terminal.Gui;
using Terminal.Gui.Graphs;

namespace Retrowarden.Views
{
    public partial class IdentityDetailView
    {
        private View vwIdentity;
        private Label lblTitle;
        private ComboBox cboTitle;
        private Label lblFirstName;
        private Label lblMiddleName;
        private Label lblLastName;
        private TextField txtFirstName;
        private TextField txtMiddleName;
        private TextField txtLastName;
        private Label lblSSN;
        private Label lblPassportNumber;
        private Label lblLicenseNumber;
        private TextField txtSSN;
        private TextField txtPassportNumber;
        private TextField txtLicenseNumber;
        private LineView lineView2;
        private LineView lineView;
        private Label lblAddress1;
        private Label lblUserName;
        private TextField txtAddress1;
        private TextField txtUserName;
        private Label lblAddress2;
        private Label lblCompany;
        private TextField txtAddress2;
        private TextField txtCompany;
        private Label lblAddress3;
        private Label lblEmail;
        private TextField txtAddress3;
        private TextField txtEmailAddress;
        private Label lblCity;
        private Label lblState;
        private Label lblPhone;
        private TextField txtCity;
        private TextField txtState;
        private TextField txtPhoneNumber;
        private Label lblZipCode;
        private Label lblCountry;
        private TextField txtZipCode;
        private TextField txtCountry;

        private void InitializeComponent()
        {
            this.vwIdentity = new View();
            this.txtCountry = new TextField();
            this.txtZipCode = new TextField();
            this.lblCountry = new Label();
            this.lblZipCode = new Label();
            this.txtPhoneNumber = new TextField();
            this.txtState = new TextField();
            this.txtCity = new TextField();
            this.lblPhone = new Label();
            this.lblState = new Label();
            this.lblCity = new Label();
            this.txtEmailAddress = new TextField();
            this.txtAddress3 = new TextField();
            this.lblEmail = new Label();
            this.lblAddress3 = new Label();
            this.txtCompany = new TextField();
            this.txtAddress2 = new TextField();
            this.lblCompany = new Label();
            this.lblAddress2 = new Label();
            this.txtUserName = new TextField();
            this.txtAddress1 = new TextField();
            this.lblUserName = new Label();
            this.lblAddress1 = new Label();
            this.lineView = new LineView();
            this.lineView2 = new LineView();
            this.txtLicenseNumber = new TextField();
            this.txtPassportNumber = new TextField();
            this.txtSSN = new TextField();
            this.lblLicenseNumber = new Label();
            this.lblPassportNumber = new Label();
            this.lblSSN = new Label();
            this.txtLastName = new TextField();
            this.txtMiddleName = new TextField();
            this.txtFirstName = new TextField();
            this.lblLastName = new Label();
            this.lblMiddleName = new Label();
            this.lblFirstName = new Label();
            this.cboTitle = new ComboBox();
            this.lblTitle = new Label();

            vwIdentity.Width = 99;
            vwIdentity.Height = 25;
            vwIdentity.X = 0;
            vwIdentity.Y = 3;
            vwIdentity.Visible = true;
            vwIdentity.TextAlignment = TextAlignment.Left;

            this.lblTitle.Width = 3;
            this.lblTitle.Height = 1;
            this.lblTitle.X = 1;
            this.lblTitle.Y = 0;
            this.lblTitle.Visible = true;
            this.lblTitle.Data = "lblTitle";
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblTitle);

            this.cboTitle.Width = 23;
            this.cboTitle.Height = 5;
            this.cboTitle.X = 1;
            this.cboTitle.Y = 1;
            this.cboTitle.Visible = true;
            this.cboTitle.Data = "cboTitle";
            this.cboTitle.Text = "";
            this.cboTitle.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.cboTitle);

            this.lblFirstName.Width = 4;
            this.lblFirstName.Height = 1;
            this.lblFirstName.X = 1;
            this.lblFirstName.Y = 3;
            this.lblFirstName.Visible = true;
            this.lblFirstName.Data = "lblFirstName";
            this.lblFirstName.Text = "First Name";
            this.lblFirstName.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblFirstName);

            this.lblMiddleName.Width = 4;
            this.lblMiddleName.Height = 1;
            this.lblMiddleName.X = 33;
            this.lblMiddleName.Y = 3;
            this.lblMiddleName.Visible = true;
            this.lblMiddleName.Data = "lblMiddleName";
            this.lblMiddleName.Text = "Middle Name";
            this.lblMiddleName.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblMiddleName);

            this.lblLastName.Width = 4;
            this.lblLastName.Height = 1;
            this.lblLastName.X = 65;
            this.lblLastName.Y = 3;
            this.lblLastName.Visible = true;
            this.lblLastName.Data = "lblLastName";
            this.lblLastName.Text = "Last Name";
            this.lblLastName.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblLastName);

            this.txtFirstName.Width = 30;
            this.txtFirstName.Height = 1;
            this.txtFirstName.X = 1;
            this.txtFirstName.Y = 4;
            this.txtFirstName.Visible = true;
            this.txtFirstName.Secret = false;
            this.txtFirstName.Data = "txtFirstName";
            this.txtFirstName.Text = "";
            this.txtFirstName.TextAlignment = TextAlignment.Left;
            this.txtFirstName.Enter += (_) => HandleControlEnter(txtFirstName);
            vwIdentity.Add(this.txtFirstName);

            this.txtMiddleName.Width = 30;
            this.txtMiddleName.Height = 1;
            this.txtMiddleName.X = 33;
            this.txtMiddleName.Y = 4;
            this.txtMiddleName.Visible = true;
            this.txtMiddleName.Secret = false;
            this.txtMiddleName.Data = "txtMiddleName";
            this.txtMiddleName.Text = "";
            this.txtMiddleName.TextAlignment = TextAlignment.Left;
            this.txtMiddleName.Enter += (_) => HandleControlEnter(txtMiddleName);
            vwIdentity.Add(this.txtMiddleName);

            this.txtLastName.Width = 30;
            this.txtLastName.Height = 1;
            this.txtLastName.X = 65;
            this.txtLastName.Y = 4;
            this.txtLastName.Visible = true;
            this.txtLastName.Secret = false;
            this.txtLastName.Data = "txtLastName";
            this.txtLastName.Text = "";
            this.txtLastName.TextAlignment = TextAlignment.Left;
            this.txtLastName.Enter += (_) => HandleControlEnter(txtLastName);
            vwIdentity.Add(this.txtLastName);

            this.lblSSN.Width = 4;
            this.lblSSN.Height = 1;
            this.lblSSN.X = 1;
            this.lblSSN.Y = 6;
            this.lblSSN.Visible = true;
            this.lblSSN.Data = "lblSSN";
            this.lblSSN.Text = "Social Security Number";
            this.lblSSN.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblSSN);

            this.lblPassportNumber.Width = 30;
            this.lblPassportNumber.Height = 1;
            this.lblPassportNumber.X = 33;
            this.lblPassportNumber.Y = 6;
            this.lblPassportNumber.Visible = true;
            this.lblPassportNumber.Data = "lblPassportNumber";
            this.lblPassportNumber.Text = "Passport Number";
            this.lblPassportNumber.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblPassportNumber);

            this.lblLicenseNumber.Width = 4;
            this.lblLicenseNumber.Height = 1;
            this.lblLicenseNumber.X = 65;
            this.lblLicenseNumber.Y = 6;
            this.lblLicenseNumber.Visible = true;
            this.lblLicenseNumber.Data = "lblLicenseNumber";
            this.lblLicenseNumber.Text = "License Number";
            this.lblLicenseNumber.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblLicenseNumber);

            this.txtSSN.Width = 30;
            this.txtSSN.Height = 1;
            this.txtSSN.X = 1;
            this.txtSSN.Y = 7;
            this.txtSSN.Visible = true;
            this.txtSSN.Secret = false;
            this.txtSSN.Data = "txtSSN";
            this.txtSSN.Text = "";
            this.txtSSN.TextAlignment = TextAlignment.Left;
            this.txtSSN.Enter += (_) => HandleControlEnter(txtSSN);
            vwIdentity.Add(this.txtSSN);

            this.txtPassportNumber.Width = 30;
            this.txtPassportNumber.Height = 1;
            this.txtPassportNumber.X = 33;
            this.txtPassportNumber.Y = 7;
            this.txtPassportNumber.Visible = true;
            this.txtPassportNumber.Secret = false;
            this.txtPassportNumber.Data = "txtPassportNumber";
            this.txtPassportNumber.Text = "";
            this.txtPassportNumber.TextAlignment = TextAlignment.Left;
            this.txtPassportNumber.Enter += (_) => HandleControlEnter(txtPassportNumber);
            vwIdentity.Add(this.txtPassportNumber);

            this.txtLicenseNumber.Width = 30;
            this.txtLicenseNumber.Height = 1;
            this.txtLicenseNumber.X = 65;
            this.txtLicenseNumber.Y = 7;
            this.txtLicenseNumber.Visible = true;
            this.txtLicenseNumber.Secret = false;
            this.txtLicenseNumber.Data = "txtLicenseNumber";
            this.txtLicenseNumber.Text = "";
            this.txtLicenseNumber.TextAlignment = TextAlignment.Left;
            this.txtLicenseNumber.Enter += (_) => HandleControlEnter(txtLicenseNumber);
            vwIdentity.Add(this.txtLicenseNumber);

            this.lineView2.Width = 94;
            this.lineView2.Height = 3;
            this.lineView2.X = 1;
            this.lineView2.Y = 9;
            this.lineView2.Visible = true;
            this.lineView2.Data = "lineView2";
            this.lineView2.TextAlignment = TextAlignment.Left;
            this.lineView2.LineRune = '─';
            this.lineView2.Orientation = Orientation.Horizontal;
            vwIdentity.Add(this.lineView2);

            this.lineView.Width = 1;
            this.lineView.Height = 15;
            this.lineView.X = 47;
            this.lineView.Y = 9;
            this.lineView.Visible = true;
            this.lineView.Data = "lineView";
            this.lineView.TextAlignment = TextAlignment.Left;
            this.lineView.LineRune = '│';
            this.lineView.Orientation = Orientation.Vertical;
            vwIdentity.Add(this.lineView);

            this.lblAddress1.Width = 4;
            this.lblAddress1.Height = 1;
            this.lblAddress1.X = 1;
            this.lblAddress1.Y = 10;
            this.lblAddress1.Visible = true;
            this.lblAddress1.Data = "lblAddress1";
            this.lblAddress1.Text = "Address 1";
            this.lblAddress1.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblAddress1);

            this.txtAddress1.Width = 45;
            this.txtAddress1.Height = 1;
            this.txtAddress1.X = 1;
            this.txtAddress1.Y = 11;
            this.txtAddress1.Visible = true;
            this.txtAddress1.Secret = false;
            this.txtAddress1.Data = "txtAddress1";
            this.txtAddress1.Text = "";
            this.txtAddress1.TextAlignment = TextAlignment.Left;
            this.txtAddress1.Enter += (_) => HandleControlEnter(txtAddress1);
            vwIdentity.Add(this.txtAddress1);

            this.lblAddress2.Width = 4;
            this.lblAddress2.Height = 1;
            this.lblAddress2.X = 1;
            this.lblAddress2.Y = 13;
            this.lblAddress2.Visible = true;
            this.lblAddress2.Data = "lblAddress2";
            this.lblAddress2.Text = "Address 2";
            this.lblAddress2.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblAddress2);

            this.txtAddress2.Width = 45;
            this.txtAddress2.Height = 2;
            this.txtAddress2.X = 1;
            this.txtAddress2.Y = 14;
            this.txtAddress2.Visible = true;
            this.txtAddress2.Secret = false;
            this.txtAddress2.Data = "txtAddress2";
            this.txtAddress2.Text = "";
            this.txtAddress2.TextAlignment = TextAlignment.Left;
            this.txtAddress2.Enter += (_) => HandleControlEnter(txtAddress2);
            vwIdentity.Add(this.txtAddress2);

            this.lblAddress3.Width = 4;
            this.lblAddress3.Height = 1;
            this.lblAddress3.X = 1;
            this.lblAddress3.Y = 16;
            this.lblAddress3.Visible = true;
            this.lblAddress3.Data = "lblAddress3";
            this.lblAddress3.Text = "Address 3";
            this.lblAddress3.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblAddress3);

            this.txtAddress3.Width = 45;
            this.txtAddress3.Height = 1;
            this.txtAddress3.X = 1;
            this.txtAddress3.Y = 17;
            this.txtAddress3.Visible = true;
            this.txtAddress3.Secret = false;
            this.txtAddress3.Data = "txtAddress3";
            this.txtAddress3.Text = "";
            this.txtAddress3.TextAlignment = TextAlignment.Left;
            this.txtAddress3.Enter += (_) => HandleControlEnter(txtAddress3);
            vwIdentity.Add(this.txtAddress3);


            this.lblCity.Width = 4;
            this.lblCity.Height = 1;
            this.lblCity.X = 1;
            this.lblCity.Y = 19;
            this.lblCity.Visible = true;
            this.lblCity.Data = "lblCity";
            this.lblCity.Text = "City / Town";
            this.lblCity.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblCity);

            this.lblState.Width = 4;
            this.lblState.Height = 1;
            this.lblState.X = 25;
            this.lblState.Y = 19;
            this.lblState.Visible = true;
            this.lblState.Data = "lblState";
            this.lblState.Text = "State / Province";
            this.lblState.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblState);

            this.txtCity.Width = 21;
            this.txtCity.Height = 1;
            this.txtCity.X = 1;
            this.txtCity.Y = 20;
            this.txtCity.Visible = true;
            this.txtCity.Secret = false;
            this.txtCity.Data = "txtCity";
            this.txtCity.Text = "";
            this.txtCity.TextAlignment = TextAlignment.Left;
            this.txtCity.Enter += (_) => HandleControlEnter(txtCity);
            vwIdentity.Add(this.txtCity);

            this.txtState.Width = 21;
            this.txtState.Height = 1;
            this.txtState.X = 25;
            this.txtState.Y = 20;
            this.txtState.Visible = true;
            this.txtState.Secret = false;
            this.txtState.Data = "txtState";
            this.txtState.Text = "";
            this.txtState.TextAlignment = TextAlignment.Left;
            this.txtState.Enter += (_) => HandleControlEnter(txtState);
            vwIdentity.Add(this.txtState);

            this.lblZipCode.Width = 4;
            this.lblZipCode.Height = 1;
            this.lblZipCode.X = 1;
            this.lblZipCode.Y = 22;
            this.lblZipCode.Visible = true;
            this.lblZipCode.Data = "lblZipCode";
            this.lblZipCode.Text = "Zip / Postal Code";
            this.lblZipCode.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblZipCode);

            this.lblCountry.Width = 4;
            this.lblCountry.Height = 1;
            this.lblCountry.X = 25;
            this.lblCountry.Y = 22;
            this.lblCountry.Visible = true;
            this.lblCountry.Data = "lblCountry";
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblCountry);

            this.txtZipCode.Width = 21;
            this.txtZipCode.Height = 1;
            this.txtZipCode.X = 1;
            this.txtZipCode.Y = 23;
            this.txtZipCode.Visible = true;
            this.txtZipCode.Secret = false;
            this.txtZipCode.Data = "txtZipCode";
            this.txtZipCode.Text = "";
            this.txtZipCode.TextAlignment = TextAlignment.Left;
            this.txtZipCode.Enter += (_) => HandleControlEnter(txtZipCode);
            vwIdentity.Add(this.txtZipCode);

            this.txtCountry.Width = 21;
            this.txtCountry.Height = 1;
            this.txtCountry.X = 25;
            this.txtCountry.Y = 23;
            this.txtCountry.Visible = true;
            this.txtCountry.Secret = false;
            this.txtCountry.Data = "txtCountry";
            this.txtCountry.Text = "";
            this.txtCountry.TextAlignment = TextAlignment.Left;
            this.txtCountry.Enter += (_) => HandleControlEnter(txtCountry);
            vwIdentity.Add(this.txtCountry);

            this.lblUserName.Width = 10;
            this.lblUserName.Height = 1;
            this.lblUserName.X = 49;
            this.lblUserName.Y = 10;
            this.lblUserName.Visible = true;
            this.lblUserName.Data = "lblUserName";
            this.lblUserName.Text = "User Name";
            this.lblUserName.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblUserName);

            this.txtUserName.Width = 45;
            this.txtUserName.Height = 1;
            this.txtUserName.X = 49;
            this.txtUserName.Y = 11;
            this.txtUserName.Visible = true;
            this.txtUserName.Secret = false;
            this.txtUserName.Data = "txtUserName";
            this.txtUserName.Text = "";
            this.txtUserName.TextAlignment = TextAlignment.Left;
            txtUserName.Enter += (_) => HandleControlEnter(txtUserName);
            vwIdentity.Add(this.txtUserName);

            this.lblCompany.Width = 4;
            this.lblCompany.Height = 1;
            this.lblCompany.X = 49;
            this.lblCompany.Y = 13;
            this.lblCompany.Visible = true;
            this.lblCompany.Data = "lblCompany";
            this.lblCompany.Text = "Company";
            this.lblCompany.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblCompany);

            this.txtCompany.Width = 45;
            this.txtCompany.Height = 1;
            this.txtCompany.X = 49;
            this.txtCompany.Y = 14;
            this.txtCompany.Visible = true;
            this.txtCompany.Secret = false;
            this.txtCompany.Data = "txtCompany";
            this.txtCompany.Text = "";
            this.txtCompany.TextAlignment = TextAlignment.Left;
            this.txtCompany.Enter += (_) => HandleControlEnter(txtCompany);
            vwIdentity.Add(this.txtCompany);

            this.lblEmail.Width = 4;
            this.lblEmail.Height = 1;
            this.lblEmail.X = 49;
            this.lblEmail.Y = 16;
            this.lblEmail.Visible = true;
            this.lblEmail.Data = "lblEmail";
            this.lblEmail.Text = "Email Address";
            this.lblEmail.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblEmail);

            this.txtEmailAddress.Width = 45;
            this.txtEmailAddress.Height = 1;
            this.txtEmailAddress.X = 49;
            this.txtEmailAddress.Y = 17;
            this.txtEmailAddress.Visible = true;
            this.txtEmailAddress.Secret = false;
            this.txtEmailAddress.Data = "txtEmailAddress";
            this.txtEmailAddress.Text = "";
            this.txtEmailAddress.TextAlignment = TextAlignment.Left;
            this.txtEmailAddress.Enter += (_) => HandleControlEnter(txtEmailAddress);
            vwIdentity.Add(this.txtEmailAddress);

            this.lblPhone.Width = 1;
            this.lblPhone.Height = 1;
            this.lblPhone.X = 49;
            this.lblPhone.Y = 19;
            this.lblPhone.Visible = true;
            this.lblPhone.Data = "lblPhone";
            this.lblPhone.Text = "Phone Number";
            this.lblPhone.TextAlignment = TextAlignment.Left;
            vwIdentity.Add(this.lblPhone);

            this.txtPhoneNumber.Width = 45;
            this.txtPhoneNumber.Height = 1;
            this.txtPhoneNumber.X = 49;
            this.txtPhoneNumber.Y = 20;
            this.txtPhoneNumber.Visible = true;
            this.txtPhoneNumber.Secret = false;
            this.txtPhoneNumber.Data = "txtPhoneNumber";
            this.txtPhoneNumber.Text = "";
            this.txtPhoneNumber.TextAlignment = TextAlignment.Left;
            this.txtPhoneNumber.Enter += (_) => HandleControlEnter(txtPhoneNumber);
            vwIdentity.Add(this.txtPhoneNumber);
        }
    }
}
