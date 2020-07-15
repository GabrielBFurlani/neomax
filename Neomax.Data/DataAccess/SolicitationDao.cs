////-----------------------------------------------------------------------
//// <copyright file="SolicitationDao.cs" company="Zetacorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    using Neomax.Model.Util;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Client Available
    /// </summary>
    public class SolicitationDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationDao" /> class.
        /// </summary>
        public SolicitationDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual int Protocol { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual ClientDao Client { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual SolicitationStatus? Status { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual DateTime CreationDate { get; set; }
    }
}
