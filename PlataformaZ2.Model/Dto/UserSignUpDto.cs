////-----------------------------------------------------------------------
//// <copyright file="UserSignUpDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Dto
{  
    using System;
    using System.Collections.Generic;
    using PlataformaZ2.Model.Util;

    /// <summary>
    /// Transitional model representing User SignUp
    /// </summary>
    public class UserSignUpDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSignUpDto" /> class.
        /// </summary>
        public UserSignUpDto()
        {
        }

        /// <summary> Gets or sets the user's username (email) </summary>
        public string Username { get; set; }

        /// <summary> Gets or sets the user's password </summary>
        public string Password { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public string Nickname { get; set; }

        /// <summary> Gets or sets the user's CPF </summary>
        public string Cpf { get; set; }
        
        /// <summary> Gets or sets the user's photo </summary>
        public ImageBase64Dto Photo { get; set; }
    }
}
