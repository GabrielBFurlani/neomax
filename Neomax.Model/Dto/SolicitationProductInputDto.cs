////-----------------------------------------------------------------------
//// <copyright file="SolicitationProductInputDto.cs" company="Gabriel Furlani">
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
    public class SolicitationProductInputDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationProductInputDto" /> class.
        /// </summary>
        public SolicitationProductInputDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Title { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public List<ImageBase64Dto> Files { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public string CNPJPayingSource { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public string ProductType { get; set; }
    }
}