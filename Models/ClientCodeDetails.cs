using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models
{
    public class ClientCodeDetails
    {
        public bool status  { get; set; }
        public string user { get; set; }
        public int InternalClientNumber { get; set; }
        public string Client_Code { get; set; }
        public string Project_Code { get; set; }
        public string Extension_code { get; set; }
        public string Type { get; set; }


    }
}