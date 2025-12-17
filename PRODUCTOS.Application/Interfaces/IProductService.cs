
using PRODUCTOS.Application.DTOs.Products;

namespace PRODUCTOS.Application.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProduct(FiltersDto request);
    }
}
