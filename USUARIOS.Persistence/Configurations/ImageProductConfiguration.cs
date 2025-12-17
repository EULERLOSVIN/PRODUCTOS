using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class ImageProductConfiguration : IEntityTypeConfiguration<ImageProduct>
    {
        public void Configure(EntityTypeBuilder<ImageProduct> entity)
        {
            entity.HasKey(e => e.IdImagen);

            entity.ToTable("ImageProduct");

            entity.Property(e => e.Image)
                .IsRequired()
                .HasColumnType("varbinary(max)");

            entity.Property(e => e.EsPrincipal)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(d => d.IdProductDetailNavigation)
                .WithMany(p => p.ImageProducts)
                .HasForeignKey(d => d.IdProductDetail)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ImageProduct_ProductDetail");
        }
    }
}