using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRUDangular_1.Models;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.SqlClient;
using System;                     // For system functions like Console.
using System.Collections.Generic;
using System.Data.Entity;
using System.Web; // For generic collections like List.


namespace CRUDangular_1.Controllers
{
    [Authorize]
    public class TutorWebApiController : ApiController
    {
        // GET api/<controller>
        public List<tutor> Get()
        {
            test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1();
            var httpRequest = HttpContext.Current.Request;
            string page = httpRequest.QueryString["page"];
            int num = Int32.Parse(page);
            int limit = 10;
            int pageSize = 0;
            int reducePageSize = 0;
            if (num == 1)
            {
                pageSize = num * limit;
            }
            else if (num > 1)
            {
                reducePageSize = (num - 1) * limit;
                pageSize = num * limit;
            }
            List<tutor> tutor = db.tutors.ToList();
            var tutors = tutor.Skip(reducePageSize).Take(pageSize);
            return tutors.ToList();
        }
        
        public tutor s_tutor {get; set;}

        // GET api/<controller>/5
        public full_record Get(int id)
        {
            test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1();
            full_record record = new full_record();
            record.i_tutor = db.tutors.Find(id);
            record.i_tutor_experience = db.tutor_experience.Where(a => a.tutor_id == id).ToList();
            record.i_tutor_qualification = db.tutor_qualification.Where(a=> a.tutor_id == id).ToList();
            return record;
        }

        

        // POST api/<controller>
        public void Post(tutor newTutor)
        {
            if (ModelState.IsValid)
            {
                test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1();
                newTutor.t_status = "unverified";
                db.tutors.Add(newTutor);
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

        }

        // PUT api/<controller>/5
        public void Put(tutor tutor)
        {
            using (test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1())
            {
                if (ModelState.IsValid)
                {
                    
                    db.Entry(tutor).State = System.Data.Entity.EntityState.Modified;
                        try
                        {
                            // Your code...
                            // Could also be before try if you know the exception occurs in SaveChanges

                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                        foreach (var ve in eve.ValidationErrors)
                                        {
                                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                                ve.PropertyName, ve.ErrorMessage);
                                        }
                                }
                                throw;
                        }
                }
            }

         }
         

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        
    }

    public class full_record
    {
        public tutor i_tutor { get; set; }
        public List<tutor_experience> i_tutor_experience { get; set; }
        public List<tutor_qualification> i_tutor_qualification { get; set; }
    }
}