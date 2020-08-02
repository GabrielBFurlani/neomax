////-----------------------------------------------------------------------
//// <copyright file="SolicitationProductDao.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
    public class SolicitationProductDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationProductDao" /> class.
        /// </summary>
        public SolicitationProductDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual SolicitationDao Solicitation { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual string ProductName { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual ProductDao Product { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual SolicitationStatus Status { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual DateTime CreationDate { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual string Title { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual IList<FileDao> ListDocuments { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual string CNPJPayingSource { get; set; }
    }
}
