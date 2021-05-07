using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication_MVC.App_Start
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add( new HandleErrorAttribute());
            filters.Add( new AuthorizeAttribute());
            filters.Add( new RequireHttpsAttribute());  //application only availabe on hhtps channel
        }
    }
}