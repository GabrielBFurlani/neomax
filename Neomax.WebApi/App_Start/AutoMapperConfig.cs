using AutoMapper;
using Neomax.Business;
using Neomax.Model;
using Neomax.Data.DataAccess;
using Neomax.Model.Dto;
using System;
using System.Linq;
using Neomax.Model.Util;

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
                cfg.CreateMap<SolicitationDao, SolicitationDto>()
                .ForMember(x => x.StatusName, y => y.MapFrom(z => Domain.TextValueFrom(z.Status)));

                cfg.CreateMap<SolicitationProductDao, SolicitationProductDto>()
                .ForMember(x => x.StatusName, y => y.MapFrom(z => Domain.TextValueFrom(z.Status)));
                ;
            });
        }
    }
}