////-----------------------------------------------------------------------
//// <copyright file="UserMap.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.Mapping
{
    using FluentNHibernate.Mapping;
    using PlataformaZ2.Data.DataAccess;

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
            this.Table("[User]");
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Username);
            this.Map(x => x.Password);
            this.Map(x => x.Name);
            this.Map(x => x.Nickname);
            this.Map(x => x.Cpf);;
            this.References(x => x.Photo, "IdPhoto");            
            this.References(x => x.Profile, "IdProfile");          
            this.Map(x => x.AccessToken);
            this.Map(x => x.AccessTokenCreationDate);
            this.Map(x => x.Active);
        }
    }
}
