//using FluentAssertions;
//using MockQueryable.Moq; // <--- Vital para que FindAsync funcione
//using Moq;
//using PRODUCTOS.Application.DTOs;
//using PRODUCTOS.Application.Features.Products.Commands;
//using PRODUCTOS.Application.Interfaces;
//using PRODUCTOS.Persistence;
//using Xunit;

//namespace PRODUCTOS.Tests.Unit
//{
//    public class UpdateStockHandlerTests
//    {
//        private readonly Mock<IApplicationDbContext> _contextMock;
//        private readonly UpdateStockHandler _handler;

//        public UpdateStockHandlerTests()
//        {
//            _contextMock = new Mock<IApplicationDbContext>();
//            _handler = new UpdateStockHandler(_contextMock.Object);
//        }

//        [Fact]
//        public async Task Handle_WhenStockIsSufficient_ShouldDecreaseStockCorrectly()
//        {
//            // ARRANGE
//            var detailId = 1;
//            var initialStock = 10;
//            var quantityToSubtract = 3;

//            // Creamos la lista que simulará nuestra tabla de base de datos
//            var productDetails = new List<ProductDetail>
//            {
//                new ProductDetail { IdDetail = detailId, Stock = initialStock }
//            };

//            // Construimos el Mock del DbSet que soporta FindAsync
//            var mockDbSet = productDetails.AsQueryable().BuildMockDbSet();

//            // Configuración del comportamiento de FindAsync para que busque en nuestra lista
//            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((object[] ids, CancellationToken ct) =>
//                {
//                    var id = (int)ids[0];
//                    return productDetails.FirstOrDefault(x => x.IdDetail == id);
//                });

//            _contextMock.Setup(x => x.ProductDetails).Returns(mockDbSet.Object);

//            var command = new UpdateStockCommand(new List<ProductStockDto>
//            {
//                new ProductStockDto { IdDetail = detailId, Quantity = quantityToSubtract }
//            });

//            // ACT
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // ASSERT
//            result.Should().BeTrue();
//            productDetails.First().Stock.Should().Be(7); // 10 - 3
//            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//        }

//        [Fact]
//        public async Task Handle_WhenQuantityExceedsStock_ShouldSetStockToZero()
//        {
//            // ARRANGE
//            var detailId = 2;
//            var initialStock = 5;
//            var productDetails = new List<ProductDetail>
//            {
//                new ProductDetail { IdDetail = detailId, Stock = initialStock }
//            };

//            var mockDbSet = productDetails.AsQueryable().BuildMockDbSet();
//            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((object[] ids, CancellationToken ct) =>
//                    productDetails.FirstOrDefault(x => x.IdDetail == (int)ids[0]));

//            _contextMock.Setup(x => x.ProductDetails).Returns(mockDbSet.Object);

//            var command = new UpdateStockCommand(new List<ProductStockDto>
//            {
//                new ProductStockDto { IdDetail = detailId, Quantity = 10 }
//            });

//            // ACT
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // ASSERT
//            result.Should().BeTrue();
//            productDetails.First().Stock.Should().Be(0); // Lógica: si no alcanza, queda en 0
//        }
//    }
//}