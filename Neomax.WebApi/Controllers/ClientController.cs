////-----------------------------------------------------------------------
//// <copyright file="ClientController.cs" company="ZetaCorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
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
    [RoutePrefix("api/clientes")]
    [SessionManagement]
    public class ClientController : ApiController
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///// <summary>
        ///// Get client by id
        ///// </summary>
        ///// <returns>Http result with the client </returns>
        //[Route("{id}")]
        //[HttpGet]
        //[SimpleAuthentication]
        //public IHttpActionResult GetById(int id)
        //{
        //    try
        //    {
        //        var response = ClientManager.GetByIdUser(id);

        //        return Ok(response);
        //    }
        //    catch (PermissionException)
        //    {
        //        return this.Unauthorized();
        //    }
        //    catch (BusinessException e)
        //    {
        //        return this.BadRequest(e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Fatal("GetById: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
        //        return this.BadRequest("Não foi possível obter o cliente");
        //    }
        //}

        /// <summary>
        /// Disable client by id
        /// </summary>
        /// <returns>Http result with the client </returns>
        [Route("{id}")]
        [HttpDelete]
        [SimpleAuthentication]
        public IHttpActionResult DeleteClientById(int id)
        {
            try
            {
                ClientManager.Disable(id);

                return Ok(new HttpResultModel("Desativado com Sucesso"));
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
                log.Fatal("DeleteClientById: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível desativar o cliente");
            }
        }

        /// <summary>
        /// Reactivate by id
        /// </summary>
        /// <returns>Http result with the client </returns>
        [Route("reativar/{id}")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult Reactivate(int id)
        {
            try
            {
                ClientManager.Reactivate(id);

                return Ok(new HttpResultModel("Reativado com Sucesso"));
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
                log.Fatal("Reactivate: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível reativar o cliente");
            }
        }

        /// <summary>
        /// Create new client
        /// </summary>
        [Route("")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult Create(ClientInputDto clientInputDto)
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, ClientManager.CreateOrUpdate(null, clientInputDto)));
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
                return this.BadRequest("Não foi possível criar um novo cliente");
            }
        }

        /// <summary>
        /// Update client
        /// </summary>
        [Route("{id}")]
        [HttpPut]
        [SimpleAuthentication]
        public IHttpActionResult Update(int id, ClientInputDto clientInputDto)
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, ClientManager.CreateOrUpdate(id, clientInputDto)));
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
                return this.BadRequest("Não foi possível atualizar o cliente");
            }
        }

        /// <summary>
        /// Search by filter
        /// </summary>
        /// <returns>Http result with the paginated list of clients</returns>
        [Route("busca")]
        [HttpPost]
        [SimpleAuthentication]
        public IHttpActionResult Search(ClientFilterDto filter)
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, ClientManager.Search(filter)));
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
                return this.BadRequest("Não foi possível obter a lista paginada de clientes");
            }
        }
    }
}
