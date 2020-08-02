////-----------------------------------------------------------------------
//// <copyright file="HibernateHelper.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Neomax.Data.DataAccess;
    using Neomax.Data.Util;
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
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(ApplicationConfiguration.DatabaseConfiguration.ConnectionString))
                .Mappings(m => m
                .FluentMappings.AddFromAssemblyOf<UserDao>())
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "web")
                                                .SetProperty(Environment.ShowSql, "true"))
                .BuildSessionFactory();

            SessionFactory = sessionFactory;
        }
    }
}