using Api.Dtos.Request;
using Api.Models;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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

  [HttpPost]
  public async Task<ActionResult> CreateModel(CreateModelDto dto) {
    await modelService.CreateModel(dto);
    return Created();
  }
}
