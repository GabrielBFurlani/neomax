////-----------------------------------------------------------------------
//// <copyright file="PaginationResponseFromRepository.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    using Neomax.Model.Dto;
    using System.Collections.Generic;

    /// <summary>
    /// Transitional model representing a pagination response containing DAOs (comes from repository)
    /// </summary>
    /// <typeparam name="DT">Data Type derived from class <see cref="PaginationResponseFromRepository" /></typeparam>
    public class PaginationResponseFromRepository<DT>
    {
        /// <summary> Gets or sets the total quantity of results </summary>
        public int TotalResults { get; set; }

        /// <summary> Gets or sets the list of registers </summary>
        public IList<DT> Response { get; set; }
    }
}