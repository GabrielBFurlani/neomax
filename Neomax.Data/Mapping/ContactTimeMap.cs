////-----------------------------------------------------------------------
//// <copyright file="ContactTimeMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Util;
    using NHibernate.Type;

    /// <summary>
    /// Mapping for File model
    /// </summary>
    public class ContactTimeMap : ClassMap<ContactTimeDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactTimeMap" /> class.
        /// </summary>
        public ContactTimeMap()
        {
            this.Table("ClienteHoraContato");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.References(x => x.Client, "CodigoCliente");
            this.Map(x => x.ContactTime, "horarioContato").CustomType<ContactTime>(); ;
        }
    }
}
