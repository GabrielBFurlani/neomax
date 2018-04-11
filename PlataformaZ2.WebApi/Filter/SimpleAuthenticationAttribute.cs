////-----------------------------------------------------------------------
//// <copyright file="SimpleAuthenticationAttribute.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.WebApi.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Http;
    using PlataformaZ2.Model.Util;
    using PlataformaZ2.Business;

    /// <summary>
    /// Attribute to Authentication
    /// </summary>
    public class SimpleAuthenticationAttribute : System.Web.Http.AuthorizeAttribute
    {
        /// <summary> Gets or sets the permissions allowed</summary>
        public Permissions[] NeededPermissions { get; set; }

        /// <summary>
        /// On Authorization action execution
        /// </summary>
        /// <param name="actionContext">Action context</param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            ////Get the access token from request header ('Authorization': 'AccessToken xxxxxxxxxx')
            string accessTokenFromRequest = string.Empty;
            
            if (actionContext.Request.Headers.Authorization != null)
            {
                //get the parameter (the "xxxxxxxxx" part at the 'AccessToken xxxxxxxxxx')
                accessTokenFromRequest = actionContext.Request.Headers.Authorization.Parameter;
            }

            ////Try to authenticate the user (check token and permissions)
            try
            {
                //binding a NHibernate session to the context
                new SessionManagementAttribute().OnActionExecuting(actionContext);

                if (!UserManager.AuthenticateByAcessToken(accessTokenFromRequest, NeededPermissions))
                {
                    //call base method to return 401 Http Status
                    base.HandleUnauthorizedRequest(actionContext);
                }
            }
            catch (Exception)
            {
                //call base method to return 401 Http Status
                base.HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}