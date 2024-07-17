using Asp.Versioning;
using FafCarsApi.Dto;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/Model")]
public class ModelController(ModelService modelService) : Controller {
  [HttpGet]
  public async Task<ActionResult<List<string>>> GetModels([FromQuery] string brandName) {
    List<Model>? models = await modelService.GetBrandModels(brandName);

    if (models == null) {
      return NotFound();
    }

    return models.Select(m => m.Name).ToList();
  }
}
