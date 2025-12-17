using MediatR;
using Microsoft.EntityFrameworkCore;
using PRODUCTOS.Application.DTOs.Category;
using PRODUCTOS.Application.Interfaces;

namespace PRODUCTOS.Application.Features.Category.Query
{
    public record GetAllCategoriesQuery(): IRequest<List<CategoryDto>>;

    public class GetAllCategoriesHandler: IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllCategoriesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.IdCategory)
                .ToListAsync(cancellationToken);

            // Mapeamos a DTOs
            var listCategories = categories.Select(c => new CategoryDto
            {
                IdCategory = c.IdCategory,
                Name = c.Name
            }).ToList();

            return listCategories;
        }
    }
}
