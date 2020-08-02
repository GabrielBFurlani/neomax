////-----------------------------------------------------------------------
//// <copyright file="ContactTimeDao.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    using Neomax.Model.Util;
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Client Available
    /// </summary>
    public class ContactTimeDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactTimeDao" /> class.
        /// </summary>
        public ContactTimeDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual ClientDao Client { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual ContactTime? ContactTime { get; set; }
    }
}
