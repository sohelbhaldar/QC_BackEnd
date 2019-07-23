
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ClientSide.DAL;
using ClientSide.Models;


namespace ClientSide.Controllers
{


    [RoutePrefix("api/CheckPoints")]  
    public class CheckPointsController : ApiController
    {

        
        [Route("LoadCheckPoints")]
        public IHttpActionResult GetCheckPoints()
        {
            Check_Points_List pts = new Check_Points_List();

            List<KeyValuePair<string, CheckPoints>> checkpts = pts.CheckPointsList();

            return Ok(checkpts);
        }
        
        [Route("LoadClientCodes")]
        public IHttpActionResult GetClientCodes()
        {
           
            ClientCodeData cd = new ClientCodeData();
            //string str = System.Environment.UserName;

            HttpRequest currentRequest = HttpContext.Current.Request;
            
            string str1 = currentRequest.LogonUserIdentity.Name;

           // String ecname = System.Environment.MachineName;
            WindowsIdentity identity1 = HttpContext.Current.Request.LogonUserIdentity;
            //string[] str = currentRequest.LogonUserIdentity.User;
            //System.Security.Principal.SecurityIdentifier str12 = new SecurityIdentifier(;
            //str12 = currentRequest.LogonUserIdentity.User;

            List<ClientCodeDetails> listClientCodes = cd.GetClientCodes(currentRequest.LogonUserIdentity.Name);
            return Ok(listClientCodes);
        }

        [Route("LoadSavedUserDATA")]
        public IHttpActionResult PostLoadSavedUserDATA([FromBody] ClientCodeDetails ccd)
        {
            Check_Points_List cd = new Check_Points_List();
            List<CheckPoints> LoadSavedUserDATA = cd.LoadSavedUserDATA(ccd.Client_Code,ccd.Project_Code,ccd.Extension_code);
            return Ok(LoadSavedUserDATA);
        }

        [Route("ProjectCodes")]
        public IHttpActionResult GetProjectCodes([FromUri] string ClientCode = null)
        {
            ClientCodeData cd = new ClientCodeData();
            List<ClientCodeDetails> listProjectCodes = cd.GetProjectCodes(ClientCode);
            return Ok(listProjectCodes);
        }

        [Route("Type")]
        public IHttpActionResult GetType([FromUri] string ClientCode = null)
        {
            ClientCodeData cd = new ClientCodeData();
            List<ClientCodeDetails> listTypes = cd.GetType(ClientCode);
            return Ok(listTypes);
        }

        [Route("ExtensionCodes")]
        public IHttpActionResult GetExtensionCodes([FromUri] string ClientCode = null)
        {
            ClientCodeData cd = new ClientCodeData();
            List<ClientCodeDetails> listExtensionCodes = cd.GetExtensionCodes(ClientCode);
            return Ok(listExtensionCodes);
        }

        [Route("Save")]
        public IHttpActionResult PostSaveData([FromBody]Save s)
        {
            Load_Data user = new Load_Data();
            Save savedStatus = user.SaveClientInfo(s.Client_Code, s.Project_Code, s.Ext_Code, s.Type, s.TFS_Development_Id, s.TFS_Source_Code);
            return Ok(savedStatus);
        }
        //[Route("Validate")]
        //public IHttpActionResult PostValidate([FromBody]Save s)
        //{
        //    Load_Data user = new Load_Data();
        //    Status status = user.LoadData(s.Client_Code, s.Project_Code, s.Ext_Code, 
        //                            s.Type, s.TFS_Development_Id, s.TFS_Source_Code);
        //    return Ok(status);
        //}

        [Route("saveRadioButtonCheckPoint")]
        public IHttpActionResult PostsaveRadioButtonCheckPoint([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();
            Status status = user.SaveClientData(s.Check_Id, s.Client_Code,
                    s.Project_Code, s.Ext_Code, s.Developer, s.QC_Resource, s.Comment, s.selectedUser,s.username);
            return Ok(status);
        }

        [Route("saveRadioButtonCheckPoint1")]
        public IHttpActionResult PostsaveRadioButtonCheckPoint1([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();
           
            Status status = user.
                SaveClientData1(s.Check_Id, s.Client_Code,s.Project_Code, s.Ext_Code, 
                                s.Developer_1, s.QC_Resource_1, s.Comment_1, s.selectedUser, s.username_1);
            return Ok(status);
        }


        [Route("saveRadioButtonCheckPoint2")]
        public IHttpActionResult PostsaveRadioButtonCheckPoint2([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();
            Status status = user.
                SaveClientData2(s.Check_Id, s.Client_Code, s.Project_Code, s.Ext_Code,
                                 s.Developer_2, s.QC_Resource_2, s.Comment_2, s.selectedUser, s.username_2);
            return Ok(status);
        }
        [Route("saveCommentCheckPoint")]
        public IHttpActionResult PostsaveCommentCheckPoint([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();
            Status status = user.SaveComment(s.Check_Id, s.Client_Code,
                    s.Project_Code, s.Ext_Code,s.Comment, s.selectedUser, s.username);
            return Ok(status);
        }

        [Route("saveCommentCheckPoint1")]
        public IHttpActionResult PostsaveCommentCheckPoint1([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();

            Status status = user.SaveComment1(s.Check_Id, s.Client_Code, 
                s.Project_Code , s.Ext_Code, s.Comment_1, s.selectedUser, s.username_1);
            return Ok(status);
        }


        [Route("saveCommentCheckPoint2")]
        public IHttpActionResult PostsaveCommentCheckPoint2([FromBody]saveRadioButtonCheckPoint s)
        {
            Load_Data user = new Load_Data();
            Status status = user.
                SaveComment2(s.Check_Id, s.Client_Code, s.Project_Code, s.Ext_Code,
                                s.Comment_2, s.selectedUser, s.username_2);
            return Ok(status);
        }

        [Route("submitClientInfo")]
        public IHttpActionResult SubmitClientInfo([FromBody]Save s)
        {
            Load_Data user = new Load_Data();
            bool status = user.SubmitClientInfo(s.Initial_QC, s.QC_Rework_1st, s.QC_Rework_2nd,
                                                s.Client_Code, s.Project_Code, s.Ext_Code);
                           return Ok(status);
        }

        [Route("LoadCodeReviewCheckPoints")]
        public IHttpActionResult GetCodeReviewCheckPoints()
        {
            Check_Points_List pts = new Check_Points_List();

            List<CodeReviewCheckPoints> checkpts = pts.GetCodeReviewCheckPoints();

            return Ok(checkpts);
        }

    }
}


