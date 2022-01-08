////-----------------------------------------------------------------------
//// <copyright file="ProfileController.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.WebApi.Controllers
{
    using System;
    using System.Web.Http;
    using Neomax.Business;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using Neomax.Model.Util;
    using Neomax.WebApi.Filter;
    using Neomax.Data.Repository;

    /// <summary>
    /// Profile Controller
    /// </summary>
    [RoutePrefix("api/profile")]
    [SessionManagement]
    public class ProfileController : ApiController
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns>Http result with the list of profiles</returns>
        [Route("all")]
        [HttpGet]

        public IHttpActionResult GetAllProfiles()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, ProfileManager.GetAllProfiles()));
            }
            //catch (PermissionException)
            //{
            //    return this.Unauthorized();
            //}
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("GetAllProfiles: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter a lista de perfil");
            }
        }
    }
}
