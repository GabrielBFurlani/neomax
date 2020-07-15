////-----------------------------------------------------------------------
//// <copyright file="BaseDao.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    /// <summary>
    /// Base fields for database models
    /// </summary>
    public class BaseDao
    {
        /// <summary> Gets or sets the DAO's Id </summary>
        public virtual int? Id { get; set; }
    }
}
