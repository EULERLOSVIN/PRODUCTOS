using System;
using System.Collections.Generic;

namespace PRODUCTOS.Persistence;

public partial class Category
{
    public int IdCategory { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
