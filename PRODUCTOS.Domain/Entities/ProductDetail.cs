namespace PRODUCTOS.Persistence;

public partial class ProductDetail
{
    public int IdDetail { get; set; }
    public int IdProduct { get; set; }
    public int IdSize { get; set; }
    public int IdColor { get; set; }
    public int IdState { get; set; }
    public int Stock { get; set; }
    public virtual Product IdProductNavigation { get; set; } = null!;
    public virtual Size IdSizeNavigation { get; set; } = null!;
    public virtual Color IdColorNavigation { get; set; } = null!;
    public virtual StateProduct IdStateNavigation { get; set; } = null!;

    public virtual ICollection<ImageProduct> ImageProducts { get; set; } = new List<ImageProduct>();
}