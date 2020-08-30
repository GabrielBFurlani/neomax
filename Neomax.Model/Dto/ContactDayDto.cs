////-----------------------------------------------------------------------
//// <copyright file="ContactDayDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Dto;
    using Neomax.Model.Util;
    using System.Collections.Generic;

    /// <summary>
    /// Database model representing Client Available
    /// </summary>
    public class ContactDayDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDao" /> class.
        /// </summary>
        public ContactDayDto()
        {
        }

        ///// <summary> Gets or sets the client name </summary>
        //public virtual ClientDto Client { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual ContactDay? ContactDay { get; set; }

        /// <summary> Gets or sets the client name </summary>
        public virtual string ContactDayName { get; set; }

    }
}
