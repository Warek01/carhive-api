using Asp.Versioning;
using AutoMapper;
using FafCarsApi.Helpers;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("Api/v{v:apiVersion}/[controller]")]
public class BrandController(
  BrandService brandService,
  CacheService cache
) : Controller {
  [HttpGet]
  public async Task<ActionResult<List<string>>> GetBrands([FromQuery] string? search) {
    List<string> brandNames;

    if (cache.BrandNamesCache != null) {
      brandNames = cache.BrandNamesCache;
    }
    else {
      List<Brand> brands = await brandService.GetBrands();
      brandNames = brands.Select(b => b.Name).ToList();
      cache.BrandNamesCache = brandNames;
    }

    if (search != null) {
      brandNames = FuzzySearchHelper.Sort(brandNames, search)
        .Take(10)
        .ToList();
    }

    return brandNames;
  }

  [HttpPost]
  public async Task<ActionResult> AddBrand([FromBody] string brandName) {
    await brandService.AddBrand(brandName);
    cache.BrandNamesCache = null;
    return Created();
  }
}
