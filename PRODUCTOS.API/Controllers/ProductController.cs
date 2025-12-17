using MediatR;
using Microsoft.AspNetCore.Mvc;
using PRODUCTOS.Application.DTOs;
using PRODUCTOS.Application.Features.Products.Commands;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Features.Products.Query;

namespace PRODUCTOS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("filtros-catalogo")]
        public async Task<ActionResult<List<FiltersDto>>> GetFilters()
        {
            var query = new GetFiltersQuery();
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetProductsByFilters([FromBody] FiltersDto filter)
        {
            var query = new GetProductsByFilterQuery(filter);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("reduce-stock")]
        public async Task<IActionResult> ReduceStock([FromBody] List<ProductStockDto> items)
        {
            var command = new UpdateStockCommand(items);
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { Message = "Stock actualizado correctamente" });
            }
            else
            {
                return BadRequest("No se pudo actualizar el stock o la lista estaba vacía.");
            }
        }

    }
}
