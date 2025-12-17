
using MediatR;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Interfaces;

namespace PRODUCTOS.Application.Features.Products.Query
{
    public record GetProductsByFilterQuery(FiltersDto filter) : IRequest<List<ProductDto>>;

    public class GetProductsByFilterHandler : IRequestHandler<GetProductsByFilterQuery, List<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetProductsByFilterHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductDto>> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProduct(request.filter);
        }
    }

}
