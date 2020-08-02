////-----------------------------------------------------------------------
//// <copyright file="SolicitationDto.cs" company="Gabriel Furlani">
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
    public class SolicitationDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationDto" /> class.
        /// </summary>
        public SolicitationDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public List<SolicitationProductDto> ProductsList { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public int? IdClient { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public ClientDto Client { get; set; }
        
        /// <summary> Gets or sets the client name </summary>
        public string Protocol { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public SolicitationStatus Status { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string StatusName { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public DateTime CreationDate { get; set; }

    }
}