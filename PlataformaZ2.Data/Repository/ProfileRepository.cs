////-----------------------------------------------------------------------
//// <copyright file="ProfileRepository.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using PlataformaZ2.Data.DataAccess;
    using PlataformaZ2.Model.Dto;
    using PlataformaZ2.Model.Exception;
    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Persists Profile data
    /// </summary>
    public class ProfileRepository : BaseRepository<ProfileDao>
    {       
    }
}
