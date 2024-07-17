using Asp.Versioning;
using FafCarsApi.Dtos;
using FafCarsApi.Dtos.Response;
using FafCarsApi.Helpers;
using FafCarsApi.Queries;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize(Roles = AuthRoles.Admin)]
[Route("/Api/v{v:apiVersion}/Statistics")]
public class StatisticsController(StatisticsService statisticsService) : Controller {
  [HttpGet]
  [Route("Market")]
  public async Task<ActionResult<MarketStatisticsDto>> GetMarketStatistics([FromQuery] MarketStatisticsQuery query) {
    if (query.IncludeStats && (query.Year == null || query.Month == null)) {
      return BadRequest("year and month should not be null");
    }

    return await statisticsService.GenerateMarketStatistics(query);
  }
}
