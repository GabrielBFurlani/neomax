////-----------------------------------------------------------------------
//// <copyright file="ChangePasswordDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    /// <summary>
    /// Transitional model representing password change
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordDto" /> class.
        /// </summary>
        public ChangePasswordDto()
        {
        }

        /// <summary> Gets or sets the user's identifier </summary>
        public int IdUser { get; set; }

        /// <summary> Gets or sets the user's new password </summary>
        public string NewPassword { get; set; }

        /// <summary> Gets or sets the token for password change </summary>
        public string ChangePasswordToken { get; set; }
    }
}
