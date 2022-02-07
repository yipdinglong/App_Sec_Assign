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
        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ACCOUNT(FirstName,LastName,CCINFO,Email,PasswordHash,PasswordSalt,DOB,Image) VALUES(@FirstName,@LastName, @CCINFO, @Email, @PasswordHash, @PasswordSalt, @DOB, @Image)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CCINFO",Convert.ToBase64String( encryptData(CCI.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Image", DBNull.Value);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            int strength = validateStrongPassword(psswd.Text);
            if (strength == 0)
            {
                psswd_check.Text = "Password needs to have more than or at least 12 characters";
                psswd_check.ForeColor = Color.Red;
            }
            if (strength == 1)
            {
                psswd_check.Text = "Password needs to have at least lowercase letter";
                psswd_check.ForeColor = Color.Red;
            }
            if(strength == 2)
            {
                psswd_check.Text = "Password needs to have at least a number";
                psswd_check.ForeColor = Color.Red;
            }
            if (strength == 3)
            {
                psswd_check.Text = "Password needs to have a uppercase letter";
                psswd_check.ForeColor = Color.Red;
            }
            if (strength == 4)
            {
                psswd_check.Text = "Password needs to have at least a special character";
                psswd_check.ForeColor = Color.Red;
            }
            else
            {

                psswd_check.Text = "STRONG PASSWORD";
                psswd_check.ForeColor = Color.Green;
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
                createAccount();
                Response.Redirect("Login.aspx", false);
            }
        }
        private int validateStrongPassword(string password)
        {
            if (password.Length < 12)
            {
                return 0;
            }
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return 1;
            }
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return 2;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return 3;
            }
            if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                return 4;
            }
            return 5;
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

    }
}