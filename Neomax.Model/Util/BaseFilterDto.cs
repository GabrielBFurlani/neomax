////-----------------------------------------------------------------------
//// <copyright file="BaseFilterDto.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    /// <summary>
    /// Base Model for transitional filter models
    /// </summary>
    public class BaseFilterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFilterDto" /> class.
        /// </summary>
        public BaseFilterDto()
        {
        }

        /// <summary> Gets or sets the page number of the search results </summary>
        public int PageNumber { get; set; }

        /// <summary> Gets or sets the quantity of results per page </summary>
        public byte ResultsPerPage { get; set; }
    }
}
