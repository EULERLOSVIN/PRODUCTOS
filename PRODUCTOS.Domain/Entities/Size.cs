using System;
using System.Collections.Generic;

namespace PRODUCTOS.Persistence;

public partial class Size
{
    public int IdSize { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
