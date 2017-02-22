using CRUDangular_1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace CRUDangular_1.Controllers
{
    public class ExperienceWebApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public tutor_experience Get(int id)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            tutor_experience expe = new tutor_experience();
            expe = db.tutor_experience.Find(id);
            return expe;
        }

        // POST api/<controller>
        public void Post(List<tutor_experience> experience)
        {
            if (ModelState.IsValid)
            {
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
                var constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    for (int i = 0; i < experience.Count; i++)
                    {
                        if (experience[i] != null)
                        {
                            string processQuery = "INSERT INTO tutor_experience VALUES (@institute, @class_level, @subject_taught, @duration,@if_past_at_aiou,@course_codes,@tutor_id)";
                            using (SqlCommand command = new SqlCommand(processQuery, connection))
                            {

                                //command = new SqlCommand(processQuery, connection);
                                command.Parameters.Add(new SqlParameter("@institute", experience[i].institute));
                                command.Parameters.Add(new SqlParameter("@class_level", experience[i].class_level));
                                command.Parameters.Add(new SqlParameter("@subject_taught", experience[i].subject_taught));
                                command.Parameters.Add(new SqlParameter("@duration", experience[i].duration));
                                if( experience[i].if_past_at_aiou == null){
                                    command.Parameters.Add("@if_past_at_aiou",DBNull.Value);
                                }
                                else{
                                command.Parameters.Add(new SqlParameter("@if_past_at_aiou", experience[i].if_past_at_aiou));
                                }
                                if (experience[i].course_codes == null)
                                {
                                    command.Parameters.Add("@course_codes", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.Add(new SqlParameter("@course_codes", experience[i].course_codes));
                                }
                                command.Parameters.Add(new SqlParameter("@tutor_id", experience[i].tutor_id));
                                command.ExecuteNonQuery();
                                
                            }
                        }

                    }

                }
            }
        }

        // PUT api/<controller>/5
        public void Put(tutor_experience expe)
        {
            using (test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(expe).State = System.Data.Entity.EntityState.Modified;
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
}