using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Models;

namespace FafCarsApi.Configurations;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserDto>().ReverseMap();
    CreateMap<Listing, ListingDto>().ReverseMap();
  }
}
