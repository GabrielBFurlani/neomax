////-----------------------------------------------------------------------
//// <copyright file="FileDao.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
using System;

namespace Neomax.Data.DataAccess
{
    /// <summary>
    /// Database model representing File
    /// </summary>
    public class FileDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDao" /> class.
        /// </summary>
        public FileDao()
        {            
        }

        /// <summary> Gets or sets the file's name (for the user visualization) </summary>
        public virtual string Name { get; set; }

        /// <summary> Gets or sets the creation date of the file </summary>
        public virtual DateTime CreateDate { get; set; }

        public virtual Byte[] Content { get; set; }

        public virtual string MimeType { get; set; }
    }
}
