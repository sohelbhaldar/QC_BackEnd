using ClientSide.DAL;
using ClientSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ClientSide.Controllers
{
    public class QcSheetController : ApiController
    {
        
        [ResponseType(typeof(Load))]
        public IHttpActionResult PostClientEntry(Load l)
        {
            Load_Data user = new Load_Data();
           // Load fetchedUser  = user.LoadData(l.Client_Code,l.Ext_Code,l.TFS_Development_Id,l.TFS_Source_Code);
            return Ok();
        }
    }
}
