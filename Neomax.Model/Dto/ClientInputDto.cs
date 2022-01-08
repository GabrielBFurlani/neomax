////-----------------------------------------------------------------------
//// <copyright file="ClientInputDto.cs" company="Gabriel Furlani">
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
    public class ClientInputDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInputDto" /> class.
        /// </summary>
        public ClientInputDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string NickName { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string Password { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string PasswordConfirmation { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string CnpjPaying { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string Email { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public Gender? Gender { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public TypeNoteEmited? TypeNoteEmited { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public AnnualBilling? AnnualBilling { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public CompanyNatureTypes? CompanyNatureType { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public HttpFileBase64Dto Photo { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public List<BankDto> Banks { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public List<TelephoneDto> Telephones { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public List<ContactDay> ContactDays { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public List<ContactTime> ContactTimes { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public List<HttpFileBase64Dto> Documents { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public bool HasPhoto { get; set; }

        public int IdUser { get; set; }
    }
}