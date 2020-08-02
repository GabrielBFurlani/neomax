////-----------------------------------------------------------------------
//// <copyright file="UserMap.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using Neomax.Data.DataAccess;

    /// <summary>
    /// Mapping for User model
    /// </summary>
    public class UserMap : ClassMap<UserDao>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMap" /> class.
        /// </summary>
        public UserMap()
        {
            this.Table("Usuario");
            this.Id(x => x.Id, "Codigo").GeneratedBy.Identity();
            this.Map(x => x.Username, "Usuario");
            this.Map(x => x.Password, "Senha");
            this.Map(x => x.Name, "Nome");
            this.Map(x => x.Nickname, "Nickname");
            this.Map(x => x.Email, "Email");
            this.References(x => x.Photo, "CodigoFoto");            
            this.References(x => x.Client, "CodigoCliente");          
            this.Map(x => x.AccessToken, "TokenAcesso");
            this.Map(x => x.AccessTokenCreationDate, "TokenAcessoDataCriacao");
            this.Map(x => x.Active, "Ativo");
        }
    }
}
