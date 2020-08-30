////-----------------------------------------------------------------------
//// <copyright file="UpdateProductStatusInputDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
using Neomax.Model.Util;
using System.Web;

namespace Neomax.Model.Dto
{
    /// <summary>
    /// Transitional model representing password change
    /// </summary>
    public class UpdateProductStatusInputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductStatusInputDto" /> class.
        /// </summary>
        public UpdateProductStatusInputDto()
        {
        }

        /// <summary> Gets or sets the user's identifier </summary>
        public SolicitationStatus Status { get; set; }

        /// <summary> Gets or sets the user's new password </summary>
        public string Suggestion { get; set; }
    }
}
