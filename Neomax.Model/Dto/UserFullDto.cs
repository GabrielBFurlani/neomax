////-----------------------------------------------------------------------
//// <copyright file="UserDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using System;
    using System.Collections.Generic;
    using Neomax.Model.Util;

    /// <summary>
    /// Transitional model representing User
    /// </summary>
    public class UserDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto" /> class.
        /// </summary>
        public UserDto()
        {
        }

        /// <summary> Gets or sets the user's username (email) </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public string Nickname { get; set; }

        /// <summary> Gets or sets the user's CPF </summary>
        public string Email { get; set; }

        /// <summary> Gets or sets the user's photo </summary>
        public HttpFileBase64Dto Photo { get; set; }

        /// <summary> Gets or sets the user's photo </summary>
        public ClientDto ClientDto { get; set; }
    }
}
