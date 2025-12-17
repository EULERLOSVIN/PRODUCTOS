using Microsoft.EntityFrameworkCore;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Interfaces;

namespace PRODUCTOS.Persistence.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IApplicationDbContext _context;

        public CatalogService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InitialCatalogDto>> GetFilters()
        {
            var resultado = await _context.Categories
                .AsNoTracking()
                .Select(c => new InitialCatalogDto
                {
                    IdCategory = c.IdCategory,
                    CategoryName = c.Name,
                    SubCategories = c.SubCategories.Select(sc => new SubCategoryDto
                    {
                        IdSubCategory = sc.IdSubcategory,
                        CategoryName = sc.Name,
                        Products = sc.Products.Select(p => new ProductDto
                        {
                            IdProduct = p.IdProduct,
                            Name = p.Name,
                            Description = p.Description ?? string.Empty, // Manejo seguro de nulos
                            Price = p.Price,
                            Details = p.ProductDetails.Select(d => new ProductDetailDto
                            {
                                IdDetail = d.IdDetail,
                                Stock = d.Stock,
                                StateCart = d.IdStateNavigation == null ? null : new StateCartDto
                                {
                                    IdStateCart = d.IdStateNavigation.IdState,
                                    StateName = d.IdStateNavigation.Name
                                },

                                Color = d.IdColorNavigation == null ? null : new ColorDto
                                {
                                    IdColor = d.IdColorNavigation.IdColor,
                                    ColorName = d.IdColorNavigation.Name,
                                    CodeColor = d.IdColorNavigation.CodigoColor
                                },

                                Size = d.IdSizeNavigation == null ? null : new SizeDto
                                {
                                    IdSize = d.IdSizeNavigation.IdSize,
                                    SizeName = d.IdSizeNavigation.Name
                                },

                                Images = d.ImageProducts.Select(img => new ImageDto
                                {
                                    IdImage = img.IdImagen,
                                    Image = img.Image != null
                                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(img.Image)}": null,
                                    EsPrincipal = img.EsPrincipal
                                }).ToList()

                            }).ToList()
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return resultado;
        }
    }
}