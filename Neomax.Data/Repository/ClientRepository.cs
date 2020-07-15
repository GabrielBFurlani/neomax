////-----------------------------------------------------------------------
//// <copyright file="ProfileRepository.cs" company="ZetaCorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using NHibernate;
    using NHibernate.Linq;
    using Neomax.Model.Util;
    using AutoMapper;

    /// <summary>
    /// Persists Profile data
    /// </summary>
    public class ClientRepository : BaseRepository<ClientDao>
    {
        /// <summary>
        /// Get list of client refined by filter and pagination
        /// </summary>        
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with list of DAO </returns>
        public PaginationResponseDto<UserDao> GetByFilter(ClientFilterDto filter)
        {
            PaginationResponseDto<UserDao> paginationResponse = new PaginationResponseDto<UserDao>();

            try
            {
                //// Refine using the filter parameters
                var query = GetSession().Query<UserDao>().Where(x => x.Client != null);

                query = query.Where(x => x.Active);

                if (!string.IsNullOrEmpty(filter.Argument))
                {
                    query = query.Where(x => x.Name.Contains(filter.Argument) || x.Username.Contains(filter.Argument));
                }

                //// Pagination                
                paginationResponse.TotalResults = query.Count();
                paginationResponse.Response = query.OrderBy(x => x.Name).Skip((filter.PageNumber - 1) * filter.ResultsPerPage).Take(filter.ResultsPerPage).ToList();

                return paginationResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar {0} através de filtro", typeof(UserDao).FullName), e);
            }
        }
    }
}
