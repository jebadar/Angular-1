using ClosedXML.Excel;
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
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        List<string> data = new List<string>();
        // POST api/<controller>
        [System.Web.Http.Authorize]
        public JsonResult Post()
        {
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            data.Equals("");
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

                            if (filename.EndsWith(".xlsx")) {
                                filename = "notification_sample.xlsx";
                            }
                            else if (filename.EndsWith(".xls")) {
                                filename = "notification_sample.xls";
                            }
                        }
                        else if (updation_sheet)
                        {

                            if (filename.EndsWith(".xlsx"))
                            {
                                filename = "tutor_updation.xlsx";
                            }
                            else if (filename.EndsWith(".xls"))
                            {
                                filename = "tutor_updation.xls";
                            }
                        }
                        try
                        {
                            string targetpath = HttpContext.Current.Server.MapPath("~/Doc/");
                            postedFile.SaveAs(targetpath + filename);
                            string pathToExcelFile = targetpath + filename;

                            using (XLWorkbook workBook = new XLWorkbook(pathToExcelFile))
                            {
                                //Read the first Sheet from Excel file.
                                IXLWorksheet workSheet = workBook.Worksheet(1);

                                //Create a new DataTable.
                                DataTable dt = new DataTable();

                                //Loop through the Worksheet rows.
                                bool firstRow = true;

                                #region updationSheet
                                if (updation_sheet == true)
                                {
                                    List<S_Tutor> TT = new List<S_Tutor>();
                                    try
                                    {
                                        foreach (IXLRow row in workSheet.Rows())
                                        {
                                            //Use the first row to add columns to DataTable.
                                            if (firstRow)
                                            {
                                                foreach (IXLCell cell in row.Cells())
                                                {
                                                    dt.Columns.Add(cell.Value.ToString());
                                                }
                                                #region FirstRowCheck
                                                if (dt.Columns[0].ToString() == "srNo")
                                                {
                                                    if (dt.Columns[1].ToString() == "registrationNo")
                                                    {
                                                        if (dt.Columns[2].ToString() == "gender")
                                                        {
                                                            if (dt.Columns[3].ToString() == "name")
                                                            {
                                                                if (dt.Columns[4].ToString() == "fatherName")
                                                                {
                                                                    if (dt.Columns[5].ToString() == "designation")
                                                                    {
                                                                        if (dt.Columns[6].ToString() == "bps")
                                                                        {
                                                                            if (dt.Columns[7].ToString() == "officialAddress")
                                                                            {
                                                                                if (dt.Columns[8].ToString() == "newAddress")
                                                                                {
                                                                                    if (dt.Columns[9].ToString() == "phoneNo")
                                                                                    {
                                                                                        if (dt.Columns[10].ToString() == "qualification")
                                                                                        {
                                                                                            if (dt.Columns[11].ToString() == "tehsil")
                                                                                            {
                                                                                                if (dt.Columns[12].ToString() == "intelCertificate")
                                                                                                {
                                                                                                    firstRow = false;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name intel Certificate Missing!");
                                                                                                    data.ToArray();

                                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                    return json(stuff);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name tehsil Missing!");
                                                                                                data.ToArray();

                                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                return json(stuff);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name Qualification Missing!");
                                                                                            data.ToArray();

                                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                            return json(stuff);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name phoneNo Missing!");
                                                                                        data.ToArray();

                                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                        return json(stuff);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name newAddress Missing!");
                                                                                    data.ToArray();

                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                    return json(stuff);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name officialAddress Missing!");
                                                                                data.ToArray();

                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                return json(stuff);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name bps Missing!");
                                                                            data.ToArray();

                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                            return json(stuff);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name designation Missing!");
                                                                        data.ToArray();

                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                        return json(stuff);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name fatherName Missing!");
                                                                    data.ToArray();

                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                    return json(stuff);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'name' Missing!");
                                                                data.ToArray();

                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                return json(stuff);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'gender' Missing!");
                                                            data.ToArray();

                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                            return json(stuff);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'registrationNo' Missing!");
                                                        data.ToArray();

                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                        return json(stuff);
                                                    }
                                                }
                                                else
                                                {
                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'srNo' Missing!");
                                                    data.ToArray();

                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                    return json(stuff);
                                                }
                                                #endregion
                                                firstRow = false;
                                            }
                                            else
                                            {
                                                //Add rows to DataTable.
                                                dt.Rows.Add();
                                                int i = 0;
                                                foreach (IXLCell cell in row.Cells())
                                                {
                                                    if (cell.Value.ToString() != null)
                                                    {
                                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                                        i++;
                                                    }
                                                    else
                                                    {
                                                        data.Add("A cell in row of table contain a null value! Whole Table Must not Contain any Null Value. Please Fill the Cell");
                                                        data.ToArray();

                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                        return json(stuff);
                                                    }
                                                }

                                            }

                                        }

                                        TT = ConvertDataTableTutor(dt);
                                        if (TT.Count != 0)
                                        {
                                            var counterEnter = 0;
                                            foreach (var tutor in TT)
                                            {
                                                try
                                                {

                                                    db.tutors.Add(modelToObjectTutor(tutor));
                                                    db.SaveChanges();
                                                    counterEnter++;
                                                    data.Add("Tutor with Registration number Entered" + tutor.registrationNo);

                                                }
                                                catch (Exception exp)
                                                {
                                                    log.Error(exp);
                                                    data.Add(exp.Message + exp.InnerException);
                                                    continue;
                                                }
                                            }
                                            data.Add("Total Entered Tutor" + counterEnter);
                                        }
                                        else
                                        {
                                            data.Add("No Record Entered. All Record Already Exist Or there is no record in uploaded sheet!");
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        log.Error(ex);
                                        data.Add(ex.Message + ex.InnerException);
                                        data.ToArray();
                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                        return json(stuff);
                                    }

                                }
                                #endregion


                                #region notificationSheet
                                else if (notification_sheet == true)
                                {
                                    List<S_Notification> TN = new List<S_Notification>();
                                    try
                                    {
                                        foreach (IXLRow row in workSheet.Rows())
                                        {
                                            //Use the first row to add columns to DataTable.
                                            if (firstRow)
                                            {
                                                foreach (IXLCell cell in row.Cells())
                                                {
                                                    dt.Columns.Add(cell.Value.ToString());
                                                }
                                                #region ColumnCheck
                                                if (dt.Columns[0].ToString() == "sr_no")
                                                {
                                                    if (dt.Columns[1].ToString() == "retention")
                                                    {
                                                        if (dt.Columns[2].ToString() == "assignment_total")
                                                        {
                                                            if (dt.Columns[3].ToString() == "amount_assignment")
                                                            {
                                                                if (dt.Columns[4].ToString() == "meetings_total")
                                                                {
                                                                    if (dt.Columns[5].ToString() == "amount_meeting")
                                                                    {
                                                                        if (dt.Columns[6].ToString() == "postage_total")
                                                                        {
                                                                            if (dt.Columns[7].ToString() == "bM_tutor")
                                                                            {
                                                                                if (dt.Columns[8].ToString() == "group_notification")
                                                                                {
                                                                                    if (dt.Columns[9].ToString() == "course_code")
                                                                                    {
                                                                                        if (dt.Columns[10].ToString() == "notification_SrNo")
                                                                                        {
                                                                                            if (dt.Columns[11].ToString() == "gross_amount")
                                                                                            {
                                                                                                if (dt.Columns[12].ToString() == "incomeTax")
                                                                                                {
                                                                                                    if (dt.Columns[13].ToString() == "net_amount")
                                                                                                    {
                                                                                                        if (dt.Columns[14].ToString() == "diaryNo_date")
                                                                                                        {
                                                                                                            if (dt.Columns[15].ToString() == "no_of_stds")
                                                                                                            {
                                                                                                                if (dt.Columns[16].ToString() == "study_center")
                                                                                                                {
                                                                                                                    if (dt.Columns[17].ToString() == "reference")
                                                                                                                    {
                                                                                                                        if (dt.Columns[18].ToString() == "section")
                                                                                                                        {
                                                                                                                            if (dt.Columns[19].ToString() == "semester")
                                                                                                                            {
                                                                                                                                if (dt.Columns[20].ToString() == "year")
                                                                                                                                {
                                                                                                                                    if (dt.Columns[21].ToString() == "contigency")
                                                                                                                                    {
                                                                                                                                        if (dt.Columns[22].ToString() == "registration_no")
                                                                                                                                        { }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'registration_no' Missing!");
                                                                                                                                            data.ToArray();

                                                                                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                                            return json(stuff);
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'contigency' Missing!");
                                                                                                                                        data.ToArray();

                                                                                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                                        return json(stuff);
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'year' Missing!");
                                                                                                                                    data.ToArray();

                                                                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                                    return json(stuff);
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'semester' Missing!");
                                                                                                                                data.ToArray();

                                                                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                                return json(stuff);
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'section' Missing!");
                                                                                                                            data.ToArray();

                                                                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                            return json(stuff);
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'reference' Missing!");
                                                                                                                        data.ToArray();

                                                                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                        return json(stuff);
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'study_center' Missing!");
                                                                                                                    data.ToArray();

                                                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                    return json(stuff);
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'no_of_stds' Missing!");
                                                                                                                data.ToArray();

                                                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                                return json(stuff);
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'diaryNo_date' Missing!");
                                                                                                            data.ToArray();

                                                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                            return json(stuff);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'net_amount' Missing!");
                                                                                                        data.ToArray();

                                                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                        return json(stuff);
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'incomeTax' Missing!");
                                                                                                    data.ToArray();

                                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                    return json(stuff);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'gross_amount' Missing!");
                                                                                                data.ToArray();

                                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                                return json(stuff);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'notification_SrNo' Missing!");
                                                                                            data.ToArray();

                                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                            return json(stuff);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'gross_amount' Missing!");
                                                                                        data.ToArray();

                                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                        return json(stuff);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'course_code' Missing!");
                                                                                    data.ToArray();

                                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                    return json(stuff);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'group_notification' Missing!");
                                                                                data.ToArray();

                                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                                return json(stuff);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'postage_total' Missing!");
                                                                            data.ToArray();

                                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                                            return json(stuff);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'amount_meeting' Missing!");
                                                                        data.ToArray();

                                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                                        return json(stuff);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'meetings_total' Missing!");
                                                                    data.ToArray();

                                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                                    return json(stuff);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'amount_assignment' Missing!");
                                                                data.ToArray();

                                                                dynamic stuff = JsonConvert.SerializeObject(data);
                                                                return json(stuff);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'assignment_total' Missing!");
                                                            data.ToArray();

                                                            dynamic stuff = JsonConvert.SerializeObject(data);
                                                            return json(stuff);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'retention' Missing!");
                                                        data.ToArray();

                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                        return json(stuff);
                                                    }
                                                }
                                                else
                                                {
                                                    data.Add("Please choose Excel file with Correct Format of Columns. Column Name 'Sr_No' Missing!");
                                                    data.ToArray();
                                                    dynamic stuff = JsonConvert.SerializeObject(data);
                                                    return json(stuff);
                                                }
                                                #endregion columnCheck
                                                firstRow = false;
                                            }
                                            else
                                            {
                                                //Add rows to DataTable.
                                                dt.Rows.Add();
                                                int i = 0;

                                                foreach (IXLCell cell in row.Cells())
                                                {
                                                    if (cell.Value.ToString() != null)
                                                    {
                                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                                        i++;
                                                    }
                                                    else
                                                    {
                                                        data.Add("A cell in row of table contain a null value! Whole Table Must not Contain any Null Value. Please Fill the Cell");
                                                        data.ToArray();

                                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                                        return json(stuff);
                                                    }
                                                }

                                            }
                                            
                                        }
                                        TN = ConvertDataTableNotification(dt);
                                        if (TN.Count != 0)
                                        {
                                            var CounterEntered = 0;
                                            foreach (var exp in TN)
                                            {
                                                try
                                                {
                                                    db.tutor_experience.Add(modelToObjectExp(exp));
                                                    db.SaveChanges();
                                                    CounterEntered++;
                                                    data.Add("");
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.Error(ex);
                                                    data.Add(ex.Message + ex.InnerException);
                                                }
                                            }

                                            data.Add("Total Tutor Experience Entered" + CounterEntered);
                                            data.Add("Entered Successfully!");
                                        }
                                        else
                                        {
                                            data.Add("No Record to Enter All Record Already Exist Or there is no record in uploaded sheet!");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        data.Add(ex.Message + ex.InnerException.Message);
                                        data.ToArray();
                                        dynamic stuff = JsonConvert.SerializeObject(data);
                                        return json(stuff);
                                    }
                                }
                            }
                                #endregion

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            data.Add(ex.Message + ex.InnerException);
                        }
    
                    }

                    else
                    {
                        if (postedFile == null) 
                        data.Add("Please choose Excel file");
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

        private tutor modelToObjectTutor(S_Tutor a)
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
            TU.image_profile = a.image_profile;
            TU.image_updation_form = a.image_updation_form;
            TU.t_status = a.t_status;
            return TU;
        
        }

        private tutor_experience modelToObjectExp(S_Notification a)
        {
            tutor_experience TE = new tutor_experience();

            TE.tutor_id = Convert.ToInt32(a.tutor_id);
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

            return TE;
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


        private List<S_Notification> ConvertDataTableNotification(DataTable tbl)
        {
            List<S_Notification> results = new List<S_Notification>();
            var tutor_id = 0;
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            // iterate over your data table
            foreach (DataRow row in tbl.Rows)
            {
                S_Notification convertedObject = ConvertRowToNotification(row);
                tutor TT = new tutor();
                TT = db.tutors.Where(a => a.reg_no == convertedObject.registration_no).FirstOrDefault();
                if (TT != null)
                {
                    if (convertedObject.registration_no != "" || convertedObject.registration_no != null || convertedObject.course_code != "" || convertedObject.section != "" || convertedObject.course_code != null || convertedObject.semester != null || convertedObject.semester != "")
                    {
                        tutor_id = TT.tutor_id;
                       
                        if (!experienceCheck(tutor_id, convertedObject.course_code, convertedObject.semester, convertedObject.year))
                        { 
                            convertedObject.tutor_id = tutor_id.ToString();
                            results.Add(convertedObject);
                        }
                        else
                        {
                            data.Add("Experience of Reg. No." + convertedObject.registration_no + "already exist!");
                        }
                    }
                    else
                    { 
                         data.Add("Registration no is required  of Serial No." + convertedObject.sr_no);
                         data.Add("Course code is required of Registration No." + convertedObject.registration_no);
                         data.Add("Section is required  of Registration No." + convertedObject.registration_no);
                         data.Add("Semester is required of Registration No." + convertedObject.registration_no);

                    }
                }
                else
                {
                    data.Add("Registration No:"+ convertedObject.registration_no + "does not exist. Please enter record of That Tutor first!");
                }
            }

            return results;
        }

        private S_Notification ConvertRowToNotification(DataRow row)
        {
            S_Notification result = new S_Notification();

            result.sr_no = row[0].ToString();
            result.retention = row[1].ToString();
            result.assignment_total = row[2].ToString();
            result.amount_assignment = row[3].ToString();
            result.meetings_total = row[4].ToString();
            result.amount_meeting = row[5].ToString();
            result.postage_total = row[6].ToString();
            result.bM_tutor = row[7].ToString();
            result.group_notification = row[8].ToString();
            result.course_code = row[9].ToString();
            result.notification_SrNo = row[10].ToString();
            result.gross_amount = row[11].ToString();
            result.incomeTax = row[12].ToString();
            result.net_amount = row[13].ToString();
            result.diaryNo_date = row[14].ToString();
            result.no_of_stds = row[15].ToString();
            result.study_center = row[16].ToString();
            result.reference = row[17].ToString();
            result.section = row[18].ToString();
            result.semester = row[19].ToString();
            result.year = row[20].ToString();
            result.contigency = row[21].ToString();
            result.registration_no = row[22].ToString();

            return result;
        
        }

        private List<S_Tutor> ConvertDataTableTutor(DataTable tbl)
        {
            List<S_Tutor> results = new List<S_Tutor>();
            test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities();
            tutor T_check = new tutor();
            // iterate over your data table
            foreach (DataRow row in tbl.Rows)
            {
                S_Tutor convertedObject = ConvertRowToTutor(row);
                
                T_check = db.tutors.Where(b => b.reg_no == convertedObject.registrationNo).FirstOrDefault();
                if (T_check != null)
                {
                    if (T_check.tutor_id != null || T_check.tutor_id != 0)
                    {
                        continue;
                    }
                }
                else if (convertedObject.name == "" || convertedObject.registrationNo == "" || convertedObject.fatherName == "")
                {
                    data.Add("Registration No is required of Serial no." + convertedObject.srNo);
                    data.Add("Name is required of Registration no." + convertedObject.registrationNo);
                    data.Add("Father name is required of Registration no." + convertedObject.registrationNo);
                }
                else
                {
                    try
                    {
                        var find = results.Find(item => item.registrationNo == convertedObject.registrationNo);
                        if (find != null)
                        {
                            continue;
                        }
                        else
                        results.Add(convertedObject);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        data.Add(ex.Message + ex.InnerException);
                    }
                }
                
            }
           
            return results;
        }

        private S_Tutor ConvertRowToTutor(DataRow row)
        {
            S_Tutor result = new S_Tutor();
             
            // assign the properties of MyObject from the DataRow
            try
            {
                result.srNo = row[0].ToString();
                result.registrationNo = row[1].ToString();
                Regex.Replace(result.registrationNo, @"\s+", "");
                if (row[2].ToString() == "")
                {
                    result.gender = "N/A";
                }
                else
                {
                    result.gender = row[2].ToString();
                }
                result.name = row[3].ToString();
                result.fatherName = row[4].ToString();
                result.designation = row[5].ToString();
                result.bps = row[6].ToString();
                result.officialAddress = row[7].ToString();
                result.newAddress = row[8].ToString();
                result.phoneNo = row[9].ToString();
                result.qualification = row[10].ToString();
                result.tehsil = row[11].ToString();
                result.intelCertificate = row[12].ToString();
                result.update_by = "admin";
                result.update_date = DateTime.Now;
                result.image_profile = "../../";
                result.image_updation_form = "../../";
                result.t_status = "unverified";
                
            }
            catch(Exception ex) 
            {
                log.Error(ex);
                data.Add(ex.Message + ex.InnerException);
            }

            // and so on .... convert the column values in the DataRow into the
            // properties of your object type

            return result;
        }

    }
}