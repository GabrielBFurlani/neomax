////-----------------------------------------------------------------------
//// <copyright file="CredentialsDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    /// <summary>
    /// Transitional model representing credentials
    /// </summary>
    public class CredentialsDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialsDto" /> class.
        /// </summary>
        public CredentialsDto()
        {
        }

        /// <summary> Gets or sets the user's username </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's password </summary>
        public string Password { get; set; }
    }
}
