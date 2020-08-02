////-----------------------------------------------------------------------
//// <copyright file="FileIdentificationDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    /// <summary>
    /// Transitional model representing a file identification (name and real name) 
    /// </summary>
    public class FileIdentificationDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileIdentificationDto" /> class.
        /// </summary>
        public FileIdentificationDto()
        {
        }

        /// <summary> Gets or sets the file's name (for user visualization) </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the file's real name (at file system) </summary>
        public string RealName { get; set; }
    }
}
