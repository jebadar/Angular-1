using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using CRUDangular_1.Models;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class RoleWebApiController : ApiController
    {
        // GET api/<controller>
        public checkUser Get()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            user UR = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault<user>();
            db.Entry(UR).Reference(s => s.user_roles).Load();
            section_region SR = db.section_region.Where(a => a.section_id == UR.section_id).FirstOrDefault();
            semester_current SC = db.semester_current.Where(a => a.section_id == SR.section_id).FirstOrDefault();
            checkUser CU = new checkUser();
            CU.user_name = UR.user_name;
            CU.u_password = UR.u_password;
            CU.user_roles = UR.user_roles;
            CU.name = UR.name;
            CU.profile_pic = UR.profile_pic;
            CU.program = UR.program;
            CU.section_id = Convert.ToInt32( UR.section_id);
            CU.designation = UR.designation;
            CU.semester_current = SC;
            return CU;
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