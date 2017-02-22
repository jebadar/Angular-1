using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRUDangular_1.Models;

namespace CRUDangular_1.ViewModel
{
    public class LayoutModel
    {
        public string UserFullName
        {
            get
            {
                var user = HttpContext.Current.User.Identity.Name;

                using (test_Applicata_DataBaseEntities db = new test_Applicata_DataBaseEntities())
                {
                    var found = db.users.Where(a => a.user_name.Equals(user)).FirstOrDefault();
                    if (found != null)
                    {
                        user = found.user_name;
                    }
                }
                return user;
            }
        }
    }
}