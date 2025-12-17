using MediatR;
using Microsoft.AspNetCore.Mvc;
using PRODUCTOS.Application.Features.Category.Query;

namespace PRODUCTOS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var listCategories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(listCategories);
        }
       
    }
}
