using AutoMapper;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;

namespace FafCarsApi.Models;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserDto>().ReverseMap();
    CreateMap<Listing, ListingDto>().ReverseMap();
  }
}
