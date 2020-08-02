////-----------------------------------------------------------------------
//// <copyright file="BankMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;
    using NHibernate.Type;

    /// <summary>
    /// Mapping for File model
    /// </summary>
    public class BankMap : ClassMap<BankDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankMap" /> class.
        /// </summary>
        public BankMap()
        {
            this.Table("Banco");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Account, "Conta");
            this.Map(x => x.Agency, "Agencia");
            this.Map(x => x.Bank, "Banco");
        }
    }
}
