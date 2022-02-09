<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App_Sec_Assignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
   <script src="https://www.google.com/recaptcha/api.js?render=6LdU3mIeAAAAAI7lWcHSws150CbsWz0qAGqN6N48"></script>
</head>
<body>
    <div>
         <a class="" runat="server" href="Home.aspx" style="text-decoration:none">SITConnect</a>&nbsp;
     <a class="" runat="server" href="Register.aspx" style="text-decoration:none">Register</a>&nbsp;
         <a class="" runat="server" href="Login.aspx" style="text-decoration:none">Login</a><br />
&nbsp;
        </div>
    <form id="form1" runat="server">
         <div>
            Login<br />
            <br />
            Email:
            <asp:TextBox ID="log_email" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:
            <asp:TextBox ID="log_psswd" runat="server" type="password"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Login" Width="164px" OnClick="LogIn" />
            <br />
             <br />
             <asp:Label ID="errorMsg" runat="server" EnableViewState="false"></asp:Label>
             <br />
             <br />
              <asp:Label ID="lockoutmsg" runat="server" EnableViewState="false"></asp:Label>
             <br />
             <br />
             <br />
             <a class="" runat="server" href="ChangePassword.aspx" style="text-decoration:none">Change Your Password</a><br />
             <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        </div>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    </form>
</body>
</html>
