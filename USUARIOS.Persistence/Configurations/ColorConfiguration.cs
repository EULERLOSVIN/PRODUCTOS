using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> entity)
        {
            entity.HasKey(e => e.IdColor);
            entity.ToTable("Color");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(e => e.CodigoColor)
                .HasMaxLength(20);
        }
    }
}