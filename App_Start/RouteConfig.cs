using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Auth", action = "Login", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //name: "CatchAll",
            //url: "{controller}/{action}/{id}",
            //url:= "{*any}", _
            //defaults: new { controller = "auth", action = "Error", id = UrlParameter.Optional }
            //    );
        }
    }
}
