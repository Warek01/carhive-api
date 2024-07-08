using Asp.Versioning;
using FafCarsApi.Dto;
using FafCarsApi.Queries;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize(Roles = "Admin")]
[Route("/Api/v{v:apiVersion}/[controller]")]
public class StatisticsController(StatisticsService statisticsService) : Controller {
  [HttpGet]
  [Route("Market")]
  public async Task<ActionResult<MarketStatisticsDto>> GetMarketStatistics([FromQuery] MarketStatisticsQuery query) {
    return await statisticsService.GenerateMarketStatistics(query);
  }
}
