<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App_Sec_Assignment.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="successMsg" runat="server" ></asp:Label>
        <br />
        <br />
        <asp:Button ID="logoutbtn" runat="server" Text="LogOut" OnClick="logout" Visible="false"/>
    </form>
</body>
</html>
