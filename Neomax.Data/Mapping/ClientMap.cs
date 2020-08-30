////-----------------------------------------------------------------------
//// <copyright file="ClientMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
            this.Map(x => x.Gender, "Sexo").CustomType<Gender>();
            this.Map(x => x.TypeNoteEmited, "TipoNotaEmitida").CustomType<TypeNoteEmited>();
            this.Map(x => x.AnnualBilling, "FaturamentoAnual").CustomType<AnnualBilling>();
            this.Map(x => x.NatureBackground, "NaturezaEmpresa").CustomType<CompanyNatureTypes>();
            this.References(x => x.User, "CodigoUsuario");
            HasManyToMany(x => x.ListTelephones).Table("ClienteTelefone").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoTelefone").Cascade.All();
            HasManyToMany(x => x.ListBanks).Table("ClienteBanco").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoBanco").Cascade.All();
            HasManyToMany(x => x.ListDocuments).Table("ClienteDocumento").ParentKeyColumn("CodigoCliente").ChildKeyColumn("CodigoDocumento").Cascade.All();
            //HasManyToMany(x => x.ListContactTime).Table("ClienteHoraContato").ParentKeyColumn("CodigoCliente").Cascade.All();
            //HasManyToMany(x => x.ListContactDay).Table("ClienteDiaContato").ParentKeyColumn("CodigoCliente").Cascade.All();
        }
    }
}
