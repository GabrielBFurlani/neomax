////-----------------------------------------------------------------------
//// <copyright file="UserInputDto.cs" company="ZetaCorp">
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
    /// Transitional model representing User SignUp
    /// </summary>
    public class UserInputDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInputDto" /> class.
        /// </summary>
        public UserInputDto()
        {
        }

        /// <summary> Gets or sets the user's username (email) </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's password </summary>
        public string Password { get; set; }

        /// <summary> Gets or sets the user's password </summary>
        public string PasswordConfirmation { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public string Nickname { get; set; }

        /// <summary> Gets or sets the user's email </summary>
        public string Email { get; set; }
        
        /// <summary> Gets or sets the user's photo </summary>
        public ImageBase64Dto Photo { get; set; }
    }
}
