using CRUDangular_1.Models;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CRUDangular_1.Controllers
{
    public class UploadController : ApiController
    {
        [Route("user/PostUserImage")]
        [Authorize]
        public String PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    var postedFile = httpRequest.Files[file];
                    bool profile_img = false;
                    bool updation_img = false;
                    if (postedFile.FileName.Contains("*profile*"))
                    {
                        profile_img = true;
                    }
                    if (postedFile.FileName.Contains("*updation*"))
                    {
                        updation_img = true;
                    }

                    var filePath = "";
                    var imgAdd = "";
                    var UniqueFileName = Guid.NewGuid();
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            return message;
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var versions = new Dictionary<string, string>(); ;
                            versions.Add("_small", "maxwidth=600&maxheight=600");
                            if (profile_img)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Profile/" + UniqueFileName + extension);
                            }
                            else if (updation_img)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Updation_Form/" + UniqueFileName + extension);
                            }
                            else
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Degree/" + UniqueFileName + extension);
                            }
                            //postedFile.FileName
                            //Generate each version
                            foreach (var suffix in versions.Keys)
                            {
                                postedFile.InputStream.Seek(0, SeekOrigin.Begin);

                                //Let the image builder add the correct extension based on the output file type
                                ImageBuilder.Current.Build(
                                    new ImageJob(
                                        postedFile.InputStream,
                                        filePath,
                                        new Instructions(versions[suffix]),
                                        false,
                                        true));
                            }

                            var message1 = string.Format("Image Updated Successfully.");
                            if (profile_img)
                            {
                                imgAdd = "../../Images/Tutor_Profile/" + UniqueFileName + extension;

                            }
                            else if (updation_img)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Updation_Form/" + UniqueFileName + extension);
                            }
                            else
                            {
                                imgAdd = "../../Images/Tutor_Degree/" + UniqueFileName + extension;
                            }
                            //postedFile.FileName 
                            return imgAdd;
                        }
                        else
                        {
                            if (profile_img)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Profile/" + UniqueFileName + extension);
                                imgAdd = "../../Images/Tutor_Profile/" + UniqueFileName + extension;
                            }
                            else if (updation_img)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Updation_Form/" + UniqueFileName + extension);
                                imgAdd = "../../Images/Tutor_Updation_Form/" + UniqueFileName + extension;
                            }
                            else
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Images/Tutor_Degree/" + UniqueFileName + extension);
                                imgAdd = "../../Images/Tutor_Degree/" + UniqueFileName + extension;
                            }
                            //postedFile.FileName
                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message2 = string.Format("Image Updated Successfully.");
                    return imgAdd;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return res;
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return res;
            }
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            //var address = new List<int>();
            int[] address = new int[] {};
            bool img_profile = false;
            bool img_updation = false;
            address = digitArr(id);
            int img_id = 0;
            for (int counter = 0; counter <= 1; counter++)
            {
               
                                if (address[counter] == 1)
                                {
                                    if (address[counter + 1] == 0)
                                    {
                                        for (int i = counter + 2; i < address.Length; i++)
                                        {
                                            img_profile = true;
                                            img_id += address[i] * Convert.ToInt32(Math.Pow(10, address.Length - i - 1));
                                        }
                                    }
                                    else if (address[counter + 1] == 1)
                                    {
                                        for (int i = counter + 2; i < address.Length; i++)
                                        {
                                            img_updation = true;
                                            img_id += address[i] * Convert.ToInt32(Math.Pow(10, address.Length - i - 1));
                                        }
                                    }
                                }
                
                else 
                    break;
            
            }
            if (!img_profile && !img_updation)
            {
                using (test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1())
                {
                    var httpRequest = HttpContext.Current.Request;
                    var col = db.tutor_qualification.Where(w => w.q_id.Equals(id));
                    foreach (var item in col)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("../" + item.image_degree);
                        //System.IO.File.Delete(filePath);
                        item.image_degree = "../../";
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //Handle ex
                    }
                }
            }
            else if (img_profile)
            {
                using (test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1())
                {
                    var httpRequest = HttpContext.Current.Request;
                    var col = db.tutors.Where(w => w.tutor_id.Equals(img_id));
                    foreach (var item in col)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("../" + item.image_profile);
                        //System.IO.File.Delete(filePath);
                        item.image_profile = "../../";
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //Handle ex
                    }
                }
            }
            else if (img_updation)
            {
                using (test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1())
                {
                    var httpRequest = HttpContext.Current.Request;
                    var col = db.tutors.Where(w => w.tutor_id.Equals(img_id));
                    foreach (var item in col)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("../" + item.image_updation_form);
                        //System.IO.File.Delete(filePath);
                        item.image_updation_form = "../../";
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //Handle ex
                    }
                }
            }
        }


        public int[] digitArr(int n)
        {
            if (n == 0) return new int[1] { 0 };

            var digits = new List<int>();

            for (; n != 0; n /= 10)
                digits.Add(n % 10);

            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }

    }
}