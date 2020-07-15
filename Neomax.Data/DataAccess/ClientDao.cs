////-----------------------------------------------------------------------
//// <copyright file="ClientDao.cs" company="Zetacorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    using Neomax.Model.Util;
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Client
    /// </summary>
    public class ClientDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDao" /> class.
        /// </summary>
        public ClientDao()
        {
        }


        /// <summary> Gets or sets the client CNPJ </summary>
        public virtual string CNPJPayingSource { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public virtual Gender? Gender { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual TypeNoteEmited? TypeNoteEmited { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual AnnualBilling? AnnualBilling { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual NatureBackground? NatureBackground { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual List<BankDao> ListBanks { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual List<TelephoneDao> ListTelephones { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual List<ContactDayDao> ListContactDay { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual List<ContactTimeDao> ListContactTime { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public virtual List<FileDao> ListDocuments { get; set; }
    }
}
