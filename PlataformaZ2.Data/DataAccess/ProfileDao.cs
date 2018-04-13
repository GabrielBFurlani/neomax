﻿////-----------------------------------------------------------------------
//// <copyright file="ProfileDao.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.DataAccess
{
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Profile
    /// </summary>
    public class ProfileDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileDao" /> class.
        /// </summary>
        public ProfileDao()
        {
        }

        /// <summary> Gets or sets the profile's name </summary>
        public virtual string Name { get; set; }

        /// <summary> Gets or sets the profile's description </summary>
        public virtual string Description { get; set; }

        /// <summary> Gets or sets the profile's permission list (it's a many-to-many reference) </summary>
        public virtual IList<PermissionDao> Permissions { get; set; }
    }
}