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
                cfg.CreateMap<ClientDto, ClientDao>().ReverseMap()
                .ForMember(x => x.GenderName, y => y.MapFrom(z => Domain.TextValueFrom(z.Gender)))
                .ForMember(x => x.AnnualBillingName, y => y.MapFrom(z => Domain.TextValueFrom(z.AnnualBilling)))
                .ForMember(x => x.NatureBackgroundName, y => y.MapFrom(z => Domain.TextValueFrom(z.NatureBackground)))
                .ForMember(x => x.TypeNoteEmitedName, y => y.MapFrom(z => Domain.TextValueFrom(z.TypeNoteEmited)));

                cfg.CreateMap<ContactDayDto, ContactDayDao>().ReverseMap()
                .ForMember(x => x.ContactDayName, y => y.MapFrom(z => Domain.TextValueFrom(z.ContactDay)));

                cfg.CreateMap<ContactTimeDto, ContactTimeDao>().ReverseMap()
                .ForMember(x => x.ContactTimeName, y => y.MapFrom(z => Domain.TextValueFrom(z.ContactTime)));

                cfg.CreateMap<TelephoneDto, TelephoneDao>().ReverseMap()
                .ForMember(x => x.TelephoneTypeName, y => y.MapFrom(z => Domain.TextValueFrom(z.TelephoneType)));

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