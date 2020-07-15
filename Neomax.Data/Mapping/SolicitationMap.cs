////-----------------------------------------------------------------------
//// <copyright file="SolicitationMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Util;

    /// <summary>
    /// Mapping for PasswordDefinition model
    /// </summary>
    public class SolicitationMap : ClassMap<SolicitationDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolicitationMap" /> class.
        /// </summary>
        public SolicitationMap()
        {
            this.Table("Solicitacao");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Protocol, "Protocolo");
            this.Map(x => x.CreationDate, "DataCriacao");
            this.Map(x => x.Status, "Status").CustomType<SolicitationStatus>();
            this.References(x => x.Client, "CodigoCliente");
        }
    }
}
