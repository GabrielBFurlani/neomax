////-----------------------------------------------------------------------
//// <copyright file="ProfileManager.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Util;
    using Neomax.Data.DataAccess;
    using Neomax.Data.Repository;
    using Neomax.Model.Dto;
    using Neomax.Model.Util;
    using Neomax.Model.Exception;
    using System.IO;
    using Neomax.Business.Util;

    /// <summary>
    /// Manages business rules related to profile
    /// </summary>
    public static class ProfileManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns>List of Profiles</returns>
        public static IList<ProfileDto> GetAllProfiles()
        {
            ProfileRepository profileRepository = new ProfileRepository();

            List<ProfileDto> profileList = new List<ProfileDto>();

            //// Get all profiles
            var profileDaoList = profileRepository.GetAll();

            //// Convert from DAO to DTO
            foreach (var profileDao in profileDaoList)
            {
                profileList.Add(new ProfileDto() { Id = profileDao.Id, Name = profileDao.Name });
            }

            return profileList;
        }
    }
}
