////-----------------------------------------------------------------------
//// <copyright file="SolicitationProductRepository.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
    using Neomax.Model.Util;
    using AutoMapper;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Persists Profile data
    /// </summary>
    public class SolicitationProductRepository : BaseRepository<SolicitationProductDao>
    {

    }
}
