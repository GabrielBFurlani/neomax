////-----------------------------------------------------------------------
//// <copyright file="ProfileRepository.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Persists Profile data
    /// </summary>
    public class SolicitationRepository : BaseRepository<SolicitationDao>
    {
        /// <summary>
        /// Get list of client refined by filter and pagination
        /// </summary>        
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with list of DAO </returns>
        public PaginationResponseDto<SolicitationDao> GetForAdmin(SolicitationFilterDto filter)
        {
            PaginationResponseDto<SolicitationDao> paginationResponse = new PaginationResponseDto<SolicitationDao>();

            try
            {
                //// Refine using the filter parameters
                var query = GetSession().Query<SolicitationDao>();

                if (filter.CreationDate.HasValue)
                {
                    query = query.Where(x => x.CreationDate.Date == filter.CreationDate.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(filter.Argument))
                {
                    query = query.Where(x => x.Client.User.Name.ToLower().Contains(filter.Argument.ToLower()) || x.Client.User.Username.ToLower().Contains(filter.Argument.ToLower())
                    || x.Protocol.ToLower().Contains(filter.Argument.ToLower()) || x.Protocol.Replace("/", "").ToLower().Contains(filter.Argument.Replace("/", "").ToLower()));
                }

                if (filter.SolicitationStatus.HasValue)
                {
                    query = query.Where(x => x.Status == filter.SolicitationStatus.Value);
                }

                //// Pagination                
                paginationResponse.TotalResults = query.Count();
                //
                paginationResponse.Response = query.ToList().OrderBy(x => x.Status).ThenByDescending(x => x.CreationDate).Skip((filter.PageNumber - 1) * filter.ResultsPerPage).Take(filter.ResultsPerPage).ToList();

                return paginationResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar {0} através de filtro", typeof(SolicitationDao).FullName), e);
            }
        }

        /// <summary>
        /// Get list of client refined by filter and pagination
        /// </summary>        
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with list of DAO </returns>
        public PaginationResponseDto<SolicitationDao> GetForClient(SolicitationFilterDto filter)
        {
            PaginationResponseDto<SolicitationDao> paginationResponse = new PaginationResponseDto<SolicitationDao>();

            try
            {
                //// Refine using the filter parameters
                var query = GetSession().Query<SolicitationDao>().Where(x => x.Client.Id == filter.IdClient);

                if (filter.CreationDate.HasValue)
                {
                    query = query.Where(x => x.CreationDate.Date == filter.CreationDate.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(filter.Argument))
                {
                    query = query.Where(x => x.Protocol.ToLower().Contains(filter.Argument.ToLower()) || x.Protocol.Replace("/","").ToLower().Contains(filter.Argument.Replace("/", "").ToLower()));
                }

                if (filter.SolicitationStatus.HasValue)
                {
                    query = query.Where(x => x.Status == filter.SolicitationStatus.Value);
                }

                //// Pagination                
                paginationResponse.TotalResults = query.Count();
                paginationResponse.Response = query.ToList().OrderByDescending(x => x.Status == SolicitationStatus.RevisionSolicited).ThenByDescending(x => x.Status == SolicitationStatus.WaitingApprove).ThenByDescending(x => x.Status).ThenByDescending(x => x.CreationDate).ToList().Skip((filter.PageNumber - 1) * filter.ResultsPerPage).Take(filter.ResultsPerPage).ToList();

               

                return paginationResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar {0} através de filtro", typeof(SolicitationDao).FullName), e);
            }
        }

        public string GetLastProtocolNumber()
        {
            return GetSession().Query<SolicitationDao>().Where(x => x.CreationDate.Year == DateTime.Now.Year).Count().ToString();
        }
    }
}
