using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDangular_1.Models
{
    public class checkUser
    {
        public string user_name { get; set; }
        public string u_password { get; set; }
        public user_roles user_roles { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string profile_pic { get; set; }
        public int section_id { get; set; }
        public semester_current semester_current { get; set; }
        public string program { get; set; }

    }
}