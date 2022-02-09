using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;

namespace App_Sec_Assignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        protected void change(object sender, EventArgs e)
        {
            string pwd = oldpsswd.Text.ToString().Trim();
            string userid = sameemail.Text.ToString().Trim();
            string newpwd = newpsswd.Text.ToString().Trim();
            bool newpsswdcheck = validateStrongPassword(newpwd);
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
            {
                string pwdWithSalt = pwd + dbSalt;
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                string userHash = Convert.ToBase64String(hashWithSalt);
                if (userHash.Equals(dbHash) && newpsswdcheck==true)
                {
                    //Generate random "salt"
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] newsaltByte = new byte[8];
                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(newsaltByte);
                    salt = Convert.ToBase64String(newsaltByte);
                    SHA512Managed newhashing = new SHA512Managed();
                    string newpwdWithSalt = newpwd + salt;
                    byte[] plainHash = newhashing.ComputeHash(Encoding.UTF8.GetBytes(newpwd));
                    byte[] newhashWithSalt = newhashing.ComputeHash(Encoding.UTF8.GetBytes(newpwdWithSalt));
                    finalHash = Convert.ToBase64String(newhashWithSalt);
                    changepassword(userid);
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    errormsg.Text = "Wrong email or old password Given.Please try again";
                    errormsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                errormsg.Text = "Wrong Username or Password Given.Please try again";
                errormsg.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void changepassword(string checkemail)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update ACCOUNT set PasswordHash=@passwordhash,PasswordSalt=@passwordsalt where Email=@checkemail "))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@checkemail", checkemail);
                            cmd.Parameters.AddWithValue("@passwordhash", finalHash);
                            cmd.Parameters.AddWithValue("@passwordsalt", salt);
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
        protected bool ValidateEmail4(String compareemail)
        {
            bool check = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email=@compareemail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@compareemail", compareemail);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (reader["Email"].ToString() == compareemail)
                        {
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
        private bool validateStrongPassword(string password)
        {
            if (password.Length == 0)
            {
                errornew.Text = "Password cannot be empty";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (password.Length < 12)
            {
                errornew.Text = "Password needs to have more than or at least 12 characters";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[a-z]"))
            {
                errornew.Text = "Password needs to have at least lowercase letter";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[A-Z]"))
            {
                errornew.Text = "Password needs to have at least a number";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (!Regex.IsMatch(password, "[0-9]"))
            {
                errornew.Text = "Password needs to have a uppercase letter";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                errornew.Text = "Password needs to have at least a special character";
                errornew.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else
            {
                errornew.Text = "STRONG PASSWORD";
                errornew.ForeColor = System.Drawing.Color.Green;
                return true;
            }
        }
        /*      protected void addingpasswordlog()
              {
                  try
                  {
                      using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                      {
                          using (SqlCommand cmd = new SqlCommand("INSERT INTO Password VALUES(@ )"))
                          {
                              using (SqlDataAdapter sda = new SqlDataAdapter())
                              {
                                  cmd.CommandType = CommandType.Text;
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
              }*/
    }
}