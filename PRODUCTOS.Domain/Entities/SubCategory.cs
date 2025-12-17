using System;
using System.Collections.Generic;

namespace PRODUCTOS.Persistence;

public partial class SubCategory
{
    public int IdSubcategory { get; set; }
    public string Name { get; set; } = null!;
    public byte[]? Image { get; set; }
    public int IdCategory { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
