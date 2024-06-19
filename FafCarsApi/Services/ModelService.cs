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

  public async Task<Model?> AddModel(string brandName, string modelName) {
    Brand? brand = await dbContext.Brands
      .Where(b => b.Name == brandName)
      .FirstOrDefaultAsync();

    if (brand == null)
      return null;
    
    var model = new Model {
      Name = modelName,
      Brand = brand
    };
    
    await dbContext.AddAsync(model);
    await dbContext.SaveChangesAsync();

    return model;
  }
}
