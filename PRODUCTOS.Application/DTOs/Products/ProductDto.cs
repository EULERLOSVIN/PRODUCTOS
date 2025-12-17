namespace PRODUCTOS.Application.DTOs.Products
{
    public class ProductDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<ProductDetailDto> Details { get; set; } = new List<ProductDetailDto>();
    }

    public class ProductDetailDto
    {
        public int IdDetail { get; set; }
        public int Stock { get; set; }
        public StateCartDto? StateCart { get; set; }
        public ColorDto? Color { get; set; }
        public SizeDto? Size { get; set; }
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    }

    public class ColorDto
    {
        public int IdColor { get; set; }
        public string? ColorName { get; set; }
        public string? CodeColor { get; set; }
    }

    public class SizeDto
    {
        public int IdSize { get; set; }
        public string? SizeName { get; set; }
    }

    public class ImageDto { 
        public int IdImage { get; set; }
        public string? Image { get; set; }
        public bool? EsPrincipal { get; set; }
    }

    public class StateCartDto
    {
        public int IdStateCart { get; set; }
        public string? StateName { get; set; }
    }
}