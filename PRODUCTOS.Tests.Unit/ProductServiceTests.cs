using FluentAssertions;
using MockQueryable.Moq; // <--- Línea vital para solucionar el error
using Moq;
using PRODUCTOS.Application.DTOs.Products;
using PRODUCTOS.Application.Interfaces; // Donde esté definido IApplicationDbContext
using PRODUCTOS.Persistence;
using PRODUCTOS.Persistence.Services;
using Xunit;

namespace PRODUCTOS.Tests.Unit
{
    public class ProductServiceTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            // Creamos el simulacro (Mock) del contexto
            _contextMock = new Mock<IApplicationDbContext>();
            // Inyectamos el mock en el servicio (Inyección de dependencias)
            _service = new ProductService(_contextMock.Object);
        }

        [Fact]
        public async Task GetProduct_FilterBySearchTerm_ShouldReturnOnlyMatchingProducts()
        {
            // 1. ARRANGE (Organizar)
            // Creamos una lista de productos en memoria
            var data = new List<Product>
            {
                new Product { IdProduct = 1, Name = "Laptop Pro", Price = 1000, Description = "Excelente laptop" },
                new Product { IdProduct = 2, Name = "Mouse", Price = 20, Description = "Mouse óptico" },
                new Product { IdProduct = 3, Name = "Laptop Office", Price = 800, Description = "Para oficina" }
            };

            // Convertimos la lista en un Mock que soporta LINQ (Async)
            var mockDbSet = data.AsQueryable().BuildMockDbSet();

            // Configuramos el contexto para que cuando se acceda a Products, devuelva nuestro mock
            _contextMock.Setup(c => c.Products).Returns(mockDbSet.Object);

            var filters = new FiltersDto
            {
                SearchTerm = "Laptop",
                pageNumber = 1,
                pageSize = 10
            };

            // 2. ACT (Actuar)
            var result = await _service.GetProduct(filters);

            // 3. ASSERT (Afirmar)
            result.Should().HaveCount(2); // Debería encontrar 2 laptops
            result.Should().OnlyContain(p => p.Name.Contains("Laptop"));
        }
    }
}