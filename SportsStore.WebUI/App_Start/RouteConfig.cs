using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //HaKM - url field: display in page url (host/ + url)
            // {page} => will map with same name page parameter in List Action
            // only map when they have the same name (page-page in this case) 
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new {Controller = "Product", action = "List"}
            );
            // Must to push the above router before this 
            // => because the router is called in order 
            //=> when call localhost/page3 for example
            //=> it will call default instead of calling the above router
            //=> not expected behavior => so push it above 
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
