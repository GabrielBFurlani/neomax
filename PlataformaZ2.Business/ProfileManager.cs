////-----------------------------------------------------------------------
//// <copyright file="ProfileManager.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Util;
    using PlataformaZ2.Data.DataAccess;
    using PlataformaZ2.Data.Repository;
    using PlataformaZ2.Model.Dto;
    using PlataformaZ2.Model.Util;
    using PlataformaZ2.Model.Exception;
    using System.IO;
    using PlataformaZ2.Business.Util;

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
