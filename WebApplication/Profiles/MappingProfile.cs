using AutoMapper;
using Binance.Net.Interfaces;
using WebApp.DTOs;
using WebApp.Models;

namespace WebApp.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<IBinanceKline, KlineDTO>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => ((DateTimeOffset)src.OpenTime).ToUnixTimeSeconds()))
            .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.OpenPrice))
            .ForMember(dest => dest.High, opt => opt.MapFrom(src => src.HighPrice))
            .ForMember(dest => dest.Low, opt => opt.MapFrom(src => src.LowPrice))
            .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.ClosePrice))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume));

        CreateProjection<KlineData, KlineDTO>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => ((DateTimeOffset)src.OpenTime).ToUnixTimeSeconds()))
            .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.OpenPrice))
            .ForMember(dest => dest.High, opt => opt.MapFrom(src => src.HighPrice))
            .ForMember(dest => dest.Low, opt => opt.MapFrom(src => src.LowPrice))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
            .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.ClosePrice));

        CreateMap<KlineData, KlineDTO>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => ((DateTimeOffset)src.OpenTime).ToUnixTimeSeconds()))
            .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.OpenPrice))
            .ForMember(dest => dest.High, opt => opt.MapFrom(src => src.HighPrice))
            .ForMember(dest => dest.Low, opt => opt.MapFrom(src => src.LowPrice))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
            .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.ClosePrice));
    }
}
