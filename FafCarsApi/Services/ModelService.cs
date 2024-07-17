using FafCarsApi.Data;
using FafCarsApi.Dtos.Request;
using FafCarsApi.Exceptions;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class ModelService(FafCarsDbContext dbContext) {
  public async Task<List<Model>?> GetBrandModels(string brandName) {
    Brand? brand = await dbContext.Brands
      .AsNoTracking()
      .Include(b => b.Models)
      .Where(b => b.Name == brandName)
      .FirstOrDefaultAsync();

    return brand?.Models;
  }

  public async Task CreateModel(CreateModelDto dto) {
    Brand? brand = await dbContext.Brands
      .Where(b => b.Name == dto.BrandName)
      .FirstOrDefaultAsync();

    if (brand == null) {
      throw new NotFoundException("brand not found");
    }

    var model = new Model {
      Name = dto.Name,
      Brand = brand
    };

    await dbContext.AddAsync(model);
    await dbContext.SaveChangesAsync();
  }
}
