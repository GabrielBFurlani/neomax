////-----------------------------------------------------------------------
//// <copyright file="ProfileDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;

    /// <summary>
    /// Transitional model representing Profile
    /// </summary>
    public class ProfileDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileDto" /> class.
        /// </summary>
        public ProfileDto()
        {
        }

        /// <summary> Gets or sets the profile's name </summary>
        public string Name { get; set; }
    }
}
