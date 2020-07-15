////-----------------------------------------------------------------------
//// <copyright file="NewUserDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;
    using Newtonsoft.Json;

    /// <summary>
    /// Transitional model representing Profile
    /// </summary>
    public class NewUserDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewUserDto" /> class.
        /// </summary>
        public NewUserDto()
        {
        }

        /// <summary> Gets or sets the profile's name </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("tokenNovoUsuario")]
        public string Token { get; set; }

    }
}
