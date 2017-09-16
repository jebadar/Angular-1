using CRUDangular_1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;

namespace CRUDangular_1.Controllers
{
    public class StudentWebApiController : ApiController
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public List<students_appoint> Get(int id)
        {

            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            
                course_codes CC = new course_codes();
                user UU = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
                semester_current SC = db.semester_current.Where(a => a.section_id == UU.section_id).FirstOrDefault();
                semester_previous SP = db.semester_previous.Where(a => a.semester == SC.semester && a.year_ == SC.year_ && a.section_id == UU.section_id).FirstOrDefault();
                CC = db.course_codes.Where(a => a.id == id).FirstOrDefault();
                int C_ID = CC.id;

                List<students_appoint> SR = new List<students_appoint>();
                var sql = "SELECT * FROM dbo.students_appoint where courseCode_id = '" + C_ID+"'AND semester_id = '"+ SP.id +"' AND status_appoint = 'Not Appointed'";
                var total = db.Database.SqlQuery<students_appoint>(sql).ToList();
            
                foreach (var std in total)
                {
                    std.students_region = db.students_region.Where(a => a.std_id == std.student_id).FirstOrDefault();
                    SR.Add(std);
                }
                
                return SR;
       }

        // POST api/<controller>
        public JsonResult Post( List<appoint> arr)
        {
            List<string> data = new List<string>();
            data.Equals("");
            if (ModelState.IsValid)
            {
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
                user UR = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
                semester_current SC = db.semester_current.Where(a => a.section_id == UR.section_id).FirstOrDefault();
                semester_previous SP = db.semester_previous.Where(a => a.section_id == UR.section_id && a.semester == SC.semester && a.year_ == SC.year_).FirstOrDefault();
                int counter = 0;
                int tutorID = 0;
                int course_codeID = 00;

                tutor TT = new tutor();
                foreach (var std in arr)
                {
                    if (std.course_code_id != null)
                    {
                        course_codeID = Convert.ToInt32(std.course_code_id);
                        if (std.tutor_id != null)
                        {
                            if (std.student_appoint_id != null)
                            {
                                std.semester_id = SP.id;
                                std.updated_by = UR.user_name;
                                std.updated_date = DateTime.Now.ToString();
                                std.is_deleted = "false";
                                db.appoints.Add(std);
                                counter++;
                                students_appoint SA = db.students_appoint.Where(a => a.id == std.student_appoint_id).FirstOrDefault();
                                SA.status_appoint = "appointed";
                                SA.is_deleted = "false";
                                db.Entry(SA).State = System.Data.Entity.EntityState.Modified;
                                tutorID = Convert.ToInt32(std.tutor_id);
                            }
                        }
                    }
                }
                section_tutors ST = db.section_tutors.Where(a => a.section_id == UR.section_id && a.semester_id == SP.id && a.tutor_id == tutorID).FirstOrDefault();
                course_codes CC = db.course_codes.Where(a => a.id == course_codeID).FirstOrDefault();
                string studentsT = ST.students;
                int num = Convert.ToInt32(studentsT);
                num = num + counter;
                ST.students = num.ToString();
                ST.appoint_status = "appointed";
                if (ST.course_code != null)
                {
                    int code = Convert.ToInt32(ST.course_code);
                    if (code != 0)
                    {
                        ST.course_code = ST.course_code + ',' + CC.courseCode;
                    }
                    else
                        ST.course_code = CC.courseCode;
                }
                else
                    ST.course_code = CC.courseCode;

                db.Entry(ST).State = System.Data.Entity.EntityState.Modified;
                TT = db.tutors.Where(a => a.tutor_id == tutorID).FirstOrDefault();

                try 
                {
                    db.SaveChanges();
                    data.Add("Students Alloted Total " + counter + "  To Tutor  " + TT.reg_no);
                }
                catch(Exception exp)
                {
                    data.Add(exp.Message);
                    log.Error(exp);
                }
                
            }
            data.ToArray();
            dynamic stuff = JsonConvert.SerializeObject(data);
            return json(stuff);
        }
        private JsonResult json(dynamic stuff)
        {
            var result = new JsonResult
            {
                Data = JsonConvert.DeserializeObject(stuff)
            };
            return result;
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