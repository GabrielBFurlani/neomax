////-----------------------------------------------------------------------
//// <copyright file="PasswordDefinitionRepository.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PlataformaZ2.Data.DataAccess;
    using PlataformaZ2.Model.Dto;
    using PlataformaZ2.Model.Exception;
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
