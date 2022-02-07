<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App_Sec_Assignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
   <script src="https://www.google.com/recaptcha/api.js?render=6LdU3mIeAAAAAI7lWcHSws150CbsWz0qAGqN6N48"></script>
</head>
<body>
    <form id="form1" runat="server">
         <div>
            Login<br />
            <br />
            Email:
            <asp:TextBox ID="log_email" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:
            <asp:TextBox ID="log_psswd" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Login" Width="164px" OnClick="LogIn" />
            <br />
             <br />
             <asp:Label ID="errorMsg" runat="server" EnableViewState="false"></asp:Label>
             <br />
             <br />
     
        </div>
    </form>

</body>
</html>
