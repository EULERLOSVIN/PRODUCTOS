namespace PRODUCTOS.Persistence;

public partial class Product
{
    public int IdProduct { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int IdSubcategory { get; set; }
    public DateTime RegistrationDate { get; set; }

    public virtual SubCategory IdSubcategoryNavigation { get; set; } = null!;
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}