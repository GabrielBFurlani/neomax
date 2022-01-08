////-----------------------------------------------------------------------
//// <copyright file="UserSessionDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;

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
        public int Id { get; set; }

        /// <summary> Gets or sets the user's username </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Email { get; set; }

        /// <summary> Gets or sets the user's photo </summary>
        public ClientDto Client { get; set; }

        public int? IdClient { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public string Nickname { get; set; }

        /// <summary> Gets or sets the user's profile identifier </summary>
        public bool IsAdmin { get; set; }

        /// <summary> Gets or sets the user's photo </summary>
        public HttpFileBase64Dto Photo { get; set; }

        /// <summary> Gets or sets the access token </summary>
        public string AccessToken { get; set; }
    }
}
