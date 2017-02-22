using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

namespace CRUDangular_1.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional});
        /*, id = RouteParameter.Optional */
            configuration.Routes.MapHttpRoute(
              name: "ActionApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { action = "get", id = RouteParameter.Optional }
            );

           //configuration.Routes.MapHttpRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
           // configuration.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
           // configuration.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
           // configuration.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
        }
    }
}