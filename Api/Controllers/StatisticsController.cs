using Api.Dtos.Response;
using Api.Helpers;
using Api.Queries;
using Api.Services;
using Asp.Versioning;
using Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
