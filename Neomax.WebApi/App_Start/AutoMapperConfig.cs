using AutoMapper;
using Neomax.Business;
using Neomax.Model;
using Neomax.Data.DataAccess;
using Neomax.Model.Dto;
using System;
using System.Linq;

namespace Neomax.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ClientDto, ClientDao>().ReverseMap();
                cfg.CreateMap<UserDto, UserDao>().ReverseMap();
            });
        }
    }
}