////-----------------------------------------------------------------------
//// <copyright file="PermissionMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
