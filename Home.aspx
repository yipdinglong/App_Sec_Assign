<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App_Sec_Assignment.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
</head>
<body>
    <div>
         <a class="" runat="server" href="home.aspx" style="text-decoration:none">SITConnect</a>&nbsp;
     <a class="" runat="server" href="Register.aspx" style="text-decoration:none">Register</a>&nbsp;
         <a class="" runat="server" href="Login.aspx" style="text-decoration:none">Login</a><br />
&nbsp;
        </div>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="successMsg" runat="server" ></asp:Label>
        <br />
        <br />
        <div>
        <asp:Label ID="Label2" runat="server" Text="Email:"></asp:Label>
        <asp:Label ID="showemail" runat="server" ></asp:Label>
        </div>
        <br />
        <asp:Button ID="logoutbtn" runat="server" Text="LogOut" OnClick="logout" Visible="false"/>
    </form>
</body>
</html>
