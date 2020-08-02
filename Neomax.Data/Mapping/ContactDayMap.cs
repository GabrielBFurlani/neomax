////-----------------------------------------------------------------------
//// <copyright file="ContactDayMap.cs" company="Gabriel Furlani">
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
    public class ContactDayMap : ClassMap<ContactDayDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDayMap" /> class.
        /// </summary>
        public ContactDayMap()
        {
            this.Table("ClienteDiaContato");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.References(x => x.Client, "CodigoCliente");
            this.Map(x => x.ContactDay, "dataContato");
        }
    }
}
