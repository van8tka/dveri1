using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dveri1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "VhodnyeDveri", action = "VhodnyeDveriIndex", id = UrlParameter.Optional }
            );
            //устанавливаем url в нижний регистр
            routes.LowercaseUrls = true;
            //добавляем слэш в конце
            routes.AppendTrailingSlash = true;

        }
    }
}
