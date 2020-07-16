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
        public string ImageBase64 { get; set; }

        /// <summary> Obtém ou define o MimeType da imagem </summary>
        public string MimeType { get; set; }

        /// <summary> Obtém ou define o nome do arquivo da imagem </summary>
        public string FileName { get; set; }

        /// <summary> Obtém ou define o código identificador do arquivo </summary>
        public int? FileId { get; set; }
    }
}