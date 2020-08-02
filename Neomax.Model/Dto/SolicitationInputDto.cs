////-----------------------------------------------------------------------
//// <copyright file="SolicitationInputDto.cs" company="Gabriel Furlani">
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
    public class SolicitationInputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationInputDto" /> class.
        /// </summary>
        public SolicitationInputDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public List<SolicitationProductInputDto> Products { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public int? IdClient { get; set; }
    }
}