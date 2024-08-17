using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Models;
using AutoMapper;

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
    CreateMap<Comment, CommentDto>();

    CreateMap<CreateListingDto, Listing>()
      .ForMember(
        dest => dest.Images,
        opt => opt.Ignore()
      );
  }
}
