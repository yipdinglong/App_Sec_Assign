    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="App_Sec_Assignment.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
       <script type ="text/javascript">
           function validate() {
               var str = document.getElementById("psswd").value;
               if (str.length < 12) {
                   document.getElementById("psswd_check").innerHTML = "Enter at least 12 characters";
                   document.getElementById("psswd_check").style.color = "Red";
                   return ("too short");
               }
               else if (str.search(/[0-9]/) == -1) {
                   document.getElementById("psswd_check").innerHTML = "Enter at least 1 number";
                   document.getElementById("psswd_check").style.color = "Red";
                   return ("no_number");
               }
               else if (str.search(/[a-z]/) == -1) {
                   document.getElementById("psswd_check").innerHTML = "Enter at least 1 lower case";
                   document.getElementById("psswd_check").style.color = "Red";
                   return ("no_lowercase");
               }
               else if (str.search(/[A-Z]/) == -1) {
                   document.getElementById("psswd_check").innerHTML = "Enter at least 1 upper case";
                   document.getElementById("psswd_check").style.color = "Red";
                   return ("no_uppercase");
               }
               else if (str.search(/[^a-zA-Z0-9\-\/]/) == -1) {
                   document.getElementById("psswd_check").innerHTML = "Enter at least 1 special characters";
                   document.getElementById("psswd_check").style.color = "Red";
                   return ("no_specialcharacter");
               }
               document.getElementById("psswd_check").innerHTML = "Strong Password";
               document.getElementById("psswd_check").style.color = "Green"
           }
           function validateemail() {
               var getemail = document.getElementById("email").value;
               if (getemail.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {
                   document.getElementById("erroremail").innerHTML = "Enter a Valid Email";
                   document.getElementById("erroremail").style.color = "Red";
                   return ("no_such_email");
               }
               document.getElementById("erroremail").innerHTML = "Valid Email";
               document.getElementById("erroremail").style.color = "Green";
           }
           function validatecardinfo() {
               var getcardinfo = document.getElementById("CCI").value;
               if (getcardinfo.length == 16 && getcardinfo.search("^[0-9]$")) {
                   document.getElementById("errorcci").innerHTML = "Valid Card Number";
                   document.getElementById("errorcci").style.color = "Green";
                   return ("valid_card_number");
               }
               document.getElementById("errorcci").innerHTML = "Enter a valid card number consisting of 16 numbers";
               document.getElementById("errorcci").style.color = "Red";
           }
     /*      function validateimage() {
               var image = document.getElementById("imageupload").value;
               var Extension = image.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();
               if (Extension == "gif" || Extension == "jpeg" || Extension == "jpg" || Extension == "png") {
                   document.getElementById("errorimage").innerHTML = "valid Image";
                   document.getElementById("errorimage").style.color = "Green";
                   return ("valid_image");
               }
               document.getElementById("errorimage").innerHTML = "Please upload only images only";
               document.getElementById("errorimage").style.color = "Red";
           }*/
       </script>
</head>
<body>
    <div>
     <a class="" runat="server" href="Home.aspx" style="text-decoration:none">SITConnect</a>&nbsp;
     <a class="" runat="server" href="Register.aspx" style="text-decoration:none">Register</a>&nbsp;
         <a class="" runat="server" href="Login.aspx" style="text-decoration:none">Login</a><br />
&nbsp;
        </div>
        <form id="form1" runat="server">
             <div id =" container">
            Registration<p>
            &nbsp;</p>
            <div>
              

    <asp:Label ID="Label1" runat="server" Text="First Name:"></asp:Label>
    <asp:TextBox ID="fname" runat="server"></asp:TextBox>
        <br />
     </div>
    <div>
        <br />
    <asp:Label ID="Label2" runat="server" Text="Last Name:" ></asp:Label>
    <asp:TextBox ID="lname" runat="server"></asp:TextBox>
        <br />
        </span>
        <br />
        </div>
    <div>
    <asp:Label ID="Label3" runat="server" Text="Credit Card Info:"></asp:Label>
    <asp:TextBox ID="CCI" runat="server"  onkeyup="javascript:validatecardinfo()"></asp:TextBox>
         <asp:Label ID="errorcci" runat="server"></asp:Label>
        <br />
        <br />
        </div>
    <div>
    <asp:Label ID="Label4" runat="server" Text="Email Address:"></asp:Label>
    <asp:TextBox ID="email" runat="server" onkeyup="javascript:validateemail()"></asp:TextBox>
        <asp:Label ID="erroremail" runat="server"></asp:Label>
        <br />
        <br />
        </div>
    <div>
    <asp:Label ID="Label5" runat="server" Text="Password:"></asp:Label>
    <asp:TextBox ID="psswd" runat="server" TextMode="Password" onkeyup="javascript:validate()" type="password"></asp:TextBox>
        &nbsp;<asp:Label ID="psswd_check" runat="server"></asp:Label>
        <br />
        <br />
        </div>
    <div>
    <asp:Label ID="Label6" runat="server" Text="Date of Birth"></asp:Label>
    <asp:TextBox ID="dob" runat="server" type="date"></asp:TextBox>
        <br />
         <br />
        </div>
    <div>
    <asp:Label ID="Label7" runat="server" Text="Photo:"></asp:Label>
        <asp:FileUpload ID="imageupload" runat="server" onkeyup="javascript:validateimage()" />
          &nbsp;<asp:Label ID="errorimage" runat="server"></asp:Label>
   
        </div>
                   <br />
    <div>
        <asp:Button ID="Button1" runat="server" Text="Register" OnClick="Button1_Click" />
    </div>
    </form>
</body>
</html>
