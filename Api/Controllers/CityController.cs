using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[AllowAnonymous]
[ApiController]
[ApiVersion(1)]
[Route("Api/v{v:apiVersion}/City")]
public class CityController(CityService cityService) : Controller {
  [HttpGet]
  public async Task<ActionResult<List<string>>> GetCities([FromQuery] string countryCode) {
    List<string> cities = await cityService.GetCountryCities(countryCode.ToUpper());
    return cities;
  }

  [HttpGet]
  [Route("Count")]
  public async Task<ActionResult<int>> GetCitiesCount() {
    return await cityService.GetCitiesCount();
  }
}
