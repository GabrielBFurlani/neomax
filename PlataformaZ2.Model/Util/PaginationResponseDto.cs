////-----------------------------------------------------------------------
//// <copyright file="PaginationResponseDto.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Util
{
    using PlataformaZ2.Model.Dto;
    using System.Collections.Generic;

    /// <summary>
    /// Transitional model representing a pagination response from a search
    /// </summary>
    /// <typeparam name="DT">Data Type derived from class <see cref="PaginationResponseDto" /></typeparam>
    public class PaginationResponseDto<DT>
    {
        /// <summary> Gets or sets the total quantity of results </summary>
        public int TotalResults { get; set; }

        /// <summary> Gets or sets the total quantity of results per page </summary>
        public int ResultsPerPage { get; set; }

        /// <summary> Gets or sets the number of the current page </summary>
        public int CurrentPage { get; set; }

        /// <summary> Gets or sets the list of registers </summary>
        public IList<DT> Response { get; set; }
    }
}