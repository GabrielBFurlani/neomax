////-----------------------------------------------------------------------
//// <copyright file="WebApiConfig.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using Newtonsoft.Json.Serialization;

    using System.Web.Http.Cors;
    using PlataformaZ2.Data.Util;

    /// <summary>
    /// API configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers http configuration
        /// </summary>
        /// <param name="config">Http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            //// Switch from PascalCase to CamelCase
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //// Enable CORS (Cross-Origin Resource Sharing) - lets the Portal access the WebApi

            // create the CORS object and define origins addresses
            var cors = new EnableCorsAttribute(ApplicationConfiguration.CorsOrigins, "*", "*");

            // allow "Content-Disposition" header (used to send seggested file name at HTTP Download)
            cors.ExposedHeaders.Add("Content-Disposition");
            
            // enable the CORS
            config.EnableCors(cors);

            //// Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
