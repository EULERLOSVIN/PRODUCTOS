using System.Collections.Generic;

namespace SALES.Application.Common.Events
{
    // Usamos 'public record' para que MassTransit pueda leer los datos correctamente
    public record SaleCreatedEvent
    {
        public int IdSale { get; init; }
        public int IdCart { get; init; }
        public List<SaleItemDto> Items { get; init; } = new();
    }

    // Definimos el DTO dentro del mismo namespace para evitar errores de referencia
    public record SaleItemDto
    {
        public int IdDetail { get; init; }
        public int Quantity { get; init; }
    }
}