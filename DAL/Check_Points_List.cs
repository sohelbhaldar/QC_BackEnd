using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientSide.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ClientSide.DAL
{
    public class Check_Points_List
    {

        SqlConnection con = null;

        public Check_Points_List()
        {
            String constr = ConfigurationManager.ConnectionStrings["LocalConnection"].ToString();
            // con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\SOHEL\QC WEB SHEET PROJECT\Project\ClientSide\App_Data\Database1.mdf';Integrated Security=True");
            con = new SqlConnection(constr);

        }

        public CheckPoints Check(int Check_Id, String Check_Points, String Category, String Developer, String QC_Resource, String Comment)
        {
            con.Open();
            SqlDataReader reader = null;
            string str = "select c.Check_Id,c.Category,c.Check_Points, i.Developer, i.QC_Resource, i.Comment from QC_Check_Points c, QC_Userdata i WHERE Category = '{0}'";

            string sql = string.Format(str, Category);

            SqlCommand cmd = new SqlCommand(sql, con);
            reader = cmd.ExecuteReader();
            CheckPoints pts = null;

            if (reader.Read())
            {
                pts = new CheckPoints()
                {
                    Check_Id = Convert.ToInt32(reader["Check_Id"]),
                    Category = reader["Category"].ToString(),
                    Check_Points = reader["Check_Points"].ToString(),
                    Developer = reader["Developer"].ToString(),
                    QC_Resource = reader["QC_Resource"].ToString(),
                    Comment = reader["Comment"].ToString(),

                };
            }
            con.Close();
            return pts;
        }

        public List<KeyValuePair<string, CheckPoints>> UserCheckPointsList(int Check_Id, String Check_Points, String Category, String Developer, String QC_Resource, String Comment)
        {
           // string category = "";
            List<CheckPoints> SelectedPointsList = new List<CheckPoints>();
            var categorisedCheckPoints = new List<KeyValuePair<string, CheckPoints>>();
              con.Open();
            try
            {
                string str = "select c.Check_Id,c.Category,c.Check_Points, i.Developer, i.QC_Resource, i.Comment from QC_Check_Points c, QC_Userdata i WHERE Category = '{0}'";

                string sql = string.Format(str, Category);

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Category =  reader["Category"].ToString();
                    CheckPoints pts = new CheckPoints()
                    {
                        Check_Id = Convert.ToInt32(reader["Check_Id"]),
                        Category = reader["Category"].ToString(),
                        Check_Points = reader["Check_Points"].ToString(),
                        Developer = reader["Developer"].ToString(),
                        QC_Resource = reader["QC_Resource"].ToString(),
                        Comment = reader["Comment"].ToString(),

                    };

                    categorisedCheckPoints.Add(new KeyValuePair<string, CheckPoints>(Category,pts));
                    SelectedPointsList.Add(pts);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();

            }
            return categorisedCheckPoints;

        }

        public List<KeyValuePair<string, CheckPoints>> CheckPointsList()
        {
            // string category = "";
            List<CheckPoints> SelectedPointsList = new List<CheckPoints>();
            string Category = "";
            var categorisedCheckPoints = new List<KeyValuePair<string, CheckPoints>>();
            con.Open();
            try
            {
                string str = "select Check_Id, Category, Check_Points from QC_Check_Points";
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Category = reader["Category"].ToString();
                    CheckPoints pts = new CheckPoints()
                    {
                        Check_Id = Convert.ToInt32(reader["Check_Id"]),
                        Category = reader["Category"].ToString(),
                        Check_Points = reader["Check_Points"].ToString(),                       

                    };
                    categorisedCheckPoints.Add(new KeyValuePair<string, CheckPoints>(Category, pts));
                    SelectedPointsList.Add(pts);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();

            }
            return categorisedCheckPoints;

        }

        public List<CheckPoints> LoadSavedUserDATA(string Client_Code, string Project_Code, string Ext_Code)
        {
            List<CheckPoints> SaveData = new List<CheckPoints>();
            SqlDataReader reader = null;
            CheckPoints pts = null;
            try
            {
                con.Open();
               
                string str = "select cp.Check_Id,ud.Developer,ud.QC_Resource,ud.Comment,ud.Developer_1,ud.QC_Resource_1,ud.Comment_1,ud.Developer_2,ud.QC_Resource_2,ud.Comment_2 from QC_Check_Points cp left outer join QC_Userdata ud on cp.Check_Id = ud.Check_Id left outer join QC_Info inf on ud.Client_Code = inf.Client_Code and ud.Ext_Code = inf.Ext_Code and ud.Project_Code = inf.Project_Code where inf.Client_Code = '{0}' and inf.Project_Code = '{1}' and inf.Ext_Code = '{2}' order by  cp.Check_Id";
                string sql = string.Format(str, Client_Code, Project_Code, Ext_Code);
                SqlCommand cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();
               
                while (reader.Read())
                {
                    pts = new CheckPoints()
                    {
                        Check_Id = Convert.ToInt32(reader["Check_Id"]),
                        Developer = reader["Developer"].ToString(),
                        QC_Resource = reader["QC_Resource"].ToString(),
                        Comment = reader["Comment"].ToString(),

                        Developer_1 = reader["Developer_1"].ToString(),
                        QC_Resource_1 = reader["QC_Resource_1"].ToString(),
                        Comment_1 = reader["Comment_1"].ToString(),

                        Developer_2 = reader["Developer_2"].ToString(),
                        QC_Resource_2 = reader["QC_Resource_2"].ToString(),
                        Comment_2 = reader["Comment_2"].ToString(),


                    };
                    SaveData.Add(pts);
                }
                return SaveData;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
           
        }

        #region CodeReview
        public List<CodeReviewCheckPoints> GetCodeReviewCheckPoints()
        {
            List<CodeReviewCheckPoints> SelectedPointsList = new List<CodeReviewCheckPoints>();
            con.Open();
            try
            {
                string str = "select Check_Id, Applied_For, Check_Points from CodeReview_Check_Points";
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CodeReviewCheckPoints pts = new CodeReviewCheckPoints()
                    {
                        Check_Id = Convert.ToInt32(reader["Check_Id"]),
                        Applied_For = reader["Applied_For"].ToString(),
                        Check_Points = reader["Check_Points"].ToString(),

                    };
                    SelectedPointsList.Add(pts);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();

            }
            return SelectedPointsList;

        }

        #endregion CodeReview
    }
}
