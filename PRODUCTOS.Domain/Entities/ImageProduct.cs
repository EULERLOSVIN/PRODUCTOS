using System;
using System.Collections.Generic;

namespace PRODUCTOS.Persistence;

public partial class ImageProduct
{
    public int IdImagen { get; set; }
    public int IdProductDetail { get; set; }
    public byte[] Image { get; set; } = null!;
    public bool EsPrincipal { get; set; }

    public virtual ProductDetail IdProductDetailNavigation { get; set; } = null!;
}