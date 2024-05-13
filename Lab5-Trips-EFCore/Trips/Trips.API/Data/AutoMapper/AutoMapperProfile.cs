using AutoMapper;
using Trips.API.Data.Models;
using Trips.API.Trips.Models.Responses;

namespace Trips.API.Data.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Trip, GetAllTripsResponse>()
            .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.Countries.Select(ct => ct.Name)))
            .ForMember(dest => dest.Clients, opt => opt.MapFrom(src => src.ClientTrips.Select(ct => ct.IdClientNavigation)));
        CreateMap<Country, CountryResponse>();
        CreateMap<Client, ClientResponse>();
    }
}