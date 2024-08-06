using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/Currency")]
public class CurrencyController(CurrencyService currencyService) : Controller {
  [HttpGet]
  public async Task<ActionResult<double>> GetLatestCurrency([FromQuery] string code) {
    double? ratio = await currencyService.GetCurrency(code.ToUpper());

    if (ratio == null) {
      return NotFound();
    }

    return ratio;
  }
}
