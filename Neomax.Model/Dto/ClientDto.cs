////-----------------------------------------------------------------------
//// <copyright file="ClientDto.cs" company="ZetaCorp">
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
    public class ClientDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDto" /> class.
        /// </summary>
        public ClientDto()
        {

        }

        /// <summary> Gets or sets the client name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public string CNPJ { get; set; }

        /// <summary> Gets or sets the client url base </summary>
        public string Url { get; set; }

        /// <summary> Gets or sets the client CNPJ </summary>
        public HttpFileBase64Dto Photo { get; set; }

        /// <summary> Gets or sets a value indicating whether client is active (when deleted, it's kept on the table, but the status is false) </summary>
        public bool Active { get; set; }
    }
}