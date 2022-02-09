<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="App_Sec_Assignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Changpassword</title>
      <script type ="text/javascript">
          function validate() {
              var str = document.getElementById("newpsswd").value;
              if (str.length < 12) {
                  document.getElementById("errornew").innerHTML = "Enter at least 12 characters";
                  document.getElementById("errornew").style.color = "Red";
                  return ("too short");
              }
              else if (str.search(/[0-9]/) == -1) {
                  document.getElementById("errornew").innerHTML = "Enter at least 1 number";
                  document.getElementById("errornew").style.color = "Red";
                  return ("no_number");
              }
              else if (str.search(/[a-z]/) == -1) {
                  document.getElementById("errornew").innerHTML = "Enter at least 1 lower case";
                  document.getElementById("errornew").style.color = "Red";
                  return ("no_lowercase");
              }
              else if (str.search(/[A-Z]/) == -1) {
                  document.getElementById("errornew").innerHTML = "Enter at least 1 upper case";
                  document.getElementById("errornew").style.color = "Red";
                  return ("no_uppercase");
              }
              else if (str.search(/[^a-zA-Z0-9\-\/]/) == -1) {
                  document.getElementById("errornew").innerHTML = "Enter at least 1 special characters";
                  document.getElementById("errornew").style.color = "Red";
                  return ("no_specialcharacter");
              }
              document.getElementById("errornew").innerHTML = "Strong Password";
              document.getElementById("errornew").style.color = "Green"
          }
          </script>
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
        <asp:Label ID="Textemail" runat="server" Text="Email:"></asp:Label>
         <asp:TextBox ID="sameemail" runat="server"></asp:TextBox>
        <asp:Label ID="erroremail" runat="server" ></asp:Label>
        <br />
        <br />
        </div>
         <div>
        <asp:Label ID="Textoldpsswd" runat="server" Text="Old Password:"></asp:Label>
               <asp:TextBox ID="oldpsswd" runat="server" type="password"></asp:TextBox>
        <asp:Label ID="errorold" runat="server" ></asp:Label>
             <br />
             <br />
        </div>
          <div>
        <asp:Label ID="Textnewpsswd" runat="server" Text="New Password:" ></asp:Label>
               <asp:TextBox ID="newpsswd" runat="server" type="password" onkeyup="javascript:validate()"></asp:TextBox>
        <asp:Label ID="errornew" runat="server" ></asp:Label>
              <br />
              <br />
        </div>
        <asp:Button ID="Button1" runat="server" Text="Change Password" Width="164px" OnClick="change" />

        <br />
        <br />

        <asp:Label ID="errormsg" runat="server" EnableViewState="false"></asp:Label>
    </form>
</body>
</html>
