using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDangular_1.Models
{
    public class S_Tutor
    {
        public string srNo { get; set; }
        public string registrationNo { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string fatherName { get; set; }
        public string designation { get; set; }
        public string bps { get; set; }
        public string officialAddress { get; set; }
        public string newAddress { get; set; }
        public string phoneNo { get; set; }
        public string qualification { get; set; }
        public string tehsil { get; set; }
        public string intelCertificate { get; set; }
        public string image_profile { get; set; }
        public string image_updation_form { get; set; }
        public string t_status { get; set; }
        public DateTime update_date { get; set; }
        public string update_by { get; set; }
    }
}