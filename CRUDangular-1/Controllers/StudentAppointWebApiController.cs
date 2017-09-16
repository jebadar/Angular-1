using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRUDangular_1.Models;

namespace CRUDangular_1.Controllers
{
    public class StudentAppointWebApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public List<appoint> Get(int id)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            user UR = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
            semester_current SC = db.semester_current.Where(a => a.section_id == UR.section_id).FirstOrDefault();
            semester_previous SP = db.semester_previous.Where(a => a.semester == SC.semester &&a.year_ == SC.year_ && a.section_id == UR.section_id).FirstOrDefault();
            
            
            List<appoint> App = new List<appoint>();
            App = db.appoints.Where(a => a.semester_id == SP.id && a.tutor_id == id).ToList();
            foreach (var std in App)
            {
                db.Entry(std).Reference(s => s.students_appoint).Load();
                //db.Entry(std).Reference(s => s.course_codes).Load();
                std.students_appoint.students_region = db.students_region.Where(a => a.std_id == std.students_appoint.student_id).FirstOrDefault();
            }
            return App;
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