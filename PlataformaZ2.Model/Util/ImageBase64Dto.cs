////-----------------------------------------------------------------------
//// <copyright file="ImageBase64Dto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Util
{
    /// <summary>
    /// Transitional model representing an image in base64 with MIME type
    /// </summary>
    public class ImageBase64Dto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBase64Dto" /> class.
        /// </summary>
        public ImageBase64Dto()
        {
        }

        /// <summary> Gets or sets the image string in base64 </summary>
        public string ImageData { get; set; }

        /// <summary> Gets or sets the image's MIME Type </summary>
        public string MimeType { get; set; }
    }
}
