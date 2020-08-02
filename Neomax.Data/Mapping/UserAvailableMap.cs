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

    /// <summary>
    /// Mapping for File model
    /// </summary>
    public class UserAvailableMap : ClassMap<UserAvailableDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAvailableMap" /> class.
        /// </summary>
        public UserAvailableMap()
        {
            this.Table("UsuarioDisponivel");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Email, "Email");
            this.Map(x => x.Token, "Token");
        }
    }
}