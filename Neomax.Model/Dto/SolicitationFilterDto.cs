////-----------------------------------------------------------------------
//// <copyright file="SolicitationFilterDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Transitional model representing client
    /// </summary>
    public class SolicitationFilterDto : BaseFilterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationFilterDto" /> class.
        /// </summary>
        public SolicitationFilterDto()
        {

        }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string Argument { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public int? IdClient { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public SolicitationStatus? SolicitationStatus { get; set; }
    }
}