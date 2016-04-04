using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace deals.earlymoments.com
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");         

            routes.MapRoute(
                name: "seuss",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Seuss", action = "four_for_1_spring", id = UrlParameter.Optional }
            );
        }
    }
}
