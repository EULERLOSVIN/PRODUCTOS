using PRODUCTOS.Application.DTOs.Products;

namespace PRODUCTOS.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<List<InitialCatalogDto>> GetFilters();
    }
}
