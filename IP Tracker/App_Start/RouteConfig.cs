using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IP_Tracker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           routes.Clear();

            routes.MapRoute(
                name: "City",
                url: "city/locations",
                defaults: new { controller = "City", action = "Index", city = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Ip",
              url: "ip/location",
              defaults: new { controller = "Ip", action = "Index", ip = UrlParameter.Optional }
            );

             routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
