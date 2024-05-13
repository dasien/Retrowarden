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
            this.vwLogin = new View();
            this.btnNewURI = new Button();
            this.fraURIList = new FrameView();
            this.txtTOTP = new TextField();
            this.lblTOTP = new Label();
            this.btnGeneratePassword = new Button();
            this.btnCopyPassword = new Button();
            this.btnViewPassword = new Button();
            this.txtPassword = new TextField();
            this.btnCopyUserName = new Button();
            this.txtUserName = new TextField();
            this.lblPassword = new Label();
            this.lblUserName = new Label();
            
            // View details.
            vwLogin.Width = 99;
            vwLogin.Height = 15;
            vwLogin.X = 0;
            vwLogin.Y = 3;
            vwLogin.Visible = true;
            vwLogin.TextAlignment = TextAlignment.Left;
            
            this.lblUserName.Width = 4;
            this.lblUserName.Height = 1;
            this.lblUserName.X = 1;
            this.lblUserName.Y = 0;
            this.lblUserName.Visible = true;
            this.lblUserName.Data = "lblUserName";
            this.lblUserName.Text = "Username";
            this.lblUserName.TextAlignment = TextAlignment.Left;
            this.vwLogin.Add(this.lblUserName);
            
            this.lblPassword.Width = 4;
            this.lblPassword.Height = 1;
            this.lblPassword.X = 40;
            this.lblPassword.Y = 0;
            this.lblPassword.Visible = true;
            this.lblPassword.Data = "lblPassword";
            this.lblPassword.Text = "Password";
            this.lblPassword.TextAlignment = TextAlignment.Left;
            this.vwLogin.Add(this.lblPassword);
            
            this.txtUserName.Width = 21;
            this.txtUserName.Height = 1;
            this.txtUserName.X = 1;
            this.txtUserName.Y = 1;
            this.txtUserName.Visible = true;
            this.txtUserName.Secret = false;
            this.txtUserName.Data = "txtUserName";
            this.txtUserName.Text = "";
            this.txtUserName.TextAlignment = TextAlignment.Left;
            txtUserName.Enter += (_) => HandleControlEnter(txtUserName);
            this.vwLogin.Add(this.txtUserName);
            
            this.btnCopyUserName.Width = 8;
            this.btnCopyUserName.Height = 1;
            this.btnCopyUserName.X = 23;
            this.btnCopyUserName.Y = 1;
            this.btnCopyUserName.Visible = true;
            this.btnCopyUserName.Data = "btnCopyUserName";
            this.btnCopyUserName.Text = "Copy";
            this.btnCopyUserName.TextAlignment = TextAlignment.Centered;
            this.btnCopyUserName.IsDefault = false;
            this.btnCopyUserName.Clicked += CopyUserNameButtonClicked;
            this.vwLogin.Add(this.btnCopyUserName);
            
            this.txtPassword.Width = 21;
            this.txtPassword.Height = 1;
            this.txtPassword.X = 40;
            this.txtPassword.Y = 1;
            this.txtPassword.Visible = true;
            this.txtPassword.Secret = true;
            this.txtPassword.Data = "txtPassword";
            this.txtPassword.Text = "";
            this.txtPassword.TextAlignment = TextAlignment.Left;
            txtPassword.Enter += (_) => HandleControlEnter(txtPassword);
            this.vwLogin.Add(this.txtPassword);
            
            this.btnViewPassword.Width = 8;
            this.btnViewPassword.Height = 1;
            this.btnViewPassword.X = 62;
            this.btnViewPassword.Y = 1;
            this.btnViewPassword.Visible = true;
            this.btnViewPassword.Data = "btnViewPassword";
            this.btnViewPassword.Text = "View";
            this.btnViewPassword.TextAlignment = TextAlignment.Centered;
            this.btnViewPassword.IsDefault = false;
            this.btnViewPassword.Clicked += ViewPasswordButtonClicked;
            this.vwLogin.Add(this.btnViewPassword);
            
            this.btnCopyPassword.Width = 8;
            this.btnCopyPassword.Height = 1;
            this.btnCopyPassword.X = 71;
            this.btnCopyPassword.Y = 1;
            this.btnCopyPassword.Visible = true;
            this.btnCopyPassword.Data = "btnCopyPassword";
            this.btnCopyPassword.Text = "Copy";
            this.btnCopyPassword.TextAlignment = TextAlignment.Centered;
            this.btnCopyPassword.IsDefault = false;
            this.btnCopyPassword.Clicked += CopyPasswordButtonClicked;
            this.vwLogin.Add(this.btnCopyPassword);
            
            this.btnGeneratePassword.Width = 12;
            this.btnGeneratePassword.Height = 1;
            this.btnGeneratePassword.X = 80;
            this.btnGeneratePassword.Y = 1;
            this.btnGeneratePassword.Visible = true;
            this.btnGeneratePassword.Data = "btnGeneratePassword";
            this.btnGeneratePassword.Text = "Generate";
            this.btnGeneratePassword.TextAlignment = TextAlignment.Centered;
            this.btnGeneratePassword.IsDefault = false;
            this.btnGeneratePassword.Clicked += GeneratePasswordButtonClicked;
            this.vwLogin.Add(this.btnGeneratePassword);
            
            this.lblTOTP.Width = 4;
            this.lblTOTP.Height = 1;
            this.lblTOTP.X = 1;
            this.lblTOTP.Y = 3;
            this.lblTOTP.Visible = true;
            this.lblTOTP.Data = "lblTOTP";
            this.lblTOTP.Text = "Authenticator Key (TOTP)";
            this.lblTOTP.TextAlignment = TextAlignment.Left;
            this.vwLogin.Add(this.lblTOTP);
            
            this.txtTOTP.Width = 30;
            this.txtTOTP.Height = 1;
            this.txtTOTP.X = 1;
            this.txtTOTP.Y = 4;
            this.txtTOTP.Visible = true;
            this.txtTOTP.Secret = false;
            this.txtTOTP.Data = "txtTOTP";
            this.txtTOTP.Text = "";
            this.txtTOTP.TextAlignment = TextAlignment.Left;
            txtTOTP.Enter += (_) => HandleControlEnter(txtTOTP);
            this.vwLogin.Add(this.txtTOTP);
            
            this.fraURIList.Width = 97;
            this.fraURIList.Height = 7;
            this.fraURIList.X = 1;
            this.fraURIList.Y = 6;
            this.fraURIList.Visible = true;
            this.fraURIList.Enabled = true;
            this.fraURIList.CanFocus = true;
            this.fraURIList.Data = "fraURIList";
            this.fraURIList.Border.BorderStyle = BorderStyle.Single;
            this.fraURIList.Border.Effect3D = false;
            this.fraURIList.Border.Effect3DBrush = null;
            this.fraURIList.Border.DrawMarginFrame = true;
            this.fraURIList.TextAlignment = TextAlignment.Left;
            this.fraURIList.Title = "URI List";
            this.vwLogin.Add(this.fraURIList);
            
            this.btnNewURI.Width = 8;
            this.btnNewURI.Height = 1;
            this.btnNewURI.X = 1;
            this.btnNewURI.Y = 13;
            this.btnNewURI.Visible = true;
            this.btnNewURI.Data = "btnNewURI";
            this.btnNewURI.Text = "New URI";
            this.btnNewURI.TextAlignment = TextAlignment.Centered;
            this.btnNewURI.IsDefault = false;
            this.btnNewURI.Clicked += NewUriButtonClicked;
            this.vwLogin.Add(this.btnNewURI);
        }
    }
}
