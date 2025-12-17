using Microsoft.EntityFrameworkCore;
using PRODUCTOS.Persistence;


namespace PRODUCTOS.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; }
        DbSet<Color> Colors { get; }
        DbSet<ImageProduct> ImageProducts { get; }
        DbSet<Product> Products { get; }
        DbSet<ProductDetail> ProductDetails { get; }
        DbSet<Size> Sizes { get; }
        DbSet<SubCategory> SubCategories { get;}
        DbSet<StateProduct> StateProducts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
