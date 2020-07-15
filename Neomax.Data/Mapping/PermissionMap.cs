////-----------------------------------------------------------------------
//// <copyright file="PermissionMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;

    /// <summary>
    /// Mapping for Permission model
    /// </summary>
    public class PermissionMap : ClassMap<PermissionDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionMap" /> class.
        /// </summary>
        public PermissionMap()
        {
            this.Table("Permission");
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Name);
            this.Map(x => x.Description);
        }
    }
}
