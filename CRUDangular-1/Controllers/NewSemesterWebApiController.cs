using CRUDangular_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class NewSemesterWebApiController : ApiController
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string semester { get; set; }
        private string year { get; set; }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post()
        {
            

        }

        // PUT api/<controller>/5
        public void Put(semester value)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            user US = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
            if (checkPreviousSemester(value, Convert.ToInt32(US.section_id)))
            {
                tutorListEntry(value);
                newEntrySemester(value, Convert.ToInt32(US.section_id));
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private bool checkPreviousSemester(semester value, int section_id)
        {
            semester_previous SP = new  semester_previous();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            SP = db.semester_previous.Where(a => a.semester == value.semester_ && a.year_ == value.year && a.section_id == section_id).FirstOrDefault();
            if (SP == null)
            {
               semester_current SC = db.semester_current.Where(a => a.section_id == section_id).FirstOrDefault();
               semester = SC.semester;
               year = SC.year_;
                semester_previous SP1 = new semester_previous();
                SP1.section_id = section_id;
                SP1.semester = value.semester_;
                SP1.year_ = value.year;
                SP1.updated_by = User.Identity.Name.ToString();
                SP1.updated_date = DateTime.Now.ToString();
                db.semester_previous.Add(SP1);
                try { db.SaveChanges(); }
                catch (Exception exp)
                {
                    log.Error(exp);
                }
                return true;
            }
            else if (SP != null)
            {
                semester = SP.semester;
                year = SP.year_;
                return false;
            }
            else
                return false;
        }

        private void newEntrySemester(semester value,int section_id)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            semester_current SC = new semester_current();
            SC = db.semester_current.Where(a => a.section_id == section_id).FirstOrDefault();
            SC.semester = value.semester_;
            SC.year_ = value.year;
            SC.updated_by = User.Identity.Name.ToString();
            SC.updated_date = DateTime.Now.ToString();
            db.Entry(SC).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exp)
            {
                log.Error(exp);

            }
        }

        private void tutorListEntry(semester value)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            user US = new user();
            US = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
            semester_current SC = db.semester_current.Where(a => a.section_id == US.section_id).FirstOrDefault();
            List<section_tutors> ST = new List<section_tutors>();
            semester_previous SP = db.semester_previous.Where(a => a.semester == semester && a.year_ == year && a.section_id == US.section_id).FirstOrDefault();
            ST = db.section_tutors.Where(a => a.semester_id == SP.id && a.section_id == US.section_id).ToList();
            semester_previous NewSP = db.semester_previous.Where(a => a.semester == value.semester_ && a.year_ == value.year && a.section_id == US.section_id).FirstOrDefault();
            foreach (var tutor in ST)
            {
                section_tutors singleTutor = new section_tutors();
                singleTutor.section_id = tutor.section_id;
                singleTutor.tutor_id = tutor.tutor_id;
                singleTutor.students = "00";
                singleTutor.appoint_status = "Not Appointed";
                singleTutor.semester_id = NewSP.id;
                db.section_tutors.Add(singleTutor);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception exp)
            {
                log.Error(exp);
            }
        }
    }
}