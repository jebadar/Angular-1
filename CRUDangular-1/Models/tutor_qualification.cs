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
    
    public partial class tutor_qualification
    {
        public int q_id { get; set; }
        public string degree_name { get; set; }
        public string institute_name { get; set; }
        public string date_of_complete { get; set; }
        public string pass_division { get; set; }
        public string is_attested { get; set; }
        public int tutor_id { get; set; }
        public string image_degree { get; set; }
    
        public virtual tutor tutor { get; set; }
    }
}