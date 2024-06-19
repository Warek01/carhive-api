using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Models;
using FafCarsApi.Services;

namespace FafCarsApi.Configurations;

public class MappingProfile : Profile {
  public MappingProfile() {
    CreateMap<User, UserDto>();
    CreateMap<Listing, ListingDto>()
      .ForMember(dto => dto.PreviewUrl,
        o => o.MapFrom(l => l.PreviewFilename == null
          ? null
          : Path.Combine(StaticFileService.RequestPath, l.PreviewFilename))
      )
      .ForMember(dto => dto.ImagesUrls,
        o => o.MapFrom(
          l => l.ImagesFilenames.Select(f => Path.Combine(StaticFileService.RequestPath, f)).ToList()
        )
      );
    CreateMap<Country, CountryDto>();
    CreateMap<Brand, BrandDto>();
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
