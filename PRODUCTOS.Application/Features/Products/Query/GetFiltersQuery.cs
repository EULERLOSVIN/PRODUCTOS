using MediatR;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Interfaces;

namespace PRODUCTOS.Application.Features.Products.Query
{
    public record GetFiltersQuery() : IRequest<List<InitialCatalogDto>>;
    public class GetFiltersHandler : IRequestHandler<GetFiltersQuery, List<InitialCatalogDto>>
    {
        private readonly ICatalogService _catalogService;
        public GetFiltersHandler(ICatalogService filtersService)
        {
            _catalogService = filtersService;
        }

        public async Task<List<InitialCatalogDto>> Handle(GetFiltersQuery request, CancellationToken cancellationToken)
        {
            return await _catalogService.GetFilters();
        }
    }
}