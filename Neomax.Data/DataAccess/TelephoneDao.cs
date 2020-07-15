////-----------------------------------------------------------------------
//// <copyright file="Telephone.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
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
        public virtual int Number { get; set; }

        /// <summary> Gets or sets the telephone type </summary>
        public virtual TelephoneType TelephoneType { get; set; }

        /// <summary> Gets or sets the telephone contact name </summary>
        public virtual string ContactName { get; set; }
    }
}