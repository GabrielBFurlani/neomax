////-----------------------------------------------------------------------
//// <copyright file="BankDto.cs" company="ZetaCorp">
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
    public class BankDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankDto" /> class.
        /// </summary>
        public BankDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        [JsonProperty("Banco")]
        public string Bank { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("Agencia")]
        public string Agency { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("Conta")]
        public string Account { get; set; }
    }
}