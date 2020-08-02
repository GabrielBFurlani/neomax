////-----------------------------------------------------------------------
//// <copyright file="PasswordDefinitionRepository.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using Mapping;
    using NHibernate.Linq;

    /// <summary>
    /// Persists PasswordDefinition data
    /// </summary>
    public class PasswordDefinitionRepository : BaseRepository<PasswordDefinitionDao>
    {
        /// <summary>
        /// Checks if the user has a valid token
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <returns>PasswordDefinition Dao</returns>
        public PasswordDefinitionDao SearchForValidToken(int idUser)
        {
            DateTime today = DateTime.Now;

            try
            {
                var query = GetSession().Query<PasswordDefinitionDao>();

                var result = query.FirstOrDefault(x => x.User.Id.Value == idUser 
                                                    && x.CreationDate < today 
                                                    && x.ExpirationDate > today);

                return result;
            }
            catch (Exception)
            {
                throw new BusinessException(string.Format("Erro ao buscar um token"));
            }
        }
    }
}
