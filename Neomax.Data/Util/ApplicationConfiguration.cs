////-----------------------------------------------------------------------
//// <copyright file="ApplicationConfiguration.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Util
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Gets configuration settings from web.config
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary> Gets Cors Origins allowed from config file </summary>
        public static string CorsOrigins
        {
            get
            {
                return ConfigurationManager.AppSettings["CorsOrigins"];
            }
        }

        /// <summary> Gets database connection configuration </summary>
        public static ConnectionStringSettings DatabaseConfiguration
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DbConnectionString"];
            }
        }

        /// <summary> Gets the quantity results per page </summary>
        public static byte ResultsPerPage
        {
            get
            {
                return Convert.ToByte(ConfigurationManager.AppSettings["ResultsPerPage"]);
            }
        }

        /// <summary> Gets the AccessToken's expiration period (in hours) </summary>
        public static int AccessTokenExpirationPeriod
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["AccessTokenExpirationPeriod"]);
            }
        }

        /// <summary> Gets the SMTP host address </summary>
        public static string SmtpHostAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpHostAddress"];
            }
        }

        /// <summary> Gets the SMTP host port </summary>
        public static int SmtpHostPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SmtpHostPort"]);
            }
        }

        /// <summary> Gets the SMTP host address </summary>
        public static bool SmtpHostSsl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpHostSsl"]);
            }
        }

        /// <summary> Gets the SMTP username  </summary>
        public static string SmtpUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpUsername"];
            }
        }

        /// <summary> Gets the SMTP password </summary>
        public static string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }

        /// <summary> Gets the SMTP sender's e-mail  </summary>
        public static string SmtpSenderEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpSenderEmail"];
            }
        }

        /// <summary> Gets the SMTP sender's name  </summary>
        public static string SmtpSenderName
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpSenderName"];
            }
        }

        /// <summary> Gets the EmailTemplate's directory path </summary>
        public static string EmailTemplateDirectoryPath
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailTemplateDirectoryPath"];
            }
        }

        /// <summary> Gets the file's directory path </summary>
        public static string FileDirectoryPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FileDirectoryPath"];
            }
        }

        /// <summary> Gets the "external password change" base link </summary>
        public static string ExternalPasswordChangeBaseLink
        {
            get
            {
                return ConfigurationManager.AppSettings["ExternalPasswordChangeBaseLink"];
            }
        }

        /// <summary> Gets the logo image link </summary>
        public static string LogoImageLink
        {
            get
            {
                return ConfigurationManager.AppSettings["LogoImageLink"];
            }
        }

        /// <summary> Gets the logo image link </summary>
        public static string NewUserBaseLink
        {
            get
            {
                return ConfigurationManager.AppSettings["NewUserBaseLink"];
            }
        }
    }
}
