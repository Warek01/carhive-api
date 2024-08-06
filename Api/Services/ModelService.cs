using Api.Data;
using Api.Dtos.Request;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ModelService(CarHiveDbContext dbContext) {
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
