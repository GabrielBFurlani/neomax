////-----------------------------------------------------------------------
//// <copyright file="BankDao.cs" company="Gabriel Furlani">
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
    public class BankDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankDao" /> class.
        /// </summary>
        public BankDao()
        {
        }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Bank { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Agency { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual string Account { get; set; }
    }
}


