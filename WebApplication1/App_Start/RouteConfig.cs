using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "Default",
              "{controller}/{action}/{id}",
               new { controller = "Home", action = "UserLogin", id = UrlParameter.Optional },
                new [] {"WebApplication1.Controllers"}

                // routes.MapRoute(
                //name: "Default",
                //url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "UserLogin", id = UrlParameter.Optional },
                //new[] { "WebApplication1.Controllers" }
            );
        }
    }
}
