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
                    query = query.Where(x => x.Name.ToLower().Contains(filter.Argument.ToLower()) || x.Username.ToLower().Contains(filter.Argument.ToLower()) || x.Nickname.ToLower().Contains(filter.Argument.ToLower()));
                }

                //// Pagination                
                paginationResponse.TotalResults = query.Count();
                paginationResponse.Response = query.OrderBy(x => x.Name).Skip((filter.PageNumber - 1) * filter.ResultsPerPage).Take(filter.ResultsPerPage).ToList();
                paginationResponse.ResultsPerPage = filter.ResultsPerPage;

                return paginationResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar {0} através de filtro", typeof(UserDao).FullName), e);
            }
        }

        public List<ContactDayDao> GetContactDayByIdClient(int idClient)
        {
            var query = GetSession().Query<ContactDayDao>().Where(x => x.Client.Id == idClient);

            return query.ToList();
        }

        public ClientDao GetByIdUser(int idUser)
        {
            var query = GetSession().Query<ClientDao>().Where(x => x.User.Id == idUser).FirstOrDefault();

            return query;
        }

        public List<ContactTimeDao> GetContactTimeByIdClient(int idClient)
        {
            var query = GetSession().Query<ContactTimeDao>().Where(x => x.Client.Id == idClient);

            return query.ToList();
        }

        public void CreateContactTime(ContactTimeDao contactTimeDao)
        {
            var session = HibernateHelper.SessionFactory.GetCurrentSession();

            session.Save(contactTimeDao);
        }

        public void CreateContactHour(ContactDayDao contactDayDao)
        {
            var session = HibernateHelper.SessionFactory.GetCurrentSession();

            session.Save(contactDayDao);
        }

    }
}
