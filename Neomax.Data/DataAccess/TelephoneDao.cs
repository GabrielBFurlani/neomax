////-----------------------------------------------------------------------
//// <copyright file="Telephone.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------

using Neomax.Data.DataAccess;
using Neomax.Model.Util;

namespace Neomax.Data.DataAccess
{
    /// <summary>
    /// Data model for Telephone
    /// </summary>
    public class TelephoneDao : BaseDao
    {

        /// <summary> Gets or sets the telephone number </summary>
        public virtual string Number { get; set; }

        /// <summary> Gets or sets the telephone type </summary>
        public virtual TelephoneType TelephoneType { get; set; }

        /// <summary> Gets or sets the telephone contact name </summary>
        public virtual string ContactName { get; set; }
    }
}