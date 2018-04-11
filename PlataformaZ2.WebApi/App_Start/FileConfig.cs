////-----------------------------------------------------------------------
//// <copyright file="FileConfig.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Http;
    using PlataformaZ2.Data.Util;

    /// <summary>
    /// File configuration
    /// </summary>
    public static class FileConfig
    {
        /// <summary>
        /// Initializes the file directory on server start
        /// </summary>
        public static void InitializeDirectory()
        {
            ////Gets the complete file path
            string directoryPath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath);

            ////Creates the directory (if it already does not exist)
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
