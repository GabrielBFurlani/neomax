////-----------------------------------------------------------------------
//// <copyright file="ProfileMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;

    /// <summary>
    /// Mapping for Profile model
    /// </summary>
    public class ProfileMap : ClassMap<ProfileDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileMap" /> class.
        /// </summary>
        public ProfileMap()
        {
            this.Table("Profile");
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Name);
            this.Map(x => x.Description);
            this.HasManyToMany(x => x.Permissions).Table("ProfilePermission").ParentKeyColumn("IdProfile").ChildKeyColumn("IdPermission");
        }
    }
}
