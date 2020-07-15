////-----------------------------------------------------------------------
//// <copyright file="ClientAvailableDao.cs" company="Zetacorp">
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
    public class UserAvailableDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDao" /> class.
        /// </summary>
        public UserAvailableDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Email { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual string Token { get; set; }
    }
}
