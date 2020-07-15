////-----------------------------------------------------------------------
//// <copyright file="BaseRepository.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Exception;
    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Base NHibernate Repository
    /// </summary>
    /// <typeparam name="DT">Data Type derived from class <see cref="DataAccess.BaseModel" /></typeparam>
    public abstract class BaseRepository<DT> where DT : BaseDao
    {
        /// <summary> Repository session </summary>
        private readonly ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{DT}" /> class.
        /// </summary>
        public BaseRepository()
        {
            this.session = HibernateHelper.SessionFactory.GetCurrentSession();
        }

        /// <summary>
        /// Gets the current session to persist model values
        /// </summary>
        /// <returns>Session interface</returns>
        public ISession GetSession()
        {
            return this.session;
        }

        /// <summary>
        /// Inserts data into database
        /// </summary>
        /// <param name="data">Data to be added</param>
        public void Create(DT data)
        {
            try
            {
                this.session.Save(data);
            }
            catch (Exception e)
            {
                this.session.Transaction.Rollback();                
                throw new BusinessException(string.Format("Erro ao criar {0}", typeof(DT).FullName), e);
            }
        }

        /// <summary>
        /// Updates data in the database
        /// </summary>
        /// <param name="data">Data to be updated</param>
        public void Update(DT data)
        {
            try
            {
                this.session.Update(data);
            }
            catch (Exception e)
            {
                this.session.Transaction.Rollback();
                throw new BusinessException(string.Format("Erro ao atualizar {0}", typeof(DT).FullName), e);
            }
        }

        /// <summary>
        /// Creates or updates data in the database
        /// </summary>
        /// <param name="data">Data to be created or updated</param>
        public void CreateOrUpdate(DT data)
        {
            try
            {
                this.session.SaveOrUpdate(data);
            }
            catch (Exception e)
            {
                this.session.Transaction.Rollback();
                throw new BusinessException(string.Format("Erro ao criar ou atualizar {0}", typeof(DT).FullName), e);
            }
        }

        /// <summary>
        /// Deletes data from database
        /// </summary>
        /// <param name="data">Data to be deleted</param>
        public void Delete(DT data)
        {
            try
            {
                this.session.Delete(data);
            }
            catch (Exception e)
            {
                this.session.Transaction.Rollback();
                throw new BusinessException(string.Format("Erro ao excluir {0}", typeof(DT).FullName), e);
            }
        }

        /// <summary>
        /// Gets a list of all objects from database without filter
        /// </summary>
        /// <returns>All Data</returns>
        public IList<DT> GetAll()
        {
            try
            {
                return this.session.Query<DT>().ToList();
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar todos {0}", typeof(DT).FullName), e);
            }
        }
           
        /// <summary>
        /// Gets an object by Id
        /// </summary>
        /// <param name="id">Data identifier of the object to be retrieved</param>
        /// <returns>Data with the given id</returns>
        public DT GetById(int id)
        {
            try
            {
                return this.session.Get<DT>(id);
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao carregar {0} através do id", typeof(DT).FullName), e);
            }
        }

        /// <summary>
        /// Rollback last transaction
        /// </summary>
        public void RollbackTransaction()
        {   
            try
            {
                this.session.Transaction.Rollback();
            }
            catch (Exception e)
            {
                throw new BusinessException(string.Format("Erro ao tentar voltar a sessão", e));
            }
        }

        #region Utils

        /// <summary>
        /// Formats a value to Like operation
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Value converted</returns>
        protected string GetLikeValue(string value)
        {
            return string.Format("%{0}%", value);
        }

        #endregion
    }
}
