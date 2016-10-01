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


            //routes.MapRoute(
            // name: "PageSizeAllRoute",
            // url: "{controller}/{action}/{page}/{pagesize}/{*catchccall}"
            // );

           //маршрут для определения товара и выдачи имени в URL и индекса товара(для сео)
            routes.MapRoute(
                name: "TovArtRoute",
                url: "{controller}/{action}/{tov}/num{id}/"
         );
       
            routes.MapRoute(
            name: "OtherRoute",
            url: "{controller}/{action}/{id}/"
            
       );
      //      routes.MapRoute(
      //     name: "OtherRoute2",
      //     url: "{controller}/{action}/{page}/"
           
      //);

              routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "VhodnyeDveri", action = "MetallicheskieVhodnyeDveri" }
          );
            //устанавливаем url в нижний регистр
            routes.LowercaseUrls = true;
            //добавляем слэш в конце
            routes.AppendTrailingSlash = true;

        }
    }
}
