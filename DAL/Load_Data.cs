
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientSide.Models;
using System.Data.SqlClient;
using System;
using System.Web.Http;
using System.Configuration;

namespace ClientSide.DAL
{
    public class Load_Data
    {
        SqlConnection con = null;

        public Load_Data()
        {
            String constr = ConfigurationManager.ConnectionStrings["LocalConnection"].ToString();
            // con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\SOHEL\QC WEB SHEET PROJECT\Project\ClientSide\App_Data\Database1.mdf';Integrated Security=True");
            con = new SqlConnection(constr);
           
        }

      

        public Save SaveClientInfo(string Client_Code, string Project_Code, string Ext_Code, string Type, string TFS_Development_Id, string TFS_Source_Code)
        {
            Status s = new Status() { status = false };
            Save savedStatus = UserInfoAlreadyPresent(Client_Code, Project_Code, Ext_Code, Type, TFS_Development_Id, TFS_Source_Code);
            
            if (!savedStatus.status)
            {
                con.Open();
                try
                {
                    string str = "Insert into QC_Info(Client_Code,Project_Code,Ext_Code,[Type],TFS_Development_Id,TFS_Source_Code)values('{0}','{1}','{2}','{3}','{4}','{5}')";
                    string sql = string.Format(str, Client_Code, Project_Code, Ext_Code, Type, TFS_Development_Id, TFS_Source_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        savedStatus.status = true ;
                    }
                    else
                    {
                        savedStatus.status = false ;
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
            }
             else
            {
                savedStatus.status = false;
                return savedStatus;
            }
            return savedStatus;
        }

        public bool SubmitClientInfo(string Initial_QC,string QC_rework_1 ,string QC_rework_2,string Client_Code, string Project_Code, string Ext_Code)
        {

            SqlCommand cmd = null;
                try
                {
                con.Open();

                if(QC_rework_2 != null && QC_rework_2 != "")
                {
                    string str = "InsertSubmitValue_QC_Rework_2";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@2nd_QC_Rework", QC_rework_2));
                   

                }
                else if(QC_rework_1 != null && QC_rework_1 != "")
                {
                    string str = "InsertSubmitValue_QC_Rework_1";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@1st_QC_Rework", QC_rework_1));

                }
                else if (Initial_QC != null && Initial_QC != "")
                {
                    string str = "InsertSubmitValue_Initial_QC";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Initial_QC", Initial_QC));
                }
                cmd.Parameters.Add(new SqlParameter("@Client_Code", Client_Code));
                cmd.Parameters.Add(new SqlParameter("@Project_Code", Project_Code));
                cmd.Parameters.Add(new SqlParameter("@Ext_Code", Ext_Code));
               //SqlDataReader status = cmd.ExecuteReader();
                int status = cmd.ExecuteNonQuery();
                if (status > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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
        }

        public Save UserInfoAlreadyPresent(string Client_Code, string Project_Code, string Ext_Code, string Type, string TFS_Development_Id, string TFS_Source_Code)
        {  
            try
            {
                con.Open();
                Save s = null;
                string str = "select * from QC_Info where Client_Code='{0}' and Project_Code ='{1}' and Ext_Code ='{2}' ";
                string sql = string.Format(str, Client_Code, Project_Code, Ext_Code);
                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    s = new Save()
                    {
                        Initial_QC = reader["Initial_QC"].ToString(),
                        QC_Rework_1st = reader["1st_QC_Rework"].ToString(),
                        QC_Rework_2nd = reader["2nd_QC_Rework"].ToString(),
                        status = true,
                    };
                    return s;
                }
                else
                {
                    s = new Save();
                    s.status = false;
                    return s;
                }
            }
            catch (Exception)
            {
                throw;
            }finally
            {
               
                con.Close();
            }
            
        }

        public bool CheckIfAlreadyPresent(int Check_Id, string Client_Code, string Project_Code, string Ext_Code)
        {
            try
            {
                con.Open();
                string str = "select Check_id from QC_Userdata where Check_Id = {0} and Client_Code = '{1}' and Project_Code = '{2}' and Ext_Code = '{3}'";
                string sql = string.Format(str, Check_Id, Client_Code, Project_Code, Ext_Code);
                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(sql, con);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return true;
                }
                else
                {
                    return false;
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

        }

        public Status SaveClientData(int Check_Id , string Client_Code, string Project_Code, string Ext_Code ,string Developer,string QC_Resource,string Comment,string selectedUser , string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                con.Open();
                try
                {
                    string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Developer_Name,QC_Time) values({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', GETDATE())";
                    string sql = string.Format(str, Check_Id, Client_Code, Project_Code, Ext_Code, Developer, QC_Resource, Comment, username);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    //return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name = '{0}' , Developer = '{1}' ,Comment = '{2}',QC_Time= GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Developer, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            { 
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name = '{0}' , QC_Resource = '{1}' ,Comment = '{2}', QC_Time =GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username,QC_Resource, Comment,Check_Id,Client_Code,Project_Code,Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
           

            return s;
        }
    
        public Status SaveClientData1(int Check_Id, string Client_Code, string Project_Code, string Ext_Code, string Developer, string QC_Resource, string Comment, string selectedUser, string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                con.Open();
                try
                {
                    string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer_1,QC_Resource_1,Comment_1,QC_Developer_Name_1,QC_Time_1) values({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', GETDATE())";
                    string sql = string.Format(str, Check_Id, Client_Code, Project_Code, Ext_Code, Developer, QC_Resource, Comment, username);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    //return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name_1 = '{0}' , Developer_1 = '{1}' ,Comment_1 = '{2}',QC_Time_1= GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Developer, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name_1 = '{0}' , QC_Resource_1 = '{1}' ,Comment_1 = '{2}', QC_Time_1 =GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, QC_Resource, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }


            return s;
        }

        public Status SaveClientData2(int Check_Id, string Client_Code, string Project_Code, string Ext_Code, string Developer, string QC_Resource, string Comment, string selectedUser, string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                con.Open();
                try
                {
                    string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer_2,QC_Resource_2,Comment_2,QC_Developer_Name_2,QC_Time_2) values({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', GETDATE())";
                    string sql = string.Format(str, Check_Id, Client_Code, Project_Code, Ext_Code, Developer, QC_Resource, Comment, username);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    //return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name_2 = '{0}' , Developer_2 = '{1}' ,Comment_2 = '{2}', QC_Time_2 = GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Developer, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name_2 = '{0}' , QC_Resource_2 = '{1}' ,Comment_2= '{2}',QC_Time_2 = GETDATE() where Check_id = {3} and  Client_Code = '{4}' and Project_Code = '{5}' and Ext_Code = '{6}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, QC_Resource, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }


            return s;
        }

        public Status SaveComment(int Check_Id, string Client_Code, string Project_Code, string Ext_Code,string Comment, string selectedUser, string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                s = new Status() { status = true };
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name = '{0}'  ,Comment = '{1}',QC_Time= GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name = '{0}' ,Comment = '{1}', QC_Time =GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }


            return s;
        }

        public Status SaveComment1(int Check_Id, string Client_Code, string Project_Code, string Ext_Code, string Comment, string selectedUser, string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                s = new Status() { status = true };
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name_1 = '{0}'  ,Comment_1 = '{1}',QC_Time_1= GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    s = new Status() { status = false };
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name_1 = '{0}' ,Comment_1 = '{1}', QC_Time_1 =GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }


            return s;
        }

        public Status SaveComment2(int Check_Id, string Client_Code, string Project_Code, string Ext_Code,  string Comment, string selectedUser, string username)
        {
            Status s = null;
            bool AlreadyPresent = false;

            AlreadyPresent = CheckIfAlreadyPresent(Check_Id, Client_Code, Project_Code, Ext_Code);

            if (selectedUser == "Developer" && !AlreadyPresent)
            {
                s = new Status() { status = true };
            }
            else if (selectedUser == "Developer" && AlreadyPresent)
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Developer_Name_2 = '{0}' ,Comment_2 = '{1}', QC_Time_2 = GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    s = new Status() { status = false };
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }
            else
            {
                con.Open();
                try
                {
                    string update = "update  QC_Userdata set QC_Reviewer_Name_2 = '{0}' ,Comment_2= '{1}',QC_Time_2 = GETDATE() where Check_id = {2} and  Client_Code = '{3}' and Project_Code = '{4}' and Ext_Code = '{5}'";
                    //string str = "insert into QC_Userdata(Check_Id,Client_Code,Project_Code,Ext_Code,Developer,QC_Resource,Comment,QC_Reviewer_Name) values(1, 'Client_Code', 'Project_Code', 'Ext_Code', 'Developer', 'QC_Resource', 'Comment', 'QC_Reviewer_Name')";
                    string sql = string.Format(update, username, Comment, Check_Id, Client_Code, Project_Code, Ext_Code);

                    SqlCommand cmd = new SqlCommand(sql, con);
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        s = new Status() { status = true };
                    }
                    else
                    {
                        s = new Status() { status = false };
                    }
                }
                catch (Exception)
                {
                    return s;
                    throw;
                }
                finally
                {
                    con.Close();

                }
            }


            return s;
        }

    }
}
