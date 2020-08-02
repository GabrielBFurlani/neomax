////-----------------------------------------------------------------------
//// <copyright file="FilterConfig.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.WebApi
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Configuration of filter
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers global filters
        /// </summary>
        /// <param name="filters">Filter collection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
