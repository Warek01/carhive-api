using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Models;
using Api.Services;
using AutoMapper;
using Api.Dtos;

namespace Api.Config;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserAdminDto>();
    CreateMap<User, UserDto>();
    CreateMap<Country, CountryDto>();
    CreateMap<Brand, BrandDto>();
    CreateMap<RegisterDto, User>();
    CreateMap<CreateUserDto, User>();
    CreateMap<CreateReportDto, Report>();

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
