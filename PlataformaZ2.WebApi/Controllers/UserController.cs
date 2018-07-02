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
        /// <returns>Http Result with User Session object</returns>
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login(CredentialsDto credentials)
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, UserManager.Login(credentials)));
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
                log.Fatal("Login: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Sign-up a new user with password
        /// </summary>
        /// <param name="userSignUpDto">User's data</param>
        /// <returns>Http Result with simple message</returns>
        [Route("signUp")]
        [HttpPost]
        public IHttpActionResult SignUpUser(UserSignUpDto userSignUpDto)
        {
            try
            {
                string successMessage = UserManager.SignUpUser(userSignUpDto);

                return this.Ok(new HttpResultModel(successMessage));
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
                log.Fatal("SignUpUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Sends an e-mail to redefine the password
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Http Result with simple message</returns>
        [Route("forgotPassword/{email}")]
        [HttpPost]
        public IHttpActionResult ForgotPassword(string email)
        {
            try
            {
                string successMessage = UserManager.ForgotPassword(email);

                return this.Ok(new HttpResultModel(successMessage));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
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
        /// <returns>Http Result with User object</returns>
        [Route("passwordLinkValidation")]
        [HttpPost]
        public IHttpActionResult ValidatePasswordLink(ChangePasswordDto changeInfo)
        {
            try
            {
                UserFullDto userParcialData = UserManager.ValidatePasswordLink(changeInfo);

                return this.Ok(new HttpResultModel(string.Empty, userParcialData));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
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
        /// <returns>Http Result with simple message</returns>
        [Route("externalPasswordChange")]
        [HttpPost]
        public IHttpActionResult ChangePasswordUsingToken(ChangePasswordDto changeInfo)
        {
            try
            {
                UserManager.ChangePasswordUsingToken(changeInfo);

                return this.Ok(new HttpResultModel("Senha alterada com sucesso"));
            }
            catch (BusinessException e)
            {
                return this.BadRequest(e.Message);
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
        /// <returns>Http Result with the User Session object</returns>
        [Route("userSession/{username}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.LoadMyUserSession })]
        public IHttpActionResult GetUserSessionByUsername(string username)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                UserSessionDto session = UserManager.GetUserSessionByUsername(loggedUser, username);

                return this.Ok(new HttpResultModel(string.Empty, session));
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
                log.Fatal("GetUserSessionByUsername: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Redefines the password of a logged user (does not use token)
        /// </summary>
        /// <param name="changeInfo">Info for password change (token is left blank)</param>
        /// <returns>Http Result with simple message</returns>
        [Route("userArea/internalPasswordChange")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ChangeMyPassword })]
        public IHttpActionResult ChangePasswordForLoggedUser(ChangePasswordDto changeInfo)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                UserManager.ChangePasswordForLoggedUser(loggedUser, changeInfo);

                return this.Ok(new HttpResultModel("Senha alterada com sucesso"));
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
                log.Fatal("ChangePasswordForLoggedUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível alterar a senha");
            }
        }

        /// <summary>
        /// Gets an user data by its identifier (requested by the correspondent user)
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Http Result with the User object</returns>
        [Route("userArea/{id}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditMyUser })]
        public IHttpActionResult GetUserByIdByUser(int id)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                return this.Ok(new HttpResultModel(string.Empty, UserManager.GetUserById(loggedUser, id)));
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
                log.Fatal("GetUserByIdByUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Updates user's data (done by the user)
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Http Result with simple message</returns>
        [Route("userArea/update")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditMyUser })]
        public IHttpActionResult UpdateUserByUser(UserFullDto user)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                UserManager.UpdateUserByUser(loggedUser, user);

                return this.Ok(new HttpResultModel("Dados alterados com sucesso"));
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
        /// <returns>Http Result with the list of users</returns>
        [Route("management/search")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ReadUsers })]
        public IHttpActionResult Search(UserFilterDto filter)
        {
            try
            {
                return this.Ok(new HttpResultModel(string.Empty, UserManager.Search(filter)));
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
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Gets an user by its identifier
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Http Result with the User object</returns>
        [Route("management/{id}")]
        [HttpGet]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.ReadUsers })]
        public IHttpActionResult GetUserById(int id)
        {
            var loggedUser = new UserRepository().GetByAcessToken(ActionContext.Request.Headers.Authorization.Parameter);

            try
            {
                return this.Ok(new HttpResultModel(string.Empty, UserManager.GetUserById(loggedUser, id)));
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
                log.Fatal("GetUserById: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Saves an user (create or update)
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Http Result with simple message</returns>
        [Route("management/save")]
        [HttpPost]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditUser })]
        public IHttpActionResult SaveUser(UserFullDto user)
        {
            try
            {
                string successMessage = UserManager.SaveUser(user);

                return this.Ok(new HttpResultModel(successMessage));
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
                log.Fatal("SaveUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        /// <summary>
        /// Deletes an user (set as inactive)
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <returns>Http Result with simple message</returns>
        [Route("management/{idUser}")]
        [HttpDelete]
        [SimpleAuthenticationAttribute(NeededPermissions = new Permissions[] { Permissions.EditUser })]
        public IHttpActionResult DeleteUser(int idUser)
        {
            try
            {
                UserManager.DeleteUser(idUser);

                return this.Ok(new HttpResultModel("Usuário excluído com sucesso"));
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
                log.Fatal("DeleteUser: " + e.ToString() + " // InnerException: " + e.InnerException?.ToString());
                return this.BadRequest("Não foi possível efetuar a operação");
            }
        }

        #endregion
    }
}
