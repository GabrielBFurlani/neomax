////-----------------------------------------------------------------------
//// <copyright file="UserSessionDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Dto
{
    using PlataformaZ2.Model.Util;

    /// <summary>
    /// Transitional model representing a session of the logged user
    /// </summary>
    public class UserSessionDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionDto" /> class.
        /// </summary>
        public UserSessionDto()
        {
        }

        /// <summary> Gets or sets the user's identifier </summary>
        public int IdUser { get; set; }

        /// <summary> Gets or sets the user's username </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public string Nickname { get; set; }

        /// <summary> Gets or sets the user's profile identifier </summary>
        public int IdProfile { get; set; }

        /// <summary> Gets or sets the user's photo </summary>
        public ImageBase64Dto Photo { get; set; }

        /// <summary> Gets or sets the access token </summary>
        public string AccessToken { get; set; }
    }
}
