////-----------------------------------------------------------------------
//// <copyright file="BankDto.cs" company="Gabriel Furlani">
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
    public class BankDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankDto" /> class.
        /// </summary>
        public BankDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Bank { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string Agency { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string Account { get; set; }
    }
}