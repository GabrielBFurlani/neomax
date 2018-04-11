////-----------------------------------------------------------------------
//// <copyright file="UserFilterDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Dto
{
    using PlataformaZ2.Model.Util;

    /// <summary>
    /// Transitional model representing User filter
    /// </summary>
    public class UserFilterDto : BaseFilterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFilterDto" /> class.
        /// </summary>
        public UserFilterDto()
        {
        }

        /// <summary> Gets or sets the user's name or username </summary>
        public string NameOrUsername { get; set; }
        
        /// <summary> Gets or sets the user's profile Id </summary>
        public int? IdProfile { get; set; }
    }
}
