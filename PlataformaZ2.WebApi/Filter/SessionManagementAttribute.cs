////-----------------------------------------------------------------------
//// <copyright file="SessionManagementAttribute.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.WebApi.Filter
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Data;
    using NHibernate;
    using NHibernate.Context;
    using System;

    /// <summary>
    /// Attribute to session management
    /// </summary>
    public class SessionManagementAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManagementAttribute" /> class.
        /// </summary>
        public SessionManagementAttribute()
        {
            this.SessionFactory = HibernateHelper.SessionFactory;
        }

        /// <summary> Gets or sets session factory </summary>
        private ISessionFactory SessionFactory { get; set; }

        /// <summary>
        /// Open session, binds and open transaction before action execution
        /// </summary>
        /// <param name="actionContext">Action context</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var session = this.SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            session.BeginTransaction();
        }

        /// <summary>
        /// Gets open session, close it and commit transaction after action executed
        /// </summary>
        /// <param name="actionExecutedContext">Action context</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var session = this.SessionFactory.GetCurrentSession();
                var transaction = session.Transaction;

                if (transaction != null && transaction.IsActive)
                {
                    transaction.Commit();
                }

                session = CurrentSessionContext.Unbind(this.SessionFactory);
                session.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); //couldn't log using Log4Net
            }            
        }
    }
}
