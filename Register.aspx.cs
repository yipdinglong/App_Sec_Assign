using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace App_Sec_Assignment
{
    public partial class Register : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            bool checkpsswd=validateStrongPassword(psswd.Text);
            bool validatecci=validatecreditcard(CCI.Text);
            //string pwd = get value from your Textbox
            string pwd = psswd.Text.ToString().Trim(); ;
            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            int check= validateemail2(email.Text);
            if (ValidateEmail(email.Text.ToString().Trim()) == false && check==2 && validatecci==true && checkpsswd==true)
            {
                createAccount();
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ACCOUNT VALUES(@FirstName,@LastName, @CCINFO, @Email, @PasswordHash, @PasswordSalt, @DOB, @Image,@IV,@Key,@lockout )"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName",HttpUtility.HtmlEncode(fname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@LastName", HttpUtility.HtmlEncode(lname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CCINFO", Convert.ToBase64String(encryptData(CCI.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email",HttpUtility.HtmlEncode((email.Text.Trim())));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", HttpUtility.HtmlEncode(dob.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Image", imageupload.FileBytes);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@lockout","false");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        private bool validateStrongPassword(string password)
        {
            if (password.Length == 0) {
                psswd_check.Text = "Password cannot be empty";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else if (password.Length < 12)
            {
                psswd_check.Text = "Password needs to have more than or at least 12 characters";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[a-z]"))
            {
                psswd_check.Text = "Password needs to have at least lowercase letter";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[A-Z]"))
            {
                psswd_check.Text = "Password needs to have at least a number";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[0-9]"))
            {
                psswd_check.Text = "Password needs to have a uppercase letter";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                psswd_check.Text = "Password needs to have at least a special character";
                psswd_check.ForeColor = Color.Red;
                return false;
            }
            else
            {
                psswd_check.Text = "STRONG PASSWORD";
                psswd_check.ForeColor = Color.Green;
                return true;
            }
        }
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
        protected bool ValidateEmail(String checkemail)
        {
            bool check = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email=@checkemail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@checkemail", checkemail);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
 
                        if (reader["Email"].ToString() == checkemail)
                        {
                            erroremail.Text = "Email has already been used!";
                            erroremail.ForeColor = Color.Red;
                            check = true;
                        }
                    }
                }
                return check;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected int validateemail2(string email)
        {
            if (email.Length==0)
            {
                erroremail.Text = "Missing Email";
                erroremail.ForeColor = Color.Red;
                return 0;
            }
            else
            {
                erroremail.Text = "Valid Email";
                erroremail.ForeColor = Color.Green;
                return 2;
            }
        }
        protected bool validatecreditcard(string cardnumber)
        {
            if (cardnumber.Length == 16 && !Regex.IsMatch(cardnumber, "^[0-9]$"))
            {
                errorcci.Text = "Valid Card Number";
                errorcci.ForeColor = Color.Green;
                return true;
            }
            else {
                errorcci.Text = "Invalid Card Number";
                errorcci.ForeColor = Color.Red;
                return false;
            }
        }
    }
}