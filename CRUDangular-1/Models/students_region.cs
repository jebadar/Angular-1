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
    
    public partial class students_region
    {
        public students_region()
        {
            this.students_appoint = new HashSet<students_appoint>();
        }
    
        public int std_id { get; set; }
        public string roll_no { get; set; }
        public string reg_no { get; set; }
        public string sex { get; set; }
        public string name { get; set; }
        public string father_name { get; set; }
        public string mailing { get; set; }
        public string tehsil { get; set; }
        public string district { get; set; }
        public string region { get; set; }
        public string semester { get; set; }
        public string year_ { get; set; }
        public string program { get; set; }
        public string cc_1 { get; set; }
        public string cc_2 { get; set; }
        public string cc_3 { get; set; }
        public string cc_4 { get; set; }
        public string cc_5 { get; set; }
        public string cc_6 { get; set; }
        public string updated_date { get; set; }
        public string updated_by { get; set; }
        public string is_deleted { get; set; }
    
        public virtual ICollection<students_appoint> students_appoint { get; set; }
    }
}