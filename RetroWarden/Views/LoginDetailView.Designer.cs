using System;
using Retrowarden.Controls;
using Terminal.Gui;

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
            this.vwLogin = new View()
            {
                Width = 99, Height = 15, X = 0, Y = 3, Visible = true, TextAlignment = Alignment.Start,
                TabStop = TabBehavior.TabStop, CanFocus = true
            };
            
            this.btnNewURI = new Button()
            {
                Width = 12, Height = 1, X = 1, Y = 13, Visible = true, Data = "btnNewURI",
                Text = "New URI", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnNewURI.Accepting += NewUriButtonClicked;
            this.btnNewURI.HasFocusChanged += (s, e) => HandleFocusChange(btnNewURI, e);
            
            this.fraURIList = new FrameView()
            {
                Width = 97, Height = 7, X = 1, Y = 6, Visible = true, Enabled = true,
                CanFocus = true, Data = "fraURIList", BorderStyle = LineStyle.Single,
                TextAlignment = Alignment.Start, Title = "URI List", TabStop = TabBehavior.TabGroup
            };
            
            this.txtTOTP = new TextField()
            {
                Width = 30, Height = 1, X = 1, Y = 4, Visible = true, Secret = false,
                Data = "txtTOTP", Text = "", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtTOTP.HasFocusChanged += (s, e) => HandleFocusChange(txtTOTP, e);
            
            this.lblTOTP = new Label()
            {
                Width = 24, Height = 1, X = 1, Y = 3, Visible = true, CanFocus = false,
                Data = "lblTOTP", Text = "Authenticator Key (TOTP)", TextAlignment = Alignment.Start
            };
            
            this.btnGeneratePassword = new Button()
            {
                Width = 12, Height = 1, X = 80, Y = 1, Visible = true, Data = "btnGeneratePassword",
                Text = "Generate", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnGeneratePassword.Accepting += GeneratePasswordButtonClicked;
            this.btnGeneratePassword.HasFocusChanged += (s, e) => HandleFocusChange(btnGeneratePassword, e);
            
            this.btnCopyPassword = new Button()
            {
                Width = 8, Height = 1, X = 71, Y = 1, Visible = true, Data = "btnCopyPassword",
                Text = "Copy", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnCopyPassword.Accepting += CopyPasswordButtonClicked;
            this.btnCopyPassword.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyPassword, e);
            
            this.btnViewPassword = new Button()
            {
                Width = 8, Height = 1, X = 62, Y = 1, Visible = true, Data = "btnViewPassword",
                Text = "Show", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnViewPassword.Accepting += ViewPasswordButtonClicked;
            this.btnViewPassword.HasFocusChanged += (s, e) => HandleFocusChange(btnViewPassword, e);
            
            this.txtPassword = new TextField()
            {
                Width = 21, Height = 1, X = 40, Y = 1, Visible = true, Secret = true,
                Data = "txtPassword", Text = "", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtPassword.HasFocusChanged += (s, e) => HandleFocusChange(txtPassword, e);

            this.btnCopyUserName = new Button()
            {
                Width = 8, Height = 1, X = 23, Y = 1, Visible = true, Data = "btnCopyUserName",
                Text = "Copy", TextAlignment = Alignment.Center, IsDefault = false, TabStop = TabBehavior.TabStop
            };
            this.btnCopyUserName.Accepting += CopyUserNameButtonClicked;
            this.btnCopyUserName.HasFocusChanged += (s, e) => HandleFocusChange(btnCopyUserName, e);
            
            this.txtUserName = new TextField()
            {
                Width = 21, Height = 1, X = 1, Y = 1, Visible = true, Secret = false,
                Data = "txtUserName", Text = "", TextAlignment = Alignment.Start, TabStop = TabBehavior.TabStop
            };
            this.txtUserName.HasFocusChanged += (s, e) => HandleFocusChange(txtUserName, e);

            this.lblPassword = new Label()
            {
                Width = 8, Height = 1, X = 40, Y = 0, Visible = true, CanFocus = false,
                Data = "lblPassword", Text = "Password", TextAlignment = Alignment.Start,
            };

            this.lblUserName = new Label()
            {
                Width = 8, Height = 1, X = 1, Y = 0, Visible = true, CanFocus = false,
                Data = "lblUserName", Text = "Username", TextAlignment = Alignment.Start
            };
            
            // Add controls in tab order.
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
