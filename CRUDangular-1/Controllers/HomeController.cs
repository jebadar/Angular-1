using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDangular_1.Models;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;

namespace CRUDangular_1.Controllers
{
   
    public class HomeController : Controller
    {
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user user)
        {
            using (test_Applicata_DataBaseEntities1 db = new test_Applicata_DataBaseEntities1())
            {
                if(ModelState.IsValid)
                {

                    if (string.IsNullOrEmpty(user.user_name) || string.IsNullOrEmpty(user.u_password))
                    {
                        ModelState.AddModelError("Invalid", "Please Enter Valid Email or password!");
                        return View();
                    }
                    else if (user.user_name != null || user.u_password != null)
                    {
                        var v = db.users.Where(a => a.user_name.Equals(user.user_name) && a.u_password.Equals(user.u_password)).FirstOrDefault();
                        if (v == null)
                        {
                            ModelState.AddModelError("Invalid", "Please Enter Valid Email or password!");
                            return View();
                        }
                        else if(v.user_role == 2)
                        {
                            FormsAuthentication.SetAuthCookie(user.user_name, false);
                            return RedirectToAction("Index", "Home");
                        }
                        
                        else if(v != null)
                        {
                            FormsAuthentication.SetAuthCookie(user.user_name, false);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    
                }
            }
            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(user user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        }

        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}