//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUDangular_1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string u_password { get; set; }
        public int user_role { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string profile_pic { get; set; }
        public Nullable<int> section_id { get; set; }
        public string program { get; set; }
    
        public virtual section_region section_region { get; set; }
        public virtual user_roles user_roles { get; set; }
    }
}
