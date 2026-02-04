using System;
using Retrowarden.Controls;
using Terminal.Gui;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Terminal.Gui.Drawing;

namespace Retrowarden.Views
{
    public partial class LoginDetailView
    {
        private View vwLogin;
        private Label lblUserName;
        private Label lblPassword;
        private TextField txtUserName;
        private Button btnCopyUserName;
        private TextField txtPassword;
        private Button btnViewPassword;
        private Button btnCopyPassword;
        private Button btnGeneratePassword;
        private Label lblTOTP;
        private TextField txtTOTP;
        private FrameView fraURIList;
        private UriScrollView scrURIList;
        private Button btnNewURI;

        private void InitializeComponent()
        {
            // Container view for login-specific controls
            this.vwLogin = new View()
            {
                X = 0,
                Y = 3,
                Width = Dim.Fill(),
                Height = 15,
                Visible = true,
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop,
                CanFocus = true
            };
            // Disable scrollbars on subview
            vwLogin.HorizontalScrollBar.Visible = false;
            vwLogin.HorizontalScrollBar.AutoShow = false;

            // ===== ROW 0: Username and Password labels =====
            this.lblUserName = new Label()
            {
                X = 1,
                Y = 0,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                CanFocus = false,
                Data = "lblUserName",
                Text = "Username",
                TextAlignment = Alignment.Start
            };

            this.lblPassword = new Label()
            {
                X = 40,
                Y = Pos.Top(lblUserName),
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                CanFocus = false,
                Data = "lblPassword",
                Text = "Password",
                TextAlignment = Alignment.Start
            };

            // ===== ROW 1: Username input with copy button, Password input with buttons =====
            this.txtUserName = new TextField()
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Bottom(lblUserName),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtUserName",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtUserName.HasFocusChanged += (s, e) => HandleFocusChange(txtUserName, e);

            this.btnCopyUserName = new Button()
            {
                X = Pos.Right(txtUserName) + 1,
                Y = Pos.Top(txtUserName),
                Text = "Cop_y",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnCopyUserName.Accepting += CopyUserNameButtonClicked;
            this.btnCopyUserName.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyUserName, e);

            this.txtPassword = new TextField()
            {
                X = Pos.Left(lblPassword),
                Y = Pos.Top(txtUserName),
                Width = 21,
                Height = 1,
                Visible = true,
                Secret = true,
                Data = "txtPassword",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtPassword.HasFocusChanged += (s, e) => HandleFocusChange(txtPassword, e);

            this.btnViewPassword = new Button()
            {
                X = Pos.Right(txtPassword) + 1,
                Y = Pos.Top(txtPassword),
                Text = "_Show",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnViewPassword.Accepting += ViewPasswordButtonClicked;
            this.btnViewPassword.HasFocusChanged += (s, e) => HandleFocusChange(btnViewPassword, e);

            this.btnCopyPassword = new Button()
            {
                X = Pos.Right(btnViewPassword) + 1,
                Y = Pos.Top(txtPassword),
                Text = "C_opy",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnCopyPassword.Accepting += CopyPasswordButtonClicked;
            this.btnCopyPassword.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyPassword, e);

            this.btnGeneratePassword = new Button()
            {
                X = Pos.Right(btnCopyPassword) + 1,
                Y = Pos.Top(txtPassword),
                Text = "_Generate",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnGeneratePassword.Accepting += GeneratePasswordButtonClicked;
            this.btnGeneratePassword.HasFocusChanged += (s, e) => HandleFocusChange(btnGeneratePassword, e);

            // ===== ROW 2: TOTP label =====
            this.lblTOTP = new Label()
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Bottom(txtUserName) + 1,
                Width = Dim.Auto(),
                Height = 1,
                Visible = true,
                CanFocus = false,
                Data = "lblTOTP",
                Text = "Authenticator Key (TOTP)",
                TextAlignment = Alignment.Start
            };

            // ===== ROW 3: TOTP input =====
            this.txtTOTP = new TextField()
            {
                X = Pos.Left(lblTOTP),
                Y = Pos.Bottom(lblTOTP),
                Width = 30,
                Height = 1,
                Visible = true,
                Secret = false,
                Data = "txtTOTP",
                Text = "",
                TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop
            };
            this.txtTOTP.HasFocusChanged += (s, e) => HandleFocusChange(txtTOTP, e);

            // ===== ROW 4-10: URI List frame =====
            this.fraURIList = new FrameView()
            {
                X = Pos.Left(lblUserName),
                Y = Pos.Bottom(txtTOTP) + 1,
                Width = 97,
                Height = 7,
                Visible = true,
                Enabled = true,
                CanFocus = true,
                Data = "fraURIList",
                BorderStyle = LineStyle.Single,
                TextAlignment = Alignment.Start,
                Title = "URI List",
                TabStop = TabBehavior.TabGroup
            };

            // ===== ROW 11: New URI button =====
            this.btnNewURI = new Button()
            {
                X = Pos.Left(fraURIList),
                Y = Pos.Bottom(fraURIList),
                Text = "New _URI",
                IsDefault = false,
                TabStop = TabBehavior.TabStop
            };
            this.btnNewURI.Accepting += NewUriButtonClicked;
            this.btnNewURI.HasFocusChanged += (s, e) => HandleFocusChange(btnNewURI, e);

            // Add controls in tab order
            this.vwLogin.Add(this.lblUserName);
            this.vwLogin.Add(this.lblPassword);
            this.vwLogin.Add(this.txtUserName);
            this.vwLogin.Add(this.btnCopyUserName);
            this.vwLogin.Add(this.txtPassword);
            this.vwLogin.Add(this.btnViewPassword);
            this.vwLogin.Add(this.btnCopyPassword);
            this.vwLogin.Add(this.btnGeneratePassword);
            this.vwLogin.Add(this.lblTOTP);
            this.vwLogin.Add(this.txtTOTP);
            this.vwLogin.Add(this.fraURIList);
            this.vwLogin.Add(this.btnNewURI);
        }
    }
}
