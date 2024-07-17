using Asp.Versioning;
using FafCarsApi.Helpers;
using FafCarsApi.Models;
using FafCarsApi.Queries;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/Brand")]
public class BrandController(
  BrandService brandService
) : Controller {
  [HttpGet]
  public async Task<ActionResult<List<string>>> GetBrands([FromQuery] BrandQuery query) {
    IEnumerable<Brand> brands = await brandService.GetBrands();

    if (query.CountryCode != null) {
      brands = brands.Where(b => b.CountryCode == query.CountryCode);
    }

    List<string> brandNames = brands.Select(b => b.Name).ToList();

    if (query.Search != null) {
      brandNames = FuzzySearchHelper.Sort(brandNames, query.Search)
        .Take(10)
        .ToList();
    }

    return brandNames;
  }

  [HttpPost]
  public async Task<ActionResult> AddBrand([FromBody] string brandName) {
    await brandService.AddBrand(brandName);
    return Created();
  }
}
