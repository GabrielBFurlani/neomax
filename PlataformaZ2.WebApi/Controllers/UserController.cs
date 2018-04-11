////-----------------------------------------------------------------------
//// <copyright file="UserController.cs" company="ZetaCorp">
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
    /// User Controller
    /// </summary>
    [RoutePrefix("api/user")]
    [SessionManagement]
    public class UserController : ApiController
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Non-token operations (user creation, login and password operations)

        /// <summary>
        /// Checks if username and password are valid for login
        /// </summary>
        /// <param name="credentials">User's credentials</param>
        /// <returns>Operation result with the User Session object</returns>
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login(CredentialsDto credentials)
        {
            try
            {
                OperationResult operation = UserManager.Login(credentials);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, string.Empty, operation.Data));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
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
                log.Fatal("Login: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }       
       
        /// <summary>
        /// Sends an e-mail to redefine the password
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Operation result</returns>
        [Route("forgotPassword/{email}")]
        [HttpPost]
        public IHttpActionResult ForgotPassword(string email)
        {
            try
            {
                OperationResult operation = UserManager.ForgotPassword(email);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, operation.Message));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
            }
            catch (BusinessException e)
            {
                return this.Content(System.Net.HttpStatusCode.PreconditionFailed, new HttpResultModel(false, e.Message));
            }
            catch (Exception e)
            {
                log.Fatal("ForgotPassword: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível enviar o e-mail para redefinição de senha");
            }
        }

        /// <summary>
        /// Checks if the link is valid for password change (the user exists and the token is not expired)
        /// </summary>
        /// <param name="changeInfo">Info for password change (password is left blank)</param>
        /// <returns>Operation result</returns>
        [Route("passwordLinkValidation")]
        [HttpPost]
        public IHttpActionResult ValidatePasswordLink(ChangePasswordDto changeInfo)
        {
            try
            {
                OperationResult operation = UserManager.ValidatePasswordLink(changeInfo);

                if (operation.Success)
                {
                    // the link is valid: return success and user's parcial data
                    return this.Ok(new HttpResultModel(true, operation.Message, operation.Data));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
            }
            catch (BusinessException e)
            {
                return this.Content(System.Net.HttpStatusCode.PreconditionFailed, new HttpResultModel(false, e.Message));
            }
            catch (Exception e)
            {
                log.Fatal("ValidatePasswordLink: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi validar o link de alteração de senha");
            }
        }

        /// <summary>
        /// Defines/Redefines the password for a non-logged user (using token)
        /// </summary>
        /// <param name="changeInfo">Info for password change</param>
        /// <returns>Operation result</returns>
        [Route("externalPasswordChange")]
        [HttpPost]
        public IHttpActionResult ChangePasswordUsingToken(ChangePasswordDto changeInfo)
        {
            try
            {
                OperationResult operation = UserManager.ChangePasswordUsingToken(changeInfo);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, "Senha alterada com sucesso"));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
            }
            catch (BusinessException e)
            {
                return this.Content(System.Net.HttpStatusCode.PreconditionFailed, new HttpResultModel(false, e.Message));
            }
            catch (Exception e)
            {
                log.Fatal("ChangePasswordUsingToken: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível alterar a senha");
            }
        }      

        #endregion

        #region User Session and User Area

        /// <summary>
        /// Get an user session by username
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>Operation result with the User Session object</returns>
        [Route("userSession/{username}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.LoadMyUserSession })]
        public IHttpActionResult GetUserSessionByUsername(string username)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);
            
            try
            {
                UserSessionDto session = UserManager.GetUserSessionByUsername(loggedUser, username);

                if (session != null)
                {
                    return this.Ok(new HttpResultModel(true, string.Empty, session));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, "Não foi possível carregar a sessão. Necessário efetuar login novamente."));
                }
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
                log.Fatal("GetUserSessionByUsername: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }   
        }

        /// <summary>
        /// Redefines the password of a logged user (does not use token)
        /// </summary>
        /// <param name="changeInfo">Info for password change (token is left blank)</param>
        /// <returns>Operation result</returns>
        [Route("userArea/internalPasswordChange")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ChangeMyPassword })]
        public IHttpActionResult ChangePasswordForLoggedUser(ChangePasswordDto changeInfo)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                OperationResult operation = UserManager.ChangePasswordForLoggedUser(loggedUser, changeInfo);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, "Senha alterada com sucesso"));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
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
                log.Fatal("ChangePasswordForLoggedUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível alterar a senha");
            }
        }

        /// <summary>
        /// Gets an user data by its identifier (requested by the correspondent user)
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Operation result with the User object</returns>
        [Route("userArea/{id}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditMyUser })]
        public IHttpActionResult GetUserByIdByUser(int id)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                UserFullDto user = UserManager.GetUserById(loggedUser, id);

                if (user != null)
                {
                    return this.Ok(new HttpResultModel(true, string.Empty, user));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, "O usuário não existe"));
                }
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
                log.Fatal("GetUserByIdByUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Updates user's data (done by the user)
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Operation result</returns>
        [Route("userArea/update")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditMyUser })]
        public IHttpActionResult UpdateUserByUser(UserFullDto user)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                OperationResult operation = UserManager.UpdateUserByUser(loggedUser, user);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, "Dados alterados com sucesso"));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
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
                log.Fatal("UpdateUserByUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }       

        #endregion

        #region User Management

        /// <summary>
        /// Searches users refined by filter and pagination
        /// </summary>
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns>Operation result with the list of users</returns>
        [Route("management/search")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ReadUsers })]
        public IHttpActionResult Search(UserFilterDto filter)
        {
            try
            {
                return this.Ok(new HttpResultModel(true, string.Empty, UserManager.Search(filter)));
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
                log.Fatal("Search: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }            
        }

        /// <summary>
        /// Gets an user by its identifier
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Operation result with the User object</returns>
        [Route("management/{id}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ReadUsers })]
        public IHttpActionResult GetUserById(int id)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                UserFullDto user = UserManager.GetUserById(loggedUser, id);

                if (user != null)
                {
                    return this.Ok(new HttpResultModel(true, string.Empty, user));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, "O usuário não existe"));
                }                
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
                log.Fatal("GetUserById: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Saves an user (create or update)
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Operation result</returns>
        [Route("management/save")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditUser })]
        public IHttpActionResult SaveUser(UserFullDto user)
        {
            try
            {
                OperationResult operation = UserManager.SaveUser(user);

                if (operation.Success)
                {
                    if (string.IsNullOrEmpty(operation.Message))
                    {
                        return this.Ok(new HttpResultModel(true, "Usuário salvo com sucesso"));
                    }
                    else
                    {
                        return this.Ok(new HttpResultModel(true, operation.Message));
                    }                    
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }
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
                log.Fatal("SaveUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Deletes an user (set as inactive)
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <returns>Operation result</returns>
        [Route("management/{idUser}")]
        [HttpDelete]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditUser })]
        public IHttpActionResult DeleteUser(int idUser)
        {
            try
            {
                OperationResult operation = UserManager.DeleteUser(idUser);

                if (operation.Success)
                {
                    return this.Ok(new HttpResultModel(true, "Usuário excluído com sucesso"));
                }
                else
                {
                    return this.Ok(new HttpResultModel(false, operation.Message));
                }                    
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
                log.Fatal("DeleteUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        #endregion
    }
}
