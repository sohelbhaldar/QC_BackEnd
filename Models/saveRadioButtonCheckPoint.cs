using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models
{
    public class saveRadioButtonCheckPoint
    {
        //Check_Id, Client_Code, Project_Code, Ext_Code,Developer,QC_Resource,Comment,selectedUser
        public int Check_Id { get; set; }
        public string Client_Code { get; set; }
        public string Project_Code { get; set; }
        public string Ext_Code { get; set; }
        public string Developer { get; set; }
        public string QC_Resource { get; set; }
        public string Comment { get; set; }
        public string selectedUser { get; set; }
        public string username { get; set; }
        public string Developer_1 { get; set; }
        public string QC_Resource_1 { get; set; }
        public string Comment_1 { get; set; }       
        public string username_1 { get; set; }
        public string Developer_2 { get; set; }
        public string QC_Resource_2 { get; set; }
        public string Comment_2 { get; set; }
        public string username_2 { get; set; }
    }
}