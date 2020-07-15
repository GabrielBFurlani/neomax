////-----------------------------------------------------------------------
//// <copyright file="UserListItemDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Dto
{
    using Neomax.Model.Util;

    /// <summary>
    /// Transitional model representing an User Item in a list
    /// </summary>
    public class UserListItemDto : BaseModelDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserListItemDto" /> class.
        /// </summary>
        public UserListItemDto()
        {
        }

        /// <summary> Gets or sets the user's name </summary>
        public string Name { get; set; }

        /// <summary> Gets or sets the user's profile name </summary>
        public string ProfileName { get; set; }
    }
}
