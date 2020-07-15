////-----------------------------------------------------------------------
//// <copyright file="PermissionDao.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.DataAccess
{
    /// <summary>
    /// Database model representing Permission
    /// </summary>
    public class PermissionDao : BaseDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionDao" /> class.
        /// </summary>
        public PermissionDao()
        {
        }

        /// <summary> Gets or sets the permission's name </summary>
        public virtual string Name { get; set; }

        /// <summary> Gets or sets the permission's description </summary>
        public virtual string Description { get; set; }
    }
}
