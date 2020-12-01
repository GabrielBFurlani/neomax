////-----------------------------------------------------------------------
//// <copyright file="HttpFileDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    /// <summary>
    /// Transitional model representing a HTTP file
    /// </summary>
    public class HttpFileDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpFileDto" /> class.
        /// </summary>
        public HttpFileDto()
        {
        }

        /// <summary> Gets or sets the file's name </summary>
        public string FileName { get; set; }

        /// <summary> Gets or sets the file's content (bytes) </summary>
        public byte[] Content { get; set; }

        /// <summary> Gets or sets the file's MIME Type </summary>
        public string MimeType { get; set; }
    }
}