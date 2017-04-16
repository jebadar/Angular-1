using CRUDangular_1.Models;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CRUDangular_1.Controllers
{
    public class ImportWebApiController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var path = HttpContext.Current.Server.MapPath("~/Doc/sample.xlsx");
            var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return response;
            //byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            //string fileName = "sample.xlsx";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [System.Web.Http.Authorize]
        public JsonResult Post()
        {
            test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1();
            List<string> data = new List<string>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var httpRequest = HttpContext.Current.Request;
            foreach (string file in httpRequest.Files)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                var postedFile = httpRequest.Files[file];
                if (postedFile != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (postedFile.ContentType == "application/vnd.ms-excel" || postedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = postedFile.FileName;
                        string targetpath = HttpContext.Current.Server.MapPath("~/Doc/");
                        postedFile.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                        }

                        var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                        var ds = new DataSet();
                        adapter.Fill(ds, "ExcelTable");
                        DataTable dtable = ds.Tables["ExcelTable"];
                        string sheetName = "Sheet1";
                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var tutors = from a in excelFile.Worksheet<S_Tutor>(sheetName) select a;
                        foreach (var a in tutors)
                        {
                            try
                            {
                                if (a.name != "" && a.fatherName != "" && a.registrationNo != "")
                                {
                                    tutor TU = new tutor();
                                    TU.srNo = a.srNo;
                                    TU.reg_no = a.registrationNo;
                                    TU.gender = a.gender;
                                    TU.f_name = a.fatherName;
                                    TU.name = a.name;
                                    TU.designation = a.designation;
                                    TU.basic_pay_scale = a.bps;
                                    TU.official_add = a.officialAddress;
                                    TU.mail_add = a.newAddress;
                                    TU.phone = a.phoneNo;
                                    TU.qulification = a.qualification;
                                    TU.tehsil = a.tehsil;
                                    TU.intelCertificate = a.intelCertificate;
                                    db.tutors.Add(TU);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.name == "" || a.name == null) data.Add("<li> name is required</li>");
                                    if (a.fatherName == "" || a.fatherName == null) data.Add("<li> Father name is required</li>");
                                    if (a.registrationNo == "" || a.registrationNo == null) data.Add("<li>Registration No is required</li>");
                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                            }

                            catch (DbEntityValidationException ex)
                            {
                                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                {

                                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                                    {
                                        return Json(data, JsonRequestBehavior.AllowGet);
                                        // Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                    }

                                }
                            }
                        }
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //alert message for invalid file format  
                        data.Add("<ul>");
                        data.Add("<li>Only Excel file format is allowed</li>");
                        data.Add("</ul>");
                        data.ToArray();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                }

                else
                {
                    data.Add("<ul>");
                    if (postedFile == null) data.Add("<li>Please choose Excel file</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Json(string p, JsonRequestBehavior jsonRequestBehavior)
        {
            throw new NotImplementedException();
        }

        private JsonResult Json(List<string> data, JsonRequestBehavior jsonRequestBehavior)
        {
            throw new NotImplementedException();
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