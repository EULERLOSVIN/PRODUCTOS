using MediatR;
using PRODUCTOS.Application.DTOs;
using PRODUCTOS.Application.Interfaces;


namespace PRODUCTOS.Application.Features.Products.Commands
{
    public record UpdateStockCommand(List<ProductStockDto> Items) : IRequest<bool>;

    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateStockHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any()) return false;

            foreach (var item in request.Items)
            {
                Console.WriteLine($"[DEBUG STOCK] Intentando descontar {item.Quantity} de la Variante ID: {item.IdDetail}");
                var productDetail = await _context.ProductDetails
                    .FindAsync(item.IdDetail, cancellationToken);

                if (productDetail != null)
                {
                    if (productDetail.Stock >= item.Quantity)
                    {
                        productDetail.Stock -= item.Quantity;
                    }
                    else
                    {
                        productDetail.Stock = 0;
                    }

                    _context.ProductDetails.Update(productDetail);
                }
                else if (item.IdDetail != 0)
                {
                    Console.WriteLine($"[DEBUG STOCK] Advertencia: No se encontró la variante {item.IdDetail} en la base de datos.");
                }
                else
                {
                    Console.WriteLine($"[DEBUG STOCK] ¡FALLO DE DATOS! El IdDetail recibido es 0. Fallo en el mapeo desde el servicio de Ventas.");
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}