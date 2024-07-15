using AutoMapper;
using FafCarsApi.Dto;
using FafCarsApi.Models;
using FafCarsApi.Services;

namespace FafCarsApi.Config;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserAdminDto>();
    CreateMap<User, UserDto>();
    CreateMap<Country, CountryDto>();
    CreateMap<Brand, BrandDto>();
    CreateMap<RegisterDto, User>();
    CreateMap<CreateUserDto, User>();

    CreateMap<Listing, ListingDto>()
      .ForMember(dto => dto.ImagesUrls,
        o => o.MapFrom(
          l => l.Images.Select(f => Path.Combine(StaticFileService.RelativeRequestPath, f)).ToList()
        )
      );

    CreateMap<CreateListingDto, Listing>()
      .ForMember(
        dest => dest.Images,
        opt => opt.Ignore()
      );
  }
}
