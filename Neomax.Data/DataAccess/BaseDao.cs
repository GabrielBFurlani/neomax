////-----------------------------------------------------------------------
//// <copyright file="BaseDao.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
