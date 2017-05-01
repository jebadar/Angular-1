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
using System.Web;
using System.Configuration; // For generic collections like List.


namespace CRUDangular_1.Controllers
{
    [Authorize]
    public class TutorWebApiController : ApiController
    {
        // GET api/<controller>
        public List<tutor> Get()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
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
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
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
                newTutor.t_status = "unverified";
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
               // var constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //using (SqlConnection connection = new SqlConnection(constr))
               // {
                   // connection.Open();
                   // string processQuery = "INSERT INTO dbo.tutor VALUES (@name, @reg_no)";
                   // using (SqlCommand command = new SqlCommand(processQuery, connection))
                    //{
                      //  command.Parameters.Add(new SqlParameter("@name", newTutor.name));
                        //command.Parameters.Add(new SqlParameter("@f_name", newTutor.f_name));
                       // command.Parameters.Add(new SqlParameter("@reg_no", newTutor.reg_no));
                       // if (newTutor.designation == null)
                        //{
                       //     command.Parameters.Add(new SqlParameter("@designation", DBNull.Value));
                      //  }
                        //else
                       // command.Parameters.Add(new SqlParameter("@designation", newTutor.designation));

                       // if (newTutor.basic_pay_scale == null)
                     //   {
                     //       command.Parameters.Add(new SqlParameter("@basic_pay_scale", DBNull.Value));
                       // }
                       // else
                       // command.Parameters.Add(new SqlParameter("@basic_pay_scale", newTutor.basic_pay_scale));

                       // if (newTutor.official_add == null)
                       // {
                         //   command.Parameters.Add(new SqlParameter("@official_add", DBNull.Value));
                        //}
                       // else
                       // command.Parameters.Add(new SqlParameter("@official_add", newTutor.official_add));

                        //command.Parameters.Add(new SqlParameter("@mail_add", newTutor.mail_add));
                       //command.Parameters.Add(new SqlParameter("@qulification", newTutor.qulification));
                        //command.Parameters.Add(new SqlParameter("@phone", newTutor.phone));
                        //command.Parameters.Add(new SqlParameter("@tehsil", newTutor.tehsil));
                       // command.Parameters.Add(new SqlParameter("@gender", newTutor.gender));
                       // command.Parameters.Add(new SqlParameter("@image_profile", newTutor.image_profile));
                       // command.Parameters.Add(new SqlParameter("@image_updation_form", newTutor.image_updation_form));
                       // command.Parameters.Add(new SqlParameter("@t_status", newTutor.t_status));
                       // command.Parameters.Add(new SqlParameter("@srNo", DBNull.Value));
                       // command.Parameters.Add(new SqlParameter("@intelCertificate", DBNull.Value));
                        //command.Parameters.Add(new SqlParameter("@update_by", "admin"));
                        //command.Parameters.Add(new SqlParameter("@update_date", DateTime.Now));


                       // command.ExecuteNonQuery();
                    //}
                    //connection.Close();
               // }
                //newTutor.tutor_id = DBNull.Value;
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
            using (test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities())
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