using CRUDangular_1.Models;
using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
             List<string> data = new List<string>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            bool updation_sheet = false;
            bool notification_sheet = false;
            var httpRequest = HttpContext.Current.Request;
            foreach (string file in httpRequest.Files)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                var postedFile = httpRequest.Files[file];
                if (postedFile.FileName.Contains("*updation*"))
                {
                    updation_sheet = true;
                }
                if (postedFile.FileName.Contains("*notification*"))
                {
                    notification_sheet = true;
                }
                if (postedFile != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (postedFile.ContentType == "application/vnd.ms-excel" || postedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = postedFile.FileName;
                        if (notification_sheet)
                        {
                            if (filename.EndsWith(".csv"))
                            {
                                filename = "notificationSample.csv";
                            }
                            else if (filename.EndsWith(".xlsx")) {
                                filename = "notification_sample.xlsx";
                            }
                            else if (filename.EndsWith(".xls")) {
                                filename = "notification_sample.xls";
                            }
                        }
                        else if (updation_sheet)
                        {
                            if (filename.EndsWith(".csv"))
                            {
                                filename = "tutor_updation.csv";
                            }
                            else if (filename.EndsWith(".xlsx"))
                            {
                                filename = "tutor_updation.xlsx";
                            }
                            else if (filename.EndsWith(".xls"))
                            {
                                filename = "tutor_updation.xls";
                            }
                        }
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
                        else if (filename.EndsWith(".csv"))
                        {
                            var fileNam = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Doc\\");
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileNam + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\";");
                        }
                        if (filename.EndsWith(".csv"))
                        {
                            var adapter = new OleDbDataAdapter("SELECT * FROM [notificationSample.csv]", connectionString);
                            var ds = new DataSet();
                            adapter.Fill(ds, "ExcelTable");
                            DataTable dtable = ds.Tables["ExcelTable"];

                        }
                        else
                        {
                            var adapter = new OleDbDataAdapter("SELECT * FROM [sheet1$]", connectionString);
                            var ds = new DataSet();
                            adapter.Fill(ds, "ExcelTable");
                            DataTable dtable = ds.Tables["ExcelTable"];
                        }
                        string sheetName = "Sheet1";
                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        if (updation_sheet == true)
                        {
                            var tutors = from a in excelFile.Worksheet<S_Tutor>(sheetName) select a;
                            foreach (var a in tutors)
                            {
                                tutor T_check = new tutor();
                                T_check = db.tutors.Where(b => b.reg_no == a.registrationNo).FirstOrDefault();
                                if (T_check != null)
                                {
                                    continue;
                                }
                                else { 
                                try
                                {

                                    if (Regex.IsMatch(a.registrationNo, @"/([0-9])\w+([-])\w+([A-])\w+/i"))
                                    {
                                        data.Add("<ul>");
                                        data.Add("<li> Tutor with Registration no:" + a.registrationNo + " is not valid</li>");
                                        data.Add("</ul>");

                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                        return json(stuff);
                                    }
                                    else{
                                        if (a.name != "" || a.registrationNo != "" || a.fatherName != "")
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
                                            TU.gender = "N/A";
                                            TU.image_profile = "../../";
                                            TU.image_updation_form = "../../";
                                            TU.t_status = "unverified";
                                            db.tutors.Add(TU);
                                            db.SaveChanges();
                                            data.Add("Tutor of Registration No:" + a.registrationNo + "is added.!");
                                        }
                                        else
                                        {
                                            if (a.name == "" || a.name == null) data.Add(" name is required");
                                            if (a.fatherName == "" || a.fatherName == null) data.Add("Father name is required");
                                            if (a.registrationNo == "" || a.registrationNo == null) data.Add("Registration No is required");
                                            data.ToArray();
                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                            return json(stuff);
                                        }
                                    }
                                    }
                                        catch (DbEntityValidationException ex)
                                        {
                                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                            {

                                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                                {
                                                    data.Add("Property  " + validationError.PropertyName + " Error: " + validationError.ErrorMessage );
                                                    // Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                                }

                                            }
                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                            return json(stuff);
                                        }
                                }

                                
                            }
                            //deleting excel file from folder  
                            if ((System.IO.File.Exists(pathToExcelFile)))
                            {
                                //System.IO.File.Delete(pathToExcelFile);
                            }
                            data.ToArray();
                            dynamic toDoStuff = JsonConvert.SerializeObject(data);
                            return json(toDoStuff);
                        }
                        else if (notification_sheet == true)
                        {
                            var record = from a in excelFile.Worksheet<S_Notification>(sheetName) select a;
                            foreach (var a in record)
                            {
                                var tutor_id = 0;
                                tutor T_check = new tutor();
                                T_check = db.tutors.Where(b => b.reg_no == a.registration_no).FirstOrDefault();
                                if (T_check != null)
                                {
                                    try
                                    {
                                        if (a.registration_no != "" || a.registration_no != null || a.course_code != "" || a.section != "" || a.course_code != null)
                                        {
                                            tutor_experience TE = new tutor_experience();
                                            tutor t = new tutor();
                                            t = db.tutors.Where(s => s.reg_no == a.registration_no).FirstOrDefault();
                                            tutor_id = t.tutor_id;
                                            if (!experienceCheck(tutor_id, a.course_code, a.semester, a.year))
                                            {
                                                TE.tutor_id = tutor_id;
                                                TE.sr_no = a.sr_no;
                                                TE.registration_no = a.registration_no;
                                                TE.retention = a.retention;
                                                TE.section = a.section;
                                                TE.semester = a.semester;
                                                TE.year = a.year;
                                                TE.study_center = a.study_center;
                                                TE.assignment_total = a.assignment_total;
                                                TE.amount_assignment = a.amount_assignment;
                                                TE.meetings_total = a.meetings_total;
                                                TE.amount_meeting = a.amount_meeting;
                                                TE.postage_total = a.postage_total;
                                                TE.bM_tutor = a.bM_tutor;
                                                TE.group_notification = a.group_notification;
                                                TE.gross_amount = a.gross_amount;
                                                TE.course_code = a.course_code;
                                                TE.notification_SrNo = a.notification_SrNo;
                                                TE.incomTax = a.incomeTax;
                                                TE.net_amount = a.net_amount;
                                                TE.diaryNo_date = a.diaryNo_date;
                                                TE.no_of_stds = a.no_of_stds;
                                                TE.reference = a.reference;
                                                TE.contigency = a.contigency;
                                                db.tutor_experience.Add(TE);
                                                db.SaveChanges();
                                            }
                                            else
                                            {
                                                data.Add("Experience of Reg. No." + a.registration_no + "already exist!");
                                            }
                                        }
                                        else
                                        {
                                            if (a.registration_no == "" || a.registration_no == null) data.Add(" registration no is required  of Registration No." + a.registration_no);
                                            if (a.course_code == "" || a.course_code == null) data.Add("Course code is required of Registration No." + a.registration_no);
                                            if (a.section == "" || a.section == null) data.Add("Section is required  of Registration No." + a.registration_no);
                                            data.ToArray();
                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                            return json(stuff);
                                        }
                                    }

                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {

                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {
                                                data.Add("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage );
                                            }

                                        }
                                        data.ToArray();
                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                        return json(stuff);
                                    }
                                }
                                else {
                                    data.Add("Registration No:"+ a.registration_no + "does not exist. Please enter record of That Tutor first!");
                                    data.ToArray();
                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                    return json(stuff);
                                }
                                
                            }
                            //deleting excel file from folder  
                            if ((System.IO.File.Exists(pathToExcelFile)))
                            {
                                //System.IO.File.Delete(pathToExcelFile);
                            }
                            data.ToArray();
                            dynamic anotherStuff = JsonConvert.SerializeObject(data);
                            return json(anotherStuff);

                        }
                        else
                        {
                            //alert message for invalid file format  
                            data.Add("Only Excel file format is allowed");
                            data.ToArray();

                            dynamic stuff = JsonConvert.SerializeObject(data);
                            return json(stuff);
                        }

                    }

                    else
                    {
                        if (postedFile == null) data.Add("Please choose Excel file");
                        data.ToArray();

                        dynamic stuff = JsonConvert.SerializeObject(data);
                        return json(stuff);
                    }
                }
            }
            //ResponseDTO abc = "result"
            data.ToArray();
            dynamic lastStuff = JsonConvert.SerializeObject(data);
            return json(lastStuff);
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

        static bool experienceCheck(int tutor_id,string course_code,string semester,string year)
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            List<tutor_experience> expe = db.tutor_experience.Where(a => a.tutor_id == tutor_id).ToList();
            foreach (var b in expe)
            {
                if (b.course_code.Contains(course_code))
                {
                    if (b.semester.Contains(semester))
                    {
                        if (b.year.Contains(year))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}