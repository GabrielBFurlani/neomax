////-----------------------------------------------------------------------
//// <copyright file="UserRepository.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using NHibernate;
    using NHibernate.Linq;
    using Neomax.Model.Util;

    /// <summary>
    /// Persists User data
    /// </summary>
    public class UserRepository : BaseRepository<UserDao>
    {
        /// <summary>
        /// Checks if username and password matches for a user
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="encryptedPassword">User's password (encrypted)</param>
        /// <returns>Matched user</returns>
        public UserDao MatchCredentials(string username, string encryptedPassword)
        {
            try
            {
                var query = GetSession().Query<UserDao>();

                var matchedUser = query.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(encryptedPassword) && x.Active);

                return matchedUser;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao autenticar usuário"), e);
            }
        }

        /// <summary>
        /// Check if there is another user with the same username
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <param name="username">User's username</param>
        /// <returns>boolean result</returns>
        public bool CheckUsername(int? idUser, string username)
        {
            try
            {
                var query = GetSession().Query<UserDao>();
                bool result;

                if (idUser.HasValue)
                {
                    result = query.Any(x => x.Username.Equals(username) && x.Id != idUser && x.Active);
                }
                else
                {
                    result = query.Any(x => x.Username.Equals(username) && x.Active);
                }
                
                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao verificar usuário"), e);
            }
        }


        public void ReleaseEmail(UserAvailableDao userAvailableDao)
        {
            var session = HibernateHelper.SessionFactory.GetCurrentSession();

            session.Save(userAvailableDao);
        }

        /// <summary>
        /// Check if there is another user with the same username
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <param name="username">User's username</param>
        /// <returns>boolean result</returns>
        public bool EmailExist(string email)
        {
            try
            {
                var query = GetSession().Query<UserDao>().Where(x => x.Email.ToLower().Equals(email.ToLower()));

                return query.Count() > 0;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao verificar usuário"), e);
            }
        }
        
        /// <summary>
        /// Check if there is another user with the same CPF
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <param name="cpf">User's cpf</param>
        /// <returns>boolean result</returns>
        public UserAvailableDao GetUserAvailable(string token)
        {
            try
            {
                var user = GetSession().Query<UserAvailableDao>().Where(x => x.Token.Equals(token)).FirstOrDefault();

                return user;
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Usuário não encontrado"), e);
            }
        }

        /// <summary>
        /// Gets user by username
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>User object</returns>
        public UserDao GetByUsername(string username)
        {
            return GetSession().Query<UserDao>().FirstOrDefault(x => x.Username.Equals(username) && x.Active);
        }
        
        /// <summary>
        /// Gets user by AccessToken
        /// </summary>
        /// <param name="accessToken">User's accessToken</param>
        /// <returns>User object</returns>
        public UserDao GetByAcessToken(string accessToken)
        {
            return GetSession().Query<UserDao>().FirstOrDefault(x => x.AccessToken.Equals(accessToken) && x.Active);
        }

        /// <summary>
        /// Get list of users refined by filter and pagination
        /// </summary>        
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with list of DAO </returns>
        public PaginationResponseFromRepository<UserDao> GetByFilter(UserFilterDto filter)
        {
            PaginationResponseFromRepository<UserDao> paginationResponse = new PaginationResponseFromRepository<UserDao>();

            try
            {                
                //// Refine using the filter parameters
                var query = GetSession().Query<UserDao>().Where(x => x.Client == null);

                query = query.Where(x => x.Active);

                if (!string.IsNullOrEmpty(filter.NameOrUsername))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(filter.NameOrUsername.ToLower()) || x.Username.ToLower().Contains(filter.NameOrUsername.ToLower()) || x.Email.ToLower().Contains(filter.NameOrUsername.ToLower()));
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
