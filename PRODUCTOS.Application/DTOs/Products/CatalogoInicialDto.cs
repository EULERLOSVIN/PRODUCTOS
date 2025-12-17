namespace PRODUCTOS.Application.DTOs.Products
{
    public class InitialCatalogDto
    {
        public int IdCategory { get; set; }
        public string? CategoryName { get; set; }
        public List<SubCategoryDto>? SubCategories { get; set; }
    }

    public class SubCategoryDto
    {
        public int IdSubCategory { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}