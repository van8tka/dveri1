using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dveri2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "PageSizeAllRoute",
              url: "{controller}/{action}/{brand}/page{id}/sort{sort}/{flagMaterial}/num{pagesize}/"

          );
            routes.MapRoute(
           name: "PageSizeAllRoute2",
           url: "{controller}/{action}/{brand}/page{id}/sort{sort}/{flagMaterial}/"

       );
          

            //маршрут для определения товара и выдачи имени в URL и индекса товара(для сео)
            routes.MapRoute(
                name: "TovRoute",
                url: "{controller}/{action}/{tov}/num{id}/{flag}/"
         );

         
            //установим ограничение только для контроллера Articles
            routes.MapRoute(
         name: "ArtRoute",
         url: "{controller}/{action}/{tov}/num{id}/",
         defaults: new {controller= "Articles", action="Getarticle"},
         constraints: new { controller = "^Ar.*" }
         );
            routes.MapRoute(
           name: "PageSizeAllRoute3",
           url: "{controller}/{action}/{brand}/{flagMaterial}/"

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
                defaults: new { controller = "MegkomnatnyeDveri", action = "MegkomnatnyeDveriIndex" }
            );
            //устанавливаем url в нижний регистр
            routes.LowercaseUrls = true;
            //добавляем слэш в конце
            routes.AppendTrailingSlash = true;
        }
    }
}
