using ClientSide.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ClientSide.DAL
{ 
        public class ClientCodeData
        {
        SqlConnection con = null;
        public ClientCodeData()
        {
            
              String constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\SOHEL\QC WEB SHEET PROJECT\Project\ClientSide\App_Data\Database1.mdf';Integrated Security=True");
              con = new SqlConnection(constr);
        }

        public List<ClientCodeDetails> GetClientCodes(string user)
        {
            List<ClientCodeDetails> SelectedClientCodes = new List<ClientCodeDetails>();
            ClientCodeDetails selectedClientCode = null;
            try
            {
                con.Open();
                string str = "select DISTINCT CLIENT_CODE from CLIENT order by CLIENT_CODE";
                // string sql = string.Format(str, RouteID, c.DepartureDate);
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    selectedClientCode = new ClientCodeDetails()
                    {
                        user = user,
                        status = true,
                        Client_Code = reader["CLIENT_CODE"].ToString(),
                    };
                    SelectedClientCodes.Add(selectedClientCode);
                }
            }
            catch (Exception )
            {
                selectedClientCode = new ClientCodeDetails()
                {
                    status = false
                };
                SelectedClientCodes.Add(selectedClientCode);
                return SelectedClientCodes;
            }
            finally { con.Close(); }

            return SelectedClientCodes;

        }


        public List<ClientCodeDetails> GetProjectCodes(string selectedClient)
        {
            List<ClientCodeDetails> SelectedClientCodes = new List<ClientCodeDetails>();
            con.Open();
            try
            {
                string str = "select DISTINCT Project_code  from CLIENT where CLIENT_CODE='{0}'";
                 string sql = string.Format(str,selectedClient);
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClientCodeDetails selectedClientCode = new ClientCodeDetails()
                    {

                        Project_Code = reader["Project_code"].ToString(),
                    };
                    SelectedClientCodes.Add(selectedClientCode);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { con.Close(); }
            return SelectedClientCodes;

        }

        public List<ClientCodeDetails> GetType(string selectedClient)
        {
            List<ClientCodeDetails> SelectedClientCodes = new List<ClientCodeDetails>();
            con.Open();
            try
            {
                string str = "  select DISTINCT m.[Type] from CLIENT c, MODIFICATION m where c.INTERNAL_CLIENT_NUM = m.INTERNAL_CLIENT_NUM and c.CLIENT_CODE = '{0}'";
                string sql = string.Format(str, selectedClient);
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClientCodeDetails selectedClientCode = new ClientCodeDetails()
                    {
                        Type = reader["Type"].ToString(),
                    };
                    SelectedClientCodes.Add(selectedClientCode);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { con.Close(); }
            return SelectedClientCodes;

        }

        public List<ClientCodeDetails> GetExtensionCodes(string selectedClient)
        {
            List<ClientCodeDetails> SelectedClientCodes = new List<ClientCodeDetails>();
            con.Open();
            try
            {
                string str = "  select DISTINCT m.MODIFICATION_NUM from CLIENT c, MODIFICATION m where c.INTERNAL_CLIENT_NUM = m.INTERNAL_CLIENT_NUM and c.CLIENT_CODE = '{0}'";
                string sql = string.Format(str, selectedClient);
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClientCodeDetails Extension_code = new ClientCodeDetails()
                    {

                        Extension_code = reader["MODIFICATION_NUM"].ToString(),
                    };
                    SelectedClientCodes.Add(Extension_code);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { con.Close(); }
            return SelectedClientCodes;

        }

    }
}