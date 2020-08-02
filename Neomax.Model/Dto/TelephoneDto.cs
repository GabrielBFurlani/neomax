////-----------------------------------------------------------------------
//// <copyright file="TelephoneDto.cs" company="Gabriel Furlani">
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
    public class TelephoneDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneDto" /> class.
        /// </summary>
        public TelephoneDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Number { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public TelephoneType TelephoneType { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string ContactName { get; set; }
    }
}