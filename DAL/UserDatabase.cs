using ClientSide.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientSide.DAL
{
    public class UserDatabase
    {
        SqlConnection con = null;
        public UserDatabase()
        {
            String constr = ConfigurationManager.ConnectionStrings["LocalConnection"].ToString();
            // con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\SOHEL\QC WEB SHEET PROJECT\Project\ClientSide\App_Data\Database1.mdf';Integrated Security=True");
            con = new SqlConnection(constr);
 
        }

        public UserLoginModel LoginUser(string Username,string Password,string SelectedCategory)
        {
            UserLoginModel user = null;
            try
            {
                con.Open();
                SqlDataReader reader = null;
                string str = "select * from Users where Username='{0}' and Password = '{1}'";
                string sql = string.Format(str, Username, Password);

                SqlCommand cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();
               
                if (reader.Read())
                {
                    user = new UserLoginModel()
                    {
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Developer = reader["Developer"].ToString(),
                        Code_Reviewer = reader["Code_Reviewer"].ToString(),
                    };
                    //if(SelectedCategory == "Developer")
                    //{

                    //}
                    //else if(SelectedCategory == "Code Reviewer")
                    //{

                    //}
                    //else
                    //{
                    //    return null;
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return user;
        }
    }
}