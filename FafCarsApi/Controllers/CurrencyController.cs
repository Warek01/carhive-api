using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/Currency")]
public class CurrencyController(CurrencyService currencyService) : Controller {
  [HttpGet]
  public async Task<ActionResult<Currency>> GetLatestCurrency() {
    return await currencyService.GetCurrentCurrency();
  }
}