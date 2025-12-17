namespace PRODUCTOS.Persistence;

public partial class StateProduct
{
    public int IdState { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}