////-----------------------------------------------------------------------
//// <copyright file="ContactDayDao.cs" company="Zetacorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    using Neomax.Model.Util;
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Client Available
    /// </summary>
    public class ContactDayDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDao" /> class.
        /// </summary>
        public ContactDayDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual ClientDao Client { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual ContactDay? ContactDay { get; set; }
    }
}
