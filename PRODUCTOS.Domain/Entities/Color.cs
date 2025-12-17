using System;
using System.Collections.Generic;

namespace PRODUCTOS.Persistence;

public partial class Color
{
    public int IdColor { get; set; }
    public string Name { get; set; } = null!;
    public string CodigoColor { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
