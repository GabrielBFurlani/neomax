////-----------------------------------------------------------------------
//// <copyright file="FileMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;
    using NHibernate.Type;
    using Neomax.Model.Util;

    /// <summary>
    /// Mapping for File model
    /// </summary>
    public class TelephoneMap : ClassMap<TelephoneDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneMap" /> class.
        /// </summary>
        public TelephoneMap()
        {
            this.Table("Telefone");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.ContactName, "NomeContato");
            this.Map(x => x.TelephoneType, "TipoTelefone").CustomType<TelephoneType>(); ;
            this.Map(x => x.Number, "Numero");
        }
    }
}
