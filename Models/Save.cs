using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models
{
    public class Save
    {
        public bool  status { get; set; }

        public int Check_Id { get; set; }

        public string Check_Points { get; set; }

        public string Category { get; set; }


        public string Client_Code { get; set; }

        public string Project_Code { get; set; }

        public string Ext_Code { get; set; }

        public string Type { get; set; }

        public string TFS_Development_Id { get; set; }

        public string TFS_Source_Code { get; set; }

        public string Initial_QC { get; set; }

        public string QC_Rework_1st { get; set; }

        public string QC_Rework_2nd { get; set; }


    }
}