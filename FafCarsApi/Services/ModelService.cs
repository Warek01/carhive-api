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

  public async Task AddModel(List<string> brandNames, string modelName) {
    List<Brand> brands = await dbContext.Brands
      .Where(b => brandNames.Contains(b.Name))
      .ToListAsync();

    var model = new Model {
      Name = modelName,
      Brands = brands
    };

    await dbContext.AddAsync(model);
    await dbContext.SaveChangesAsync();
  }
}
