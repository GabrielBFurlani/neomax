////-----------------------------------------------------------------------
//// <copyright file="UserFilterDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;

    /// <summary>
    /// Transitional model representing User filter
    /// </summary>
    public class UserFilterDto : BaseFilterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFilterDto" /> class.
        /// </summary>
        public UserFilterDto()
        {
        }

        /// <summary> Gets or sets the user's name or username </summary>
        public string NameOrUsername { get; set; }
        
        /// <summary> Gets or sets the user's profile Id </summary>
        public int? IdProfile { get; set; }
    }
}
