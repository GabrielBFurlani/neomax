////-----------------------------------------------------------------------
//// <copyright file="ProductDao.cs" company="Zetacorp">
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
    public class ProductDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDao" /> class.
        /// </summary>
        public ProductDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Name { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Description { get; set; }

        /// <summary> Gets or sets a value indicating whether user is active (when deleted, it's kept on the table, but the status is false) </summary>
        public virtual bool Active { get; set; }
    }
}
