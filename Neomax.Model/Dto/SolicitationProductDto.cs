////-----------------------------------------------------------------------
//// <copyright file="SolicitationProductDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Transitional model representing client
    /// </summary>
    public class SolicitationProductDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationProductDto" /> class.
        /// </summary>
        public SolicitationProductDto()
        {

        }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string Suggestion { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public string Title { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public List<HttpFileBase64Dto> Files { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public string CNPJPayingSource { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public string ProductName { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public SolicitationStatus Status { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string StatusName { get; set; }
    }
}