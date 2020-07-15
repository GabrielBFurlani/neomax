////-----------------------------------------------------------------------
//// <copyright file="FileMap.cs" company="Zetacorp">
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
    public class FileMap : ClassMap<FileDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileMap" /> class.
        /// </summary>
        public FileMap()
        {
            this.Table("Arquivo");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Name, "Nome");
            this.Map(x => x.MimeType, "TipoArquivo");
            this.Map(x => x.Content, "Conteudo").CustomType<BinaryBlobType>(); ;
            this.Map(x => x.CreateDate, "DataCriacao");
        }
    }
}
