////-----------------------------------------------------------------------
//// <copyright file="ClientFilterDto.cs" company="ZetaCorp">
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
    public class ClientFilterDto : BaseFilterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientFilterDto" /> class.
        /// </summary>
        public ClientFilterDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Argument { get; set; }

        /// <summary> Gets or sets the client configurations list (it's a many-to-many reference) </summary>
        public int? IdCity { get; set; }

        /// <summary> Gets or sets the state of City </summary>
        public bool ActiveOnly { get; set; }
    }
}