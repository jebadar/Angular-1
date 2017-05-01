using CRUDangular_1.Models;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class QualificationWebApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public tutor_qualification Get(int id)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            tutor_qualification qual = new tutor_qualification();
            qual = db.tutor_qualification.Find(id);
            return qual;
        }

        // POST api/<controller>
        public void Post(List<tutor_qualification> qualification)
        {

            if (ModelState.IsValid)
            {
                string tutorId = qualification[0].tutor_id.ToString();
                test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
                var constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(constr))
                {
                      connection.Open();
                        for (int i = 0; i < qualification.Count; i++)
                        {
                            
                            if (qualification[i] != null)
                            {

                                string processQuery = "INSERT INTO tutor_qualification VALUES (@degree_name, @institute_name, @date_of_complete, @pass_division,@is_attested,@tutor_id,@image_degree,@is_deleted,@update_by,@update_date)";
                                using (SqlCommand command = new SqlCommand(processQuery, connection))
                                {

                                    //command = new SqlCommand(processQuery, connection);
                                    command.Parameters.Add(new SqlParameter("@degree_name", qualification[i].degree_name));
                                    command.Parameters.Add(new SqlParameter("@institute_name", qualification[i].institute_name));
                                    command.Parameters.Add(new SqlParameter("@date_of_complete", qualification[i].date_of_complete));
                                    command.Parameters.Add(new SqlParameter("@pass_division", qualification[i].pass_division));
                                    command.Parameters.Add(new SqlParameter("@is_attested", qualification[i].is_attested));
                                    command.Parameters.Add(new SqlParameter("@tutor_id", qualification[i].tutor_id));
                                    if (qualification[i].image_degree == null)
                                    {
                                        command.Parameters.Add("@image_degree", "../../");
                                    }
                                    else
                                    {
                                        command.Parameters.Add(new SqlParameter("@image_degree", qualification[i].image_degree));
                                    }
                                    command.Parameters.Add("@is_deleted", DBNull.Value);
                                    command.Parameters.Add("@update_by", "admin");
                                    command.Parameters.Add("@update_date", DateTime.Now);
                                    
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                        db.Database.ExecuteSqlCommand("UPDATE tutor SET dbo.tutor.t_status = 'verified' WHERE dbo.tutor.tutor_id =" + tutorId);
                        connection.Close();
                }
            }

        }

        // PUT api/<controller>/5
        public void Put(tutor_qualification qual)
        {
            using (test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(qual).State = System.Data.Entity.EntityState.Modified;
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