using CRUDangular_1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CRUDangular_1.Controllers
{
    public class EmployeeWebApiController : ApiController
    {

        [HttpGet]
        // GET api/<controller>
        public IEnumerable<employee> Get()
        {
            test_Applicata_DataBaseEntities5 db = new test_Applicata_DataBaseEntities5();
           
            List<employee> employee = db.employees.ToList();
            return employee;
        }



        // GET api/<controller>/5
        public employee Get(int id)
        {
            using (test_Applicata_DataBaseEntities5 db = new test_Applicata_DataBaseEntities5())
            {
                return db.employees.Find(id);
            }
            //return "Test";
        }

        // POST api/<controller>
        public void Post(employee employee)
        {
            if (ModelState.IsValid)
            {
                test_Applicata_DataBaseEntities5 db = new test_Applicata_DataBaseEntities5();
                db.employees.Add(employee);

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

                // db.SaveChanges();
            };



        }

        // PUT api/<controller>/5
        public void Put(employee employee)
        {
            using (test_Applicata_DataBaseEntities5 db = new test_Applicata_DataBaseEntities5())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(employee).State = EntityState.Modified;
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
                        throw exception(e);
                    }
                }
                
            }

        }

        private Exception exception(DbEntityValidationException e)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            using (test_Applicata_DataBaseEntities5 db = new test_Applicata_DataBaseEntities5())
            {
                employee emp = db.employees.Find(id);
                db.employees.Remove(emp);
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
                    throw exception(e);
                }
            }
        }
    }
}