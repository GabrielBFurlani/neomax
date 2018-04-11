////-----------------------------------------------------------------------
//// <copyright file="UserDao.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.DataAccess
{  
    using System;
    using System.Collections.Generic;
    using PlataformaZ2.Model.Util;

    /// <summary>
    /// Database model representing User
    /// </summary>
    public class UserDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDao" /> class.
        /// </summary>
        public UserDao()
        {            
        }

        /// <summary> Gets or sets the user's username (email) </summary>
        public virtual string Username { get; set; }

        /// <summary> Gets or sets the user's password </summary>
        public virtual string Password { get; set; }

        /// <summary> Gets or sets the user's name </summary>
        public virtual string Name { get; set; }

        /// <summary> Gets or sets the user's nickname </summary>
        public virtual string Nickname { get; set; }

        /// <summary> Gets or sets the user's CPF </summary>
        public virtual string Cpf { get; set; }

        /// <summary> Gets or sets the user's photo (it's a reference for another DAO) </summary>
        public virtual FileDao Photo { get; set; }

        /// <summary> Gets or sets the user's profile (it's a reference for another DAO) </summary>
        public virtual ProfileDao Profile { get; set; }      

        /// <summary> Gets or sets the user's access token for API </summary>
        public virtual string AccessToken { get; set; }

        /// <summary> Gets or sets the user's access token creation date for API </summary>
        public virtual DateTime? AccessTokenCreationDate { get; set; }

        /// <summary> Gets or sets a value indicating whether user is active (when deleted, it's kept on the table, but the status is false) </summary>
        public virtual bool Active { get; set; }
    }
}
