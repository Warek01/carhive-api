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

    CreateMap<Listing, ListingDto>()
      .ForMember(dto => dto.PreviewUrl,
        o => o.MapFrom(l => l.PreviewFilename == null
          ? null
          : Path.Combine(StaticFileService.RelativeRequestPath, l.PreviewFilename))
      )
      .ForMember(dto => dto.ImagesUrls,
        o => o.MapFrom(
          l => l.ImagesFilenames.Select(f => Path.Combine(StaticFileService.RelativeRequestPath, f)).ToList()
        )
      );

    CreateMap<CreateListingDto, Listing>()
      .ForMember(
        dest => dest.ImagesFilenames,
        opt => opt.Ignore()
      )
      .ForMember(
        dest => dest.PreviewFilename,
        opt => opt.Ignore()
      );
  }
}
