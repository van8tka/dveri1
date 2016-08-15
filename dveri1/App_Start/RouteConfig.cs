﻿using System;
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
             name: "PageSizeAllRoute",
             url: "{controller}/{action}/{brand}/{id}/{sort}/{pagesize}/"

         );

            routes.MapRoute(
               name: "BrandAllRoute",
               url: "{controller}/{action}/{brand}/{id}/{sort}/"

           );

            //маршрут для определения товара и выдачи имени в URL и индекса товара(для сео)
            routes.MapRoute(
                name: "TovArtRoute",
                url: "{controller}/{action}/{tov}/{id}/" 
         );

            routes.MapRoute(
            name: "OtherRoute",
            url: "{controller}/{action}/{id}/"
       );
            routes.MapRoute(
           name: "OtherRoute2",
           url: "{controller}/{action}/{page}/"
      );

            routes.MapRoute(
          name: "BrandRoute",
          url: "{controller}/{action}/{brand}/"
      );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "VhodnyeDveri", action = "VhodnyeDveriIndex"}
            );
            //устанавливаем url в нижний регистр
            routes.LowercaseUrls = true;
            //добавляем слэш в конце
            routes.AppendTrailingSlash = true;

        }
    }
}
