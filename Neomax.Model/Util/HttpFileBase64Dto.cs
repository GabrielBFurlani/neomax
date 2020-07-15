////-----------------------------------------------------------------------
//// <copyright file="HttpFileBase64Dto.cs" company="ZetaCorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    using Newtonsoft.Json;

    /// <summary>
    /// Transitional model representing a http file base64
    /// </summary>
    public class HttpFileBase64Dto
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpFileBase64Dto" /> class.
        /// </summary>
        public HttpFileBase64Dto()
        {
        }

        /// <summary> Obtém ou define o conteúdo da imagem em base 64 </summary>
        [JsonProperty("imagemBase64")]
        public string ImagemBase64 { get; set; }

        /// <summary> Obtém ou define o MimeType da imagem </summary>
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        /// <summary> Obtém ou define o nome do arquivo da imagem </summary>
        [JsonProperty("nomeArquivo")]
        public string FileName { get; set; }

        /// <summary> Obtém ou define o código identificador do arquivo </summary>
        [JsonProperty("codigoArquivo")]
        public int? FileId { get; set; }
    }
}