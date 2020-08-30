////-----------------------------------------------------------------------
//// <copyright file="ClientDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Transitional model representing client
    /// </summary>
    public class ClientDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDto" /> class.
        /// </summary>
        public ClientDto()
        {

        }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string CNPJPayingSource { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public Gender? Gender { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string GenderName { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public TypeNoteEmited? TypeNoteEmited { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string TypeNoteEmitedName { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public AnnualBilling? AnnualBilling { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string AnnualBillingName { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public CompanyNatureTypes? NatureBackground { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string NatureBackgroundName { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public IList<BankDto> ListBanks { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public IList<TelephoneDto> ListTelephones { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public IList<ContactDayDto> ListContactDay { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public IList<ContactTimeDto> ListContactTime { get; set; }

        /// <summary> Gets or sets the client Blazon </summary>
        public IList<HttpFileBase64Dto> ListDocumentsBase64 { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public UserDto User { get; set; }
    }
}