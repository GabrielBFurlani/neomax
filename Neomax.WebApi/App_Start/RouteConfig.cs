////-----------------------------------------------------------------------
//// <copyright file="RouteConfig.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.WebApi
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Configuration of routing
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers routes
        /// </summary>
        /// <param name="routes">Route collection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
