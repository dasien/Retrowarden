using System;
using System.Text;
using Terminal.Gui;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

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
        private Line lineView2;
        private Line lineView;
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

        // Column anchor positions
        private const int Col1 = 1;
        private const int Col2 = 25;
        private const int Col3 = 33;
        private const int Col4 = 49;
        private const int Col5 = 65;

        private void InitializeComponent()
        {
            // Container view for identity-specific controls
            vwIdentity = new View
            {
                X = 0,
                Y = 3,
                Width = Dim.Fill(),
                Height = 25,
                Visible = true,
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop,
                CanFocus = true
            };
            // Disable scrollbars on subview
            vwIdentity.HorizontalScrollBar.Visible = false;
            vwIdentity.HorizontalScrollBar.AutoShow = false;

            // ========== TOP SECTION: Personal Information ==========

            // ===== ROW 0: Title label =====
            this.lblTitle = new Label
            {
                X = Col1,
                Y = 0,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblTitle",
                Text = "Title",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblTitle);

            // ===== ROW 1: Title combo =====
            // Note: Height=1 for just the input field; dropdown renders as overlay in Terminal.Gui v2
            this.cboTitle = new ComboBox
            {
                X = Pos.Left(lblTitle),
                Y = Pos.Bottom(lblTitle),
                Width = 23,
                Height = 1,
                Visible = true,
                Data = "cboTitle",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.cboTitle.HasFocusChanged += (s, e) => HandleFocusChange(cboTitle, e);
            vwIdentity.Add(this.cboTitle);

            // ===== ROW 2: Name labels (3 columns) =====
            // Position immediately below the ComboBox (dropdown is an overlay in v2)
            this.lblFirstName = new Label
            {
                X = Col1,
                Y = Pos.Bottom(cboTitle) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblFirstName",
                Text = "First Name",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblFirstName);

            this.lblMiddleName = new Label
            {
                X = Col3,
                Y = Pos.Top(lblFirstName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblMiddleName",
                Text = "Middle Name",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblMiddleName);

            this.lblLastName = new Label
            {
                X = Col5,
                Y = Pos.Top(lblFirstName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblLastName",
                Text = "Last Name",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblLastName);

            // ===== ROW 3: Name inputs (3 columns) =====
            this.txtFirstName = new TextField
            {
                X = Pos.Left(lblFirstName),
                Y = Pos.Bottom(lblFirstName),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtFirstName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtFirstName.HasFocusChanged += (s, e) => HandleFocusChange(txtFirstName, e);
            vwIdentity.Add(this.txtFirstName);

            this.txtMiddleName = new TextField
            {
                X = Pos.Left(lblMiddleName),
                Y = Pos.Top(txtFirstName),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtMiddleName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtMiddleName.HasFocusChanged += (s, e) => HandleFocusChange(txtMiddleName, e);
            vwIdentity.Add(this.txtMiddleName);

            this.txtLastName = new TextField
            {
                X = Pos.Left(lblLastName),
                Y = Pos.Top(txtFirstName),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtLastName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtLastName.HasFocusChanged += (s, e) => HandleFocusChange(txtLastName, e);
            vwIdentity.Add(this.txtLastName);

            // ===== ROW 4: ID labels (3 columns) =====
            this.lblSSN = new Label
            {
                X = Col1,
                Y = Pos.Bottom(txtFirstName) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblSSN",
                Text = "Social Security Number",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblSSN);

            this.lblPassportNumber = new Label
            {
                X = Col3,
                Y = Pos.Top(lblSSN),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblPassportNumber",
                Text = "Passport Number",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblPassportNumber);

            this.lblLicenseNumber = new Label
            {
                X = Col5,
                Y = Pos.Top(lblSSN),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblLicenseNumber",
                Text = "License Number",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblLicenseNumber);

            // ===== ROW 5: ID inputs (3 columns) =====
            this.txtSSN = new TextField
            {
                X = Pos.Left(lblSSN),
                Y = Pos.Bottom(lblSSN),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtSSN",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtSSN.HasFocusChanged += (s, e) => HandleFocusChange(txtSSN, e);
            vwIdentity.Add(this.txtSSN);

            this.txtPassportNumber = new TextField
            {
                X = Pos.Left(lblPassportNumber),
                Y = Pos.Top(txtSSN),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtPassportNumber",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtPassportNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtPassportNumber, e);
            vwIdentity.Add(this.txtPassportNumber);

            this.txtLicenseNumber = new TextField
            {
                X = Pos.Left(lblLicenseNumber),
                Y = Pos.Top(txtSSN),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtLicenseNumber",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtLicenseNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtLicenseNumber, e);
            vwIdentity.Add(this.txtLicenseNumber);

            // ========== DIVIDERS ==========
            this.lineView2 = new Line
            {
                X = Col1,
                Y = Pos.Bottom(txtSSN) + 1,
                Width = 94,
                Height = 3,
                Visible = true,
                Data = "lineView2",
                TextAlignment = Alignment.Start,
                Orientation = Orientation.Horizontal,
                CanFocus = false
            };
            vwIdentity.Add(this.lineView2);

            this.lineView = new Line
            {
                X = 47,
                Y = Pos.Top(lineView2),
                Width = 1,
                Height = 15,
                Visible = true,
                Data = "lineView",
                TextAlignment = Alignment.Start,
                Orientation = Orientation.Vertical,
                CanFocus = false
            };
            vwIdentity.Add(this.lineView);

            // ========== LEFT COLUMN: Address Information ==========

            // ===== Address 1 =====
            this.lblAddress1 = new Label
            {
                X = Col1,
                Y = Pos.Top(lineView2) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblAddress1",
                Text = "Address 1",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblAddress1);

            this.txtAddress1 = new TextField
            {
                X = Pos.Left(lblAddress1),
                Y = Pos.Bottom(lblAddress1),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtAddress1",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtAddress1.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress1, e);
            vwIdentity.Add(this.txtAddress1);

            // ===== Address 2 =====
            this.lblAddress2 = new Label
            {
                X = Pos.Left(lblAddress1),
                Y = Pos.Bottom(txtAddress1) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblAddress2",
                Text = "Address 2",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblAddress2);

            this.txtAddress2 = new TextField
            {
                X = Pos.Left(lblAddress2),
                Y = Pos.Bottom(lblAddress2),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtAddress2",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtAddress2.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress2, e);
            vwIdentity.Add(this.txtAddress2);

            // ===== Address 3 =====
            this.lblAddress3 = new Label
            {
                X = Pos.Left(lblAddress1),
                Y = Pos.Bottom(txtAddress2) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblAddress3",
                Text = "Address 3",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblAddress3);

            this.txtAddress3 = new TextField
            {
                X = Pos.Left(lblAddress3),
                Y = Pos.Bottom(lblAddress3),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtAddress3",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtAddress3.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress3, e);
            vwIdentity.Add(this.txtAddress3);

            // ===== City / State =====
            this.lblCity = new Label
            {
                X = Pos.Left(lblAddress1),
                Y = Pos.Bottom(txtAddress3) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCity",
                Text = "City / Town",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblCity);

            this.lblState = new Label
            {
                X = Col2,
                Y = Pos.Top(lblCity),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblState",
                Text = "State / Province",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblState);

            this.txtCity = new TextField
            {
                X = Pos.Left(lblCity),
                Y = Pos.Bottom(lblCity),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtCity",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCity.HasFocusChanged += (s, e) => HandleFocusChange(txtCity, e);
            vwIdentity.Add(this.txtCity);

            this.txtState = new TextField
            {
                X = Pos.Left(lblState),
                Y = Pos.Top(txtCity),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtState",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtState.HasFocusChanged += (s, e) => HandleFocusChange(txtState, e);
            vwIdentity.Add(this.txtState);

            // ===== Zip / Country =====
            this.lblZipCode = new Label
            {
                X = Pos.Left(lblCity),
                Y = Pos.Bottom(txtCity) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblZipCode",
                Text = "Zip / Postal Code",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblZipCode);

            this.lblCountry = new Label
            {
                X = Col2,
                Y = Pos.Top(lblZipCode),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCountry",
                Text = "Country",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblCountry);

            this.txtZipCode = new TextField
            {
                X = Pos.Left(lblZipCode),
                Y = Pos.Bottom(lblZipCode),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtZipCode",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtZipCode.HasFocusChanged += (s, e) => HandleFocusChange(txtZipCode, e);
            vwIdentity.Add(this.txtZipCode);

            this.txtCountry = new TextField
            {
                X = Pos.Left(lblCountry),
                Y = Pos.Top(txtZipCode),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtCountry",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCountry.HasFocusChanged += (s, e) => HandleFocusChange(txtCountry, e);
            vwIdentity.Add(this.txtCountry);

            // ========== RIGHT COLUMN: Contact Information ==========

            // ===== Username =====
            this.lblUserName = new Label
            {
                X = Col4,
                Y = Pos.Top(lblAddress1),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblUserName",
                Text = "User Name",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblUserName);

            this.txtUserName = new TextField
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Bottom(lblUserName),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtUserName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtUserName.HasFocusChanged += (s, e) => HandleFocusChange(txtUserName, e);
            vwIdentity.Add(this.txtUserName);

            // ===== Company =====
            this.lblCompany = new Label
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Top(lblAddress2),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblCompany",
                Text = "Company",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblCompany);

            this.txtCompany = new TextField
            {
                X = Pos.Left(lblCompany),
                Y = Pos.Bottom(lblCompany),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtCompany",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtCompany.HasFocusChanged += (s, e) => HandleFocusChange(txtCompany, e);
            vwIdentity.Add(this.txtCompany);

            // ===== Email =====
            this.lblEmail = new Label
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Top(lblAddress3),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblEmail",
                Text = "Email Address",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblEmail);

            this.txtEmailAddress = new TextField
            {
                X = Pos.Left(lblEmail),
                Y = Pos.Bottom(lblEmail),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtEmailAddress",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtEmailAddress.HasFocusChanged += (s, e) => HandleFocusChange(txtEmailAddress, e);
            vwIdentity.Add(this.txtEmailAddress);

            // ===== Phone =====
            this.lblPhone = new Label
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Top(lblCity),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                Data = "lblPhone",
                Text = "Phone Number",
                TextAlignment = Alignment.Start,
                CanFocus = false
            };
            vwIdentity.Add(this.lblPhone);

            this.txtPhoneNumber = new TextField
            {
                X = Pos.Left(lblPhone),
                Y = Pos.Bottom(lblPhone),
                Width = 45,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtPhoneNumber",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtPhoneNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtPhoneNumber, e);
            vwIdentity.Add(this.txtPhoneNumber);
        }
    }
}
