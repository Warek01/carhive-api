using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Models;

namespace FafCarsApi.Configurations;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserDto>();
    CreateMap<Listing, ListingDto>();
    CreateMap<Country, CountryDto>();
    CreateMap<Brand, BrandDto>();
  }
}
