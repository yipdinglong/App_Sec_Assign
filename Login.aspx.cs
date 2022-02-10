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
using System.Threading;
namespace App_Sec_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        static int attempts = 0;
        static bool lockout = false;
        static DateTime date;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public class myobject
        {
            public string success { get; set; }

            public List<String> ErrorMessage { get; set; }
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

        protected void LogIn(object sender, EventArgs e)
        {
            string pwd = log_psswd.Text.ToString().Trim();
            string userid = log_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            try
            {
                if (Validatelockout(userid) == false)
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (userHash.Equals(dbHash))
                        {
                            Session["loggedIn"] = log_email.Text.Trim();
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Session["UserID"] = userid;
                            Response.Redirect("Home.aspx", false);
                        }
                        else
                        {
                            if (ValidateEmail3(userid) == false)
                            {
                                errorMsg.Text = "Wrong Email or Password. Please try again.";
                                errorMsg.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                attempts += 1;
                                errorMsg.Text = "Email or Password is not valid. Please try again." + "Your account has " + (3 - attempts) + " tries left before your account get lockout";
                                errorMsg.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                    else
                    {
                        errorMsg.Text = "Email or Password Incorrect. Please try again.";
                        errorMsg.ForeColor = System.Drawing.Color.Red;

                    }
                }
                else
                {
                    errorMsg.Text = "Your account has been locked Out due to 3 login failures";
                    errorMsg.ForeColor = System.Drawing.Color.Red;
                }
                if (attempts == 3)
                {
                    date = DateTime.Now;
                    errorMsg.Text = "Your account has been locked Out due to 3 login failures";
                    errorMsg.ForeColor = System.Drawing.Color.Red;
                    setlockout(userid);
                    attempts = 0;
                    lockout = true;
                }
                if (DateTime.Now>=date.AddSeconds(10) && lockout==true)
                {
                    setfalselockout(log_email.Text);
                    errorMsg.Text = "";
                    lockoutmsg.Text = "Account recovered";
                    lockoutmsg.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captcharesponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://wwww.google.com/recaptcha/api/siteverify?secret= &response=" + captcharesponse);
            try
            {
                using (WebResponse wresponse = req.GetResponse())
                {
                    using (StreamReader readstream = new StreamReader(wresponse.GetResponseStream()))
                    {
                        string jsonResponse = readstream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        myobject jsonobject = js.Deserialize<myobject>(jsonResponse);
                        result = Convert.ToBoolean(jsonobject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected bool ValidateEmail3(String compareemail)
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
        protected void setlockout(string lockemail)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update ACCOUNT set lockout='true' where Email=@lockemail "))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@lockemail", lockemail);
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
        protected bool Validatelockout(String compareemail)
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

                        if (reader["lockout"].ToString() == "true")
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
        protected void setfalselockout(string lockemail)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update ACCOUNT set lockout='false' where Email=@lockemail "))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@lockemail", lockemail);
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


    }
}