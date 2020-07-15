////-----------------------------------------------------------------------
//// <copyright file="TelephoneDto.cs" company="ZetaCorp">
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
    public class TelephoneDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneDto" /> class.
        /// </summary>
        public TelephoneDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        [JsonProperty("Numero")]
        public string Number { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        [JsonProperty("Tipo")]
        public TelephoneType TelephoneType { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        [JsonProperty("Contato")]
        public string ContactName { get; set; }
    }
}