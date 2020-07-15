////-----------------------------------------------------------------------
//// <copyright file="PasswordDefinitionMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;

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
            this.Table("DefinicaoSenha");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Token, "Token");
            this.Map(x => x.CreationDate, "DataCriacao");
            this.Map(x => x.ExpirationDate, "DataExpiracao");
            this.References(x => x.User, "CodigoUsuario");
        }
    }
}
