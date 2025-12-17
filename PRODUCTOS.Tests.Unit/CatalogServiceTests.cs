using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using PRODUCTOS.Application.Interfaces;
using PRODUCTOS.Persistence;
using PRODUCTOS.Persistence.Services;
using Xunit;

namespace PRODUCTOS.Tests.Unit
{
    public class CatalogServiceTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly CatalogService _service;

        public CatalogServiceTests()
        {
            // 1. ARRANGE: Mock del contexto para aislamiento total (Prueba Unitaria Pura)
            _contextMock = new Mock<IApplicationDbContext>();
            _service = new CatalogService(_contextMock.Object);
        }

        [Fact]
        public async Task GetFilters_ShouldReturnMappedCatalog_WithoutUsingDatabase()
        {
            // ARRANGE: Datos simulados con jerarquía completa para evitar NullReferenceException
            var categories = CreateFakeCatalogData();
            var mockDbSet = categories.AsQueryable().BuildMockDbSet();

            _contextMock.Setup(x => x.Categories).Returns(mockDbSet.Object);

            // ACT
            var result = await _service.GetFilters();

            // ASSERT: Validaciones paso a paso para evitar advertencias de nulidad
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();

            var firstCategory = result.First();
            firstCategory.CategoryName.Should().Be("Tecnología");

            // Validación segura de Subcategorías
            firstCategory.SubCategories.Should().NotBeNull();
            var firstSub = firstCategory.SubCategories!.First();

            // Validación segura de Productos
            firstSub.Products.Should().NotBeNull();
            var firstProduct = firstSub.Products!.First();
            firstProduct.Name.Should().Be("Laptop Pro");

            // Validación segura de Detalles e Imágenes (conversión Base64)
            firstProduct.Details.Should().NotBeNull();
            var firstDetail = firstProduct.Details!.First();

            firstDetail.Images.Should().NotBeNull();
            firstDetail.Images!.First().Image.Should().StartWith("data:image/jpeg;base64,");
        }

        private List<Category> CreateFakeCatalogData()
        {
            // Inicialización de entidades según el diagrama de base de datos
            var image = new ImageProduct
            {
                IdImagen = 1,
                Image = new byte[] { 0x20, 0x21, 0x22 },
                EsPrincipal = true
            };

            var detail = new ProductDetail
            {
                IdDetail = 1,
                Stock = 50,
                // Objetos de navegación requeridos por el Select del servicio
                IdColorNavigation = new Color { IdColor = 1, Name = "Negro" },
                IdSizeNavigation = new Size { IdSize = 1, Name = "L" },
                IdStateNavigation = new StateProduct { IdState = 1, Name = "Disponible" },
                ImageProducts = new List<ImageProduct> { image }
            };

            var product = new Product
            {
                IdProduct = 1,
                Name = "Laptop Pro",
                ProductDetails = new List<ProductDetail> { detail }
            };

            var subCategory = new SubCategory
            {
                IdSubcategory = 1,
                Name = "Laptops",
                Products = new List<Product> { product }
            };

            return new List<Category>
            {
                new Category
                {
                    IdCategory = 1,
                    Name = "Tecnología",
                    SubCategories = new List<SubCategory> { subCategory }
                }
            };
        }
    }
}