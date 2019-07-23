using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models
{
    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SelectedCategory { get; set; }
        public string Developer { get; set; }
        public string Code_Reviewer { get; set; }
    }
}