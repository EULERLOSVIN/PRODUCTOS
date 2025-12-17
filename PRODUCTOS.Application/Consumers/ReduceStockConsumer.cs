using MassTransit;
using MediatR;
using SALES.Application.Common.Events;
using PRODUCTOS.Application.Features.Products.Commands;
using PRODUCTOS.Application.DTOs;

namespace PRODUCTOS.Application.Consumers
{
    public class ReduceStockConsumer : IConsumer<SaleCreatedEvent>
    {
        private readonly IMediator _mediator;

        public ReduceStockConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<SaleCreatedEvent> context)
        {
            // Mapeamos los datos del evento de integración al Command de MediatR
            var items = context.Message.Items.Select(i => new ProductStockDto
            {
                IdDetail = i.IdDetail,
                Quantity = i.Quantity
            }).ToList();

            // Enviamos el comando a tu UpdateStockHandler existente
            await _mediator.Send(new UpdateStockCommand(items));
        }
    }
}