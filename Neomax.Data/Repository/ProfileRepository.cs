////-----------------------------------------------------------------------
//// <copyright file="ProfileRepository.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Neomax.Data.DataAccess;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Persists Profile data
    /// </summary>
    public class ProfileRepository : BaseRepository<ProfileDao>
    {       
    }
}
