////-----------------------------------------------------------------------
//// <copyright file="ClientController.cs" company="Gabriel Furlani">
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
    [RoutePrefix("api/clients")]
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
        public IHttpActionResult Create(ClientInputDto clientInputDto)
        {
            try
            {
                var message = ClientManager.CreateOrUpdate(null, clientInputDto);

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

        #region enumerators

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("telephoneTypes")]
        [HttpGet]
        public IHttpActionResult GetTelephoneTypes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(TelephoneType)).Cast<TelephoneType>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("genderTypes")]
        [HttpGet]
        public IHttpActionResult GetGenderTypes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("noteTypes")]
        [HttpGet]
        public IHttpActionResult GetNoteTypes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(TypeNoteEmited)).Cast<TypeNoteEmited>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("annualBillingTypes")]
        [HttpGet]
        public IHttpActionResult GetAnnualBillingTypes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(AnnualBilling)).Cast<AnnualBilling>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("companyNatureTypes")]
        [HttpGet]
        public IHttpActionResult GetCompanyNatureTypes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(CompanyNatureTypes)).Cast<CompanyNatureTypes>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("days")]
        [HttpGet]
        public IHttpActionResult GetWeekDays()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(ContactDay)).Cast<ContactDay>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

        /// <summary>
        /// Get all activities types
        /// </summary>
        /// <returns>Operation result with List of all activities types (enum)</returns>
        [Route("times")]
        [HttpGet]
        public IHttpActionResult GetContactTimes()
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, Enum.GetValues(typeof(ContactTime)).Cast<ContactTime>().Select(x => new ParameterDto() { Parameter = (int)x, Name = Domain.TextValueFrom(x) }).OrderBy(x => x.Parameter)));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }

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
                return this.BadRequest("COULD_NOT_PERFORM_OPERATION");
            }
        }
        

        #endregion
    }
}
