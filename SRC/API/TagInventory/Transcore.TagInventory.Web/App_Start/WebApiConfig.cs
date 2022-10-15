using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Transcore.TagInventory.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
             //config.EnableCors();           

            var cors = new EnableCorsAttribute("*", "*", "POST,PUT,GET,OPTIONS");
            config.EnableCors(cors);


            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
