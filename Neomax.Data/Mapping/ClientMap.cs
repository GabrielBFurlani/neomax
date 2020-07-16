////-----------------------------------------------------------------------
//// <copyright file="ClientMap.cs" company="Zetacorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using NHibernate;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Util;

    /// <summary>
    /// Mapping for Client model
    /// </summary>
    public class ClientMap : ClassMap<ClientDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMap" /> class.
        /// </summary>
        public ClientMap()
        {
            this.Table("Cliente");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.CNPJPayingSource, "CNPJFontePagadora");
            this.Map(x => x.Gender, "Sexo").CustomType<Gender>();
            this.Map(x => x.TypeNoteEmited, "TipoNotaEmitida").CustomType<TypeNoteEmited>();
            this.Map(x => x.AnnualBilling, "FaturamentoAnual").CustomType<AnnualBilling>();
            this.Map(x => x.NatureBackground, "NaturezaEmpresa").CustomType<CompanyNatureTypes>();
            HasManyToMany(x => x.ListTelephones).Table("ClienteTelefone").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoTelefone").Cascade.All();
            HasManyToMany(x => x.ListBanks).Table("ClienteBanco").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoBanco").Cascade.All();
            HasManyToMany(x => x.ListDocuments).Table("ClienteDocumento").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoDocumento").Cascade.All();
        }
    }
}
