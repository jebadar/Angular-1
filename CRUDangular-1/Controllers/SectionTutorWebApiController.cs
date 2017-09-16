using CRUDangular_1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CRUDangular_1.Controllers
{
    public class SectionTutorWebApiController : ApiController
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/<controller>
        public List<section_tutors> Get()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            user UR = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();

            List<section_tutors> ST = new List<section_tutors>();
            ST = db.section_tutors.Where(a => a.section_id == UR.section_id).ToList();
            foreach (var std in ST)
            {
                db.Entry(std).Reference(s => s.tutor).Load();
            }


            semester_current SC = db.semester_current.Where(a =>a.section_id == UR.section_id).FirstOrDefault();
            
            semester_previous SP = new semester_previous();
            SP = db.semester_previous.Where(a => a.semester == SC.semester && a.year_ == SC.year_ && a.section_id == UR.section_id).FirstOrDefault();
            ST = ST.Where(a => a.semester_id == SP.id).ToList();
            return ST;
        }

        // GET api/<controller>/5
        public string Get(string id)
        {
            if (id == "Total")
            {
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
                user US = db.users.Where(a => a.user_name == User.Identity.Name.ToString()).FirstOrDefault();
                semester_current SC = db.semester_current.Where(a => a.section_id == US.section_id).FirstOrDefault();
                semester_previous SP = db.semester_previous.Where(a => a.semester == SC.semester && a.year_ == SC.year_ && a.section_id == US.section_id).FirstOrDefault();
                var sql = "SELECT COUNT(*) FROM dbo.section_tutors where semester_id ='" + SP.id +"' AND section_id ="+ SC.section_id + " ";
                var total = db.Database.SqlQuery<int>(sql).Single();
                string value = total.ToString();
                return value;
            }
            else
            {
                int total = Convert.ToInt32(id);
            }
            return "value";
        }

        // POST api/<controller>
        public JsonResult Post(List<int> ids)
        {
            List<string> data = new List<string>();
            data.Equals("");
            List<section_tutors> ST = convertTutorIdToObject(ids);
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            foreach (var SingleT in ST)
            {
                try
                {
                    db.section_tutors.Add(SingleT);
                    db.SaveChanges();
                }
                catch (Exception exp)
                {
                    log.Error(exp);
                    data.Add(exp.Message);
                    data.ToArray();
                    dynamic stuff = JsonConvert.SerializeObject(data);
                    return json(stuff);
                }
            }
            data.Add("Succesfully Added!");
            data.ToArray();
            dynamic stuff1 = JsonConvert.SerializeObject(data);
            return json(stuff1);
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
        private List<section_tutors> convertTutorIdToObject(List<int> Ids) 
        {
            List<section_tutors> ST = new List<section_tutors>();
            user U = new user();
            semester_previous SP = new semester_previous();
            semester_current SC = new semester_current();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            U = db.users.Where(a => a.user_name == User.Identity.Name).FirstOrDefault();
            SC = db.semester_current.Where(a => a.section_id == U.section_id).FirstOrDefault();
            SP = db.semester_previous.Where(a => a.semester == SC.semester && a.year_ == SC.year_ && a.section_id == U.section_id).FirstOrDefault();
            foreach(var Id in Ids)
            {
                section_tutors SingleST = new section_tutors();
                SingleST.tutor_id = Id;
                SingleST.section_id = Convert.ToInt32(U.section_id);
                SingleST.appoint_status = "Not Appointed";
                SingleST.students = "00";
                SingleST.semester_id = SP.id;
                ST.Add(SingleST);
            }
            return ST;
        }
    }
}