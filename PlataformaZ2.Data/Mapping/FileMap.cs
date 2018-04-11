////-----------------------------------------------------------------------
//// <copyright file="FileMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using PlataformaZ2.Data.DataAccess;

    /// <summary>
    /// Mapping for File model
    /// </summary>
    public class FileMap : ClassMap<FileDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileMap" /> class.
        /// </summary>
        public FileMap()
        {
            this.Table("[File]");
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Name);
            this.Map(x => x.RealName);
        }
    }
}
