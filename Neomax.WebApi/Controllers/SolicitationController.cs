////-----------------------------------------------------------------------
//// <copyright file="SolicitationController.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
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
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http.Description;
    using System.Web.Http.Cors;

    /// <summary>
    /// Profile Controller
    /// </summary>
    [RoutePrefix("api/solicitations")]
    [SessionManagement]
    public class SolicitationController : ApiController
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get solicitation by id
        /// </summary>
        /// <returns>Http result with the solicitation </returns>
        [Route("{id}")]
        [HttpGet]
        [SimpleAuthentication]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var response = SolicitationManager.GetById(id);

                return Ok(response);
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("GetById: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter o solicitações");
            }
        }

        /// <summary>
        /// Create new solicitation
        /// </summary>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create(SolicitationInputDto solicitationInputDto)
        {
            try
            {
                var user = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

                solicitationInputDto.IdClient = user.Client.Id;

                var message = SolicitationManager.Create(solicitationInputDto);

                return Ok(new HttpResultModel(message));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("Create: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível criar um novo solicitatione");
            }
        }

        /// <summary>
        /// Update solicitation
        /// </summary>
        [Route("{id}")]
        [HttpPut]
        [SimpleAuthentication]
        public IHttpActionResult Update(int id, SolicitationInputDto solicitationInputDto)
        {
            try
            {
                var message = SolicitationManager.Update(id, solicitationInputDto);

                return this.Ok(new HttpResultModel(message));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("GetAllProfiles: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível atualizar o solicitatione");
            }
        }

        /// <summary>
        /// Update solicitation
        /// </summary>
        [Route("{id}/updateProductStatus")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult UpdateProductStatus (int id, UpdateProductStatusInputDto updateProductStatusInputModel)
        {
            try
            {
                var message = SolicitationManager.UpdateProductStatus(id, updateProductStatusInputModel);

                return this.Ok(new HttpResultModel(message));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("GetAllProfiles: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível atualizar o solicitatione");
            }
        }

        /// <summary>
        /// Search by filter
        /// </summary>
        /// <returns>Http result with the paginated list of solicitations</returns>
        [Route("admin/search")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult AdminSearch(SolicitationFilterDto filter)
        {
            try
            {
                var isAdmin = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter).Client != null ? false : true;

                if (!isAdmin)
                {
                    return this.BadRequest("Busca somente para administradores");
                }

                return this.Ok(new HttpResultModel(string.Empty, SolicitationManager.AdminSearch(filter)));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("Search: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter a lista paginada de solicitações");
            }
        }

        /// <summary>
        /// Search by filter
        /// </summary>
        /// <returns>Http result with the paginated list of solicitations</returns>
        [Route("client/search")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult ClientSearch(SolicitationFilterDto filter)
        {
            try
            {
                filter.IdClient = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter).Client?.Id;

                if (filter.IdClient == null)
                {
                    return this.Unauthorized();
                }

                return this.Ok(new HttpResultModel(string.Empty, SolicitationManager.ClientSearch(filter)));
            }
            catch (PermissionException)
            {
                return this.Unauthorized();
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("Search: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter a lista paginada de solicitações");
            }
        }

        #region enumerators

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("solicitationStatus")]
        [HttpGet]
        public IHttpActionResult GetSolicitationStatus()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(SolicitationStatus)).Cast<SolicitationStatus>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                log.Fatal("Search: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível obter a lista paginada de solicitações");
            }
        }

        #endregion
    }
}
