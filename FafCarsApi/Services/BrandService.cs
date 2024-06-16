using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class BrandService(FafCarsDbContext dbContext) {
  public async Task<List<Brand>> GetBrands() {
    return await dbContext.Brands
      .AsNoTracking()
      .OrderBy(b => b.Name)
      .ToListAsync();
  }

  public async Task AddBrand(string brandName) {
    var brand = new Brand {
      Name = brandName,
    };

    await dbContext.Brands.AddAsync(brand);
    await dbContext.SaveChangesAsync();
  }
}
