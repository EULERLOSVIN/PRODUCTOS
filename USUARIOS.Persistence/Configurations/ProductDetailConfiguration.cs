using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> entity)
        {
            // PK
            entity.HasKey(e => e.IdDetail);
            entity.ToTable("ProductDetail");

            // Índice compuesto para evitar duplicados de variantes
            entity.HasIndex(e => new { e.IdProduct, e.IdSize, e.IdColor }, "UQ_ProductVariant").IsUnique();

            // Propiedades
            entity.Property(e => e.Stock)
                .IsRequired()
                .HasDefaultValue(0);

            // ELIMINADO: entity.Property(e => e.IdImagen); -> Esto no existe en la tabla ProductDetail del diagrama

            // Relaciones

            // Color
            entity.HasOne(d => d.IdColorNavigation)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.IdColor)
                .OnDelete(DeleteBehavior.Restrict) // Cambiado a Restrict para mayor seguridad
                .HasConstraintName("FK_ProductDetail_Color");

            // Producto
            entity.HasOne(d => d.IdProductNavigation)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductDetail_Product");

            // Talla (Size)
            entity.HasOne(d => d.IdSizeNavigation)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.IdSize)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductDetail_Size");

            // Estado
            entity.HasOne(d => d.IdStateNavigation)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.IdState)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductDetail_StateProduct");
        }
    }
}