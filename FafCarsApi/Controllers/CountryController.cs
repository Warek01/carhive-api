using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FafCarsApi.Dto;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[Route("Api/v{v:apiVersion}/[controller]")]
public class CountryController(
  IMapper mapper,
  CountryService countryService
) : Controller {
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<CountryDto>>> GetCountries() {
    List<Country> countries = await countryService.GetCountries();
    List<CountryDto> dtos = countries.Select(mapper.Map<CountryDto>).ToList();
    var result = new PaginatedResultDto<CountryDto> {
      Items = dtos,
      TotalItems = dtos.Count,
    };

    return result;
  }

  [HttpGet]
  [Route("{code}/Brands")]
  public async Task<ActionResult<PaginatedResultDto<BrandDto>>> GetCountryBrands([StringLength(2)] string code) {
    List<Brand> brands = await countryService.GetCountryBrands(code);
    List<BrandDto> dtos = brands.Select(mapper.Map<BrandDto>).ToList();

    return new PaginatedResultDto<BrandDto> {
      Items = dtos,
      TotalItems = dtos.Count,
    };
  }
}