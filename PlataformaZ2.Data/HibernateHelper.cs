////-----------------------------------------------------------------------
//// <copyright file="HibernateHelper.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using PlataformaZ2.Data.DataAccess;
    using PlataformaZ2.Data.Util;
    using NHibernate;
    using NHibernate.Cfg;

    /// <summary>
    /// NHibernate helper class
    /// </summary>
    public class HibernateHelper
    {
        /// <summary> Gets NHibernate session factory </summary>
        public static ISessionFactory SessionFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Public method to initialize session factory
        /// </summary>
        public static void StartSessionFactory()
        {
            InitializeSessionFactory();
        }

        /// <summary>
        /// Initializes configuring session factory
        /// </summary>
        private static void InitializeSessionFactory()
        {
            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(ApplicationConfiguration.DatabaseConfiguration.ConnectionString))
                .Mappings(m => m
                .FluentMappings.AddFromAssemblyOf<UserDao>())
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "web")
                                                .SetProperty(Environment.ShowSql, "true"))
                .BuildSessionFactory();
        }
    }
}
