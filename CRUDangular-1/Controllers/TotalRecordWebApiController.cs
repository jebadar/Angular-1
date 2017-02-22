using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using CRUDangular_1.Models;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
namespace CRUDangular_1.Controllers
{
    public class TotalRecordWebApiController : ApiController
    {
        // GET api/<controller>
        public String Get()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            var sql = "SELECT COUNT(*) FROM dbo.tutors";
            var total = db.Database.SqlQuery<int>(sql).Single();
            string value = total.ToString();
            return value;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}