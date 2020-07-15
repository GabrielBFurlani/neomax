////-----------------------------------------------------------------------
//// <copyright file="FilterConfig.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
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
