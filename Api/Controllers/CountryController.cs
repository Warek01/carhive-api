using Api.Dtos.Response;
using Api.Models;
using Api.Services;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
[ApiVersion(1)]
[Route("Api/v{v:apiVersion}/Country")]
public class CountryController(
  IMapper mapper,
  CountryService countryService
) : Controller {
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<CountryDto>>> GetCountries() {
    List<Country> countries = await countryService.GetSupportedCountries();
    List<CountryDto> dtos = countries.Select(mapper.Map<CountryDto>).ToList();
    var result = new PaginatedResultDto<CountryDto> {
      Items = dtos,
      TotalItems = dtos.Count,
    };

    return result;
  }

  [HttpGet]
  [Route("Count")]
  public async Task<ActionResult<int>> GetSupportedCountriesCount() {
    return await countryService.GetSupportedCountriesCount();
  }
}
