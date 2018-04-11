////-----------------------------------------------------------------------
//// <copyright file="ProfileController.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.WebApi.Controllers
{
    using System;
    using System.Web.Http;
    using PlataformaZ2.Business;
    using PlataformaZ2.Model.Dto;
    using PlataformaZ2.Model.Exception;
    using PlataformaZ2.Model.Util;
    using PlataformaZ2.WebApi.Filter;
    using PlataformaZ2.Data.Repository;

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
        /// <returns>Operation result with the list of profiles</returns>
        [Route("all")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ReadProfiles })]
        public IHttpActionResult GetAllProfiles()
        {
            try
            {
                return this.Ok(new HttpResultModel(true, string.Empty, ProfileManager.GetAllProfiles()));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.Content(System.Net.HttpStatusCode.PreconditionFailed, new HttpResultModel(false, e.Message));
            }
            catch (Exception e)
            {
                log.Fatal("GetAllProfiles: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter a lista de perfil");
            }
        }
    }
}
