using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.ViewModels;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoVm>()
                .ForMember(dest => dest.Palestrantes, opt =>
                {
                    opt.MapFrom(src => src.PalestranteEventos.Select(x => x.Palestrante).ToList());
                }).ReverseMap();
            CreateMap<Palestrante, PalestranteVm>()
                .ForMember(dest => dest.Eventos, opt =>
                {
                    opt.MapFrom(src => src.PalestranteEventos.Select(x => x.Evento).ToList());
                }).ReverseMap();
            CreateMap<Lote, LoteVm>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialVm>().ReverseMap();

            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<User, UserLoginVm>().ReverseMap();
        }
    }
}