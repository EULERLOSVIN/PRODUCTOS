
namespace PRODUCTOS.Application.DTOs.Products
{
    public class FiltersDto
    {
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public List<int>? SubCategoryIds { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<int>? SizeIds { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? StateId { get; set; }
        public string? SortBy { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
} 
