////-----------------------------------------------------------------------
//// <copyright file="PasswordDefinitionMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using PlataformaZ2.Data.DataAccess;

    /// <summary>
    /// Mapping for PasswordDefinition model
    /// </summary>
    public class PasswordDefinitionMap : ClassMap<PasswordDefinitionDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordDefinitionMap" /> class.
        /// </summary>
        public PasswordDefinitionMap()
        {
            this.Table("[PasswordDefinition]");
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Token);
            this.Map(x => x.CreationDate);
            this.Map(x => x.ExpirationDate);
            this.References(x => x.User, "IdUser");
        }
    }
}
