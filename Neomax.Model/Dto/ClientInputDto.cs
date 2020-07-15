////-----------------------------------------------------------------------
//// <copyright file="ClientInputDto.cs" company="ZetaCorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
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
        [JsonProperty("nome")]
        public string Name { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("Username")]
        public string Username { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("NickName")]
        public string NickName { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("Senha")]
        public string Password { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("SenhaConfirmacao")]
        public string PasswordConfirmation { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("CNPJFontePagadora")]
        public string CNPJPayingSource { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("sexo")]
        public Gender? Gender { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("TipoNotaEmitida")]
        public TypeNoteEmited? TypeNoteEmited { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("FaturamentoAnual")]
        public AnnualBilling? AnnualBilling { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("NaturezaEmpresa")]
        public NatureBackground? NatureBackground { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("foto")]
        public ImageBase64Dto Photo { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        [JsonProperty("Bancos")]
        public List<BankDto> Banks { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        [JsonProperty("Bancos")]
        public List<TelephoneDto> Telephones { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        [JsonProperty("DiasContato")]
        public List<ContactDay> ContactDays { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        [JsonProperty("HorasContato")]
        public List<ContactTime> ContactTimes { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        [JsonProperty("Documentos")]
        public List<HttpFileBase64Dto> Documents { get; set; }
    }
}