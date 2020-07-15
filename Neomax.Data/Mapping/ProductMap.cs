////-----------------------------------------------------------------------
//// <copyright file="ProductMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
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
    public class ProductMap : ClassMap<ProductDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductMap" /> class.
        /// </summary>
        public ProductMap()
        {
            this.Table("Produto");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Description, "Descricao");
            this.Map(x => x.Name, "Nome");
            this.Map(x => x.Active, "Ativo");
        }
    }
}
