/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * IdentityDetailView.Designer.cs
 *
 * Auto-generated Terminal.Gui designer code for the main view layout.
 * Do not modify this file manually.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/using System;
using System.Text;
using Terminal.Gui;
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
            vwIdentity = new View 
            {
                Width = 99, Height = 25, X = 0, Y = 3, Visible = true, TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop, CanFocus = true
            };

            this.lblTitle = new Label
            {
                Width = 6, Height = 1, X = 1, Y = 0, Visible = true, Data = "lblTitle", Text = "Title", 
                TextAlignment = Alignment.Start, CanFocus = false
            }; 
            vwIdentity.Add(this.lblTitle);

            this.cboTitle = new ComboBox
            {
                Width = 23, Height = 5, X = 1, Y = 1, Visible = true, Data = "cboTitle", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.cboTitle.HasFocusChanged += (s, e) => HandleFocusChange(cboTitle, e);
            vwIdentity.Add(this.cboTitle);

            this.lblFirstName = new Label
            {
                Width = 12, Height = 1, X = 1, Y = 3, Visible = true, Data = "lblFirstName", Text = "First Name", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblFirstName);

            this.lblMiddleName = new Label
            {
                Width = 12, Height = 1, X = 33, Y = 3, Visible = true, Data = "lblMiddleName", Text = "Middle Name", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblMiddleName);

            this.lblLastName = new Label
            {
                Width = 12, Height = 1, X = 65, Y = 3, Visible = true, Data = "lblLastName", Text = "Last Name", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblLastName);
            
            this.txtFirstName = new TextField
            {
                Width = 30, Height = 1, X = 1, Y = 4, Visible = true, Secret = false, Data = "txtFirstName", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtFirstName.HasFocusChanged += (s, e) => HandleFocusChange(txtFirstName, e);
            vwIdentity.Add(this.txtFirstName);

            this.txtMiddleName = new TextField
            {
                Width = 30, Height = 1, X = 33, Y = 4, Visible = true, Secret = false, Data = "txtMiddleName", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtMiddleName.HasFocusChanged += (s, e) => HandleFocusChange(txtMiddleName, e);
            vwIdentity.Add(this.txtMiddleName);

            this.txtLastName = new TextField
            {
                Width = 30, Height = 1, X = 65, Y = 4, Visible = true, Secret = false, Data = "txtLastName", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtLastName.HasFocusChanged += (s, e) => HandleFocusChange(txtLastName, e);
            vwIdentity.Add(this.txtLastName);
            
            this.lblSSN = new Label
            {
                Width = 22, Height = 1, X = 1, Y = 6, Visible = true, Data = "lblSSN", Text = "Social Security Number",
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblSSN);

            this.lblPassportNumber = new Label
            {
                Width = 30, Height = 1, X = 33, Y = 6, Visible = true, Data = "lblPassportNumber", Text = "Passport Number",
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblPassportNumber);

            this.lblLicenseNumber = new Label
            {
                Width = 20, Height = 1, X = 65, Y = 6, Visible = true, Data = "lblLicenseNumber", Text = "License Number",
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblLicenseNumber);
            
            this.txtSSN = new TextField
            {
                Width = 30, Height = 1, X = 1, Y = 7, Visible = true, Secret = false, Data = "txtSSN", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtSSN.HasFocusChanged += (s, e) => HandleFocusChange(txtSSN, e);
            vwIdentity.Add(this.txtSSN);

            this.txtPassportNumber = new TextField
            {
                Width = 30, Height = 1, X = 33, Y = 7, Visible = true, Secret = false, Data = "txtPassportNumber", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtPassportNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtPassportNumber, e);
            vwIdentity.Add(this.txtPassportNumber);

            this.txtLicenseNumber = new TextField
            {
                Width = 30, Height = 1, X = 65, Y = 7, Visible = true, Secret = false, Data = "txtLicenseNumber", Text = "",
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtLicenseNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtLicenseNumber, e);
            vwIdentity.Add(this.txtLicenseNumber);
            
            this.lineView2 = new LineView
            {
                Width = 94, Height = 3, X = 1, Y = 9, Visible = true, Data = "lineView2", TextAlignment = Alignment.Start,
                LineRune = new Rune('─'), Orientation = Orientation.Horizontal, CanFocus = false
            };
            vwIdentity.Add(this.lineView2);

            this.lineView = new LineView
            {
                Width = 1, Height = 15, X = 47, Y = 9, Visible = true, Data = "lineView", TextAlignment = Alignment.Start,
                LineRune = new Rune('│'), Orientation = Orientation.Vertical, CanFocus = false
            };
            vwIdentity.Add(this.lineView);

            this.lblAddress1 = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 10, Visible = true, Data = "lblAddress1", Text = "Address 1", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblAddress1);

            this.txtAddress1 = new TextField
            {
                Width = 45, Height = 1, X = 1, Y = 11, Visible = true, Secret = false, Data = "txtAddress1", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtAddress1.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress1, e);
            vwIdentity.Add(this.txtAddress1);

            this.lblAddress2 = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 13, Visible = true, Data = "lblAddress2", Text = "Address 2", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblAddress2);

            this.txtAddress2 = new TextField
            {
                Width = 45, Height = 2, X = 1, Y = 14, Visible = true, Secret = false, Data = "txtAddress2", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtAddress2.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress2, e);
            vwIdentity.Add(this.txtAddress2);

            this.lblAddress3 = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 16, Visible = true, Data = "lblAddress3", Text = "Address 3", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblAddress3);

            this.txtAddress3 = new TextField
            {
                Width = 45, Height = 1, X = 1, Y = 17, Visible = true, Secret = false, Data = "txtAddress3", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtAddress3.HasFocusChanged += (s, e) => HandleFocusChange(txtAddress3, e);
            vwIdentity.Add(this.txtAddress3);

            this.lblCity = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 19, Visible = true, Data = "lblCity", Text = "City / Town", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblCity);

            this.lblState = new Label
            {
                Width = 16, Height = 1, X = 25, Y = 19, Visible = true, Data = "lblState", Text = "State / Province", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblState);

            this.txtCity = new TextField
            {
                Width = 21, Height = 1, X = 1, Y = 20, Visible = true, Secret = false, Data = "txtCity", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtCity.HasFocusChanged += (s, e) => HandleFocusChange(txtCity, e);
            vwIdentity.Add(this.txtCity);

            this.txtState = new TextField
            {
                Width = 21, Height = 1, X = 25, Y = 20, Visible = true, Secret = false, Data = "txtState", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtState.HasFocusChanged += (s, e) => HandleFocusChange(txtState, e);
            vwIdentity.Add(this.txtState);

            this.lblZipCode = new Label
            {
                Width = 16, Height = 1, X = 1, Y = 22, Visible = true, Data = "lblZipCode", Text = "Zip / Postal Code", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblZipCode);

            this.lblCountry = new Label
            {
                Width = 12, Height = 1, X = 25, Y = 22, Visible = true, Data = "lblCountry", Text = "Country", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblCountry);

            this.txtZipCode = new TextField
            {
                Width = 21, Height = 1, X = 1, Y = 23, Visible = true, Secret = false, Data = "txtZipCode", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtZipCode.HasFocusChanged += (s, e) => HandleFocusChange(txtZipCode, e);
            vwIdentity.Add(this.txtZipCode);

            this.txtCountry = new TextField
            {
                Width = 21, Height = 1, X = 25, Y = 23, Visible = true, Secret = false, Data = "txtCountry", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtCountry.HasFocusChanged += (s, e) => HandleFocusChange(txtCountry, e);
            vwIdentity.Add(this.txtCountry);

            this.lblUserName = new Label
            {
                Width = 16, Height = 1, X = 49, Y = 10, Visible = true, Data = "lblUserName", Text = "User Name", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblUserName);

            this.txtUserName = new TextField
            {
                Width = 45, Height = 1, X = 49, Y = 11, Visible = true, Secret = false, Data = "txtUserName", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtUserName.HasFocusChanged += (s, e) => HandleFocusChange(txtUserName, e);
            vwIdentity.Add(this.txtUserName);

            this.lblCompany = new Label
            {
                Width =12, Height = 1, X = 49, Y = 13, Visible = true, Data = "lblCompany", Text = "Company", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblCompany);

            this.txtCompany = new TextField
            {
                Width = 45, Height = 1, X = 49, Y = 14, Visible = true, Secret = false, Data = "txtCompany", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtCompany.HasFocusChanged += (s, e) => HandleFocusChange(txtCompany, e);
            vwIdentity.Add(this.txtCompany);

            this.lblEmail = new Label
            {
                Width = 16, Height = 1, X = 49, Y = 16, Visible = true, Data = "lblEmail", Text = "Email Address", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblEmail);

            this.txtEmailAddress = new TextField
            {
                Width = 45, Height = 1, X = 49, Y = 17, Visible = true, Secret = false, Data = "txtEmailAddress", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtEmailAddress.HasFocusChanged += (s, e) => HandleFocusChange(txtEmailAddress, e);
            vwIdentity.Add(this.txtEmailAddress);

            this.lblPhone = new Label
            {
                Width = 16, Height = 1, X = 49, Y = 19, Visible = true, Data = "lblPhone", Text = "Phone Number", 
                TextAlignment = Alignment.Start, CanFocus = false
            };
            vwIdentity.Add(this.lblPhone);

            this.txtPhoneNumber = new TextField
            {
                Width = 45, Height = 1, X = 49, Y = 20, Visible = true, Secret = false, Data = "txtPhoneNumber", Text = "", 
                TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtPhoneNumber.HasFocusChanged += (s, e) => HandleFocusChange(txtPhoneNumber, e);
            vwIdentity.Add(this.txtPhoneNumber);
        }
    }   
}
