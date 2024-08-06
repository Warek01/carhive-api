using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class BrandService(CarHiveDbContext dbContext) {
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
