using CRUDangular_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class CourseCodeWebApiController : ApiController
    {
        // GET api/<controller>
        public List<course_codes> Get()
        {
            user UU = new user();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            UU = db.users.Where(a => a.user_name == User.Identity.Name.ToString()).FirstOrDefault();
            List<course_codes> CCList = new List<course_codes>();
            CCList = db.course_codes.Where(a => a.section_id == UU.section_id).ToList();
            return CCList;
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