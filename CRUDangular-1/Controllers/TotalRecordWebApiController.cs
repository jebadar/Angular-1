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
using NinjaNye.SearchExtensions;
namespace CRUDangular_1.Controllers
{
    public class TotalRecordWebApiController : ApiController
    {
        // GET api/<controller>
        public String Get()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            var sql = "SELECT COUNT(*) FROM dbo.tutor";
            var total = db.Database.SqlQuery<int>(sql).Single();
            string value = total.ToString();
            return value;
        }

        // GET api/<controller>/5
        public List<tutor> Get(string id)
        {
            char[] str = id.ToCharArray();
            int length = str.Length;
            List<tutor> t = new List<tutor>();
            var result = t;
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            if (str[length - 1] == 'N')
            {
                string b = "-N";
                char[] n = b.ToCharArray();
                string query = id.Trim(n);
                t = db.tutors.Search(x => x.name).Containing(query).ToList();
                return t;
            }
            if (str[length - 1] == 'R')
            {
                string b = "-R";
                char[] n = b.ToCharArray();
                string query = id.Trim(n);
                t = db.tutors.Search(x => x.reg_no).Containing(query).ToList();
                return t;
            }
            if (str[length - 1] == 'A')
            {
                string b = "-A";
                char[] n = b.ToCharArray();
                string query = id.Trim(n);
                t = db.tutors.Search(x => x.mail_add).Containing(query).ToList();
                return t;
            }
            if (str[length - 1] == 'V')
            {
       //         string b = "-V";
     //           char[] n = b.ToCharArray();
   //             string query = id.Trim(n);
 //               t = db.tutors.Search(x => x.t_status).Containing("verified").ToList();

                var sql = "SELECT * FROM dbo.tutor where t_status = 'verified'";
                var total = db.Database.SqlQuery<tutor>(sql).ToList();
                return total;
            }
            if (str[length - 1] == 'U')
            {
                string b = "-U";
                char[] n = b.ToCharArray();
                string query = id.Trim(n);
                t = db.tutors.Search(x => x.t_status).Containing("unverified").ToList();
                return t;
            }

            return t;
           
        }

        public string[] strToArr(string str) {
            return new[] { str };
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