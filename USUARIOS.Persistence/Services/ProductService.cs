using Microsoft.EntityFrameworkCore;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Interfaces;

namespace PRODUCTOS.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbContext _context;

        public ProductService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetProduct(FiltersDto request)
        {
            int pageNumber = request.pageNumber <= 0 ? 1 : request.pageNumber;
            int pageSize = request.pageSize <= 0 ? 10 : request.pageSize;

            var query = _context.Products.AsNoTracking().AsQueryable();
            // ---------------------------------------------------------
            // APLICACIÓN DE FILTROS
            // ---------------------------------------------------------

            // A) Categoría
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.IdSubcategoryNavigation.IdCategory == request.CategoryId);
            }

            // B) Búsqueda de Texto (Nombre o Descripción)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.Trim();
                query = query.Where(p => p.Name.Contains(term) || (p.Description != null && p.Description.Contains(term)));
            }

            // C) Subcategorías
            if (request.SubCategoryIds != null && request.SubCategoryIds.Any(id => id > 0))
            {
                var validSubIds = request.SubCategoryIds.Where(id => id > 0).ToList();
                query = query.Where(p => validSubIds.Contains(p.IdSubcategory));
            }

            // D) Rango de Precios
            if (request.MinPrice.HasValue && request.MinPrice.Value > 0)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue && request.MaxPrice.Value > 0)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // E) Estado (Ahora el estado está en el detalle, pero a veces se filtra si AL MENOS UN detalle tiene ese estado)
            if (request.StateId.HasValue && request.StateId.Value > 0)
            {
                query = query.Where(p => p.ProductDetails.Any(pd => pd.IdState == request.StateId.Value));
            }

            // F) Colores (Filtra si el producto tiene al menos una variante con ese color)
            if (request.ColorIds != null && request.ColorIds.Any(id => id > 0))
            {
                var validColorIds = request.ColorIds.Where(id => id > 0).ToList();
                query = query.Where(p => p.ProductDetails.Any(pd => validColorIds.Contains(pd.IdColor)));
            }

            // G) Tallas
            if (request.SizeIds != null && request.SizeIds.Any(id => id > 0))
            {
                var validSizeIds = request.SizeIds.Where(id => id > 0).ToList();
                query = query.Where(p => p.ProductDetails.Any(pd => validSizeIds.Contains(pd.IdSize)));
            }

            // ---------------------------------------------------------
            // 4. ORDENAMIENTO
            // ---------------------------------------------------------
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "price_asc": query = query.OrderBy(p => p.Price); break;
                    case "price_desc": query = query.OrderByDescending(p => p.Price); break;
                    case "name_asc": query = query.OrderBy(p => p.Name); break;
                    default: query = query.OrderBy(p => p.Name); break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.IdProduct);
            }

            // 5. Paginación
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // ---------------------------------------------------------
            // 6. PROYECCIÓN Y MAPEO
            // ---------------------------------------------------------
            var dataRaw = await query.Select(p => new
            {
                p.IdProduct,
                p.Name,
                p.Description,
                p.Price,
                RawDetails = p.ProductDetails.Select(pd => new
                {
                    pd.IdDetail,
                    pd.Stock,
                    StateId = pd.IdStateNavigation.IdState,
                    StateName = pd.IdStateNavigation.Name,
                    ColorData = new { pd.IdColorNavigation.IdColor, pd.IdColorNavigation.Name, pd.IdColorNavigation.CodigoColor },
                    SizeData = new { pd.IdSizeNavigation.IdSize, pd.IdSizeNavigation.Name },
                    Images = pd.ImageProducts.Select(img => new
                    {
                        img.IdImagen,
                        img.Image,
                        img.EsPrincipal
                    }).ToList()
                }).ToList()
            }).ToListAsync();

            var result = dataRaw.Select(item => new ProductDto
            {
                IdProduct = item.IdProduct,
                Name = item.Name,
                Description = item.Description ?? string.Empty,
                Price = item.Price,

                Details = item.RawDetails.Select(d => new ProductDetailDto
                {
                    IdDetail = d.IdDetail,
                    Stock = d.Stock,
                    Color = new ColorDto
                    {
                        IdColor = d.ColorData.IdColor,
                        ColorName = d.ColorData.Name,
                        CodeColor = d.ColorData.CodigoColor
                    },
                    Size = new SizeDto
                    {
                        IdSize = d.SizeData.IdSize,
                        SizeName = d.SizeData.Name
                    },
                    StateCart = new StateCartDto
                    {
                        IdStateCart = d.StateId,
                        StateName = d.StateName
                    },
                    Images = d.Images.Select(img => new ImageDto
                    {
                        IdImage = img.IdImagen,
                        EsPrincipal = img.EsPrincipal,
                        Image = img.Image != null
                            ? $"data:image/jpeg;base64,{Convert.ToBase64String(img.Image)}"
                            : null
                    }).ToList()

                }).ToList()
            }).ToList();

            return result;
        }
    }
}