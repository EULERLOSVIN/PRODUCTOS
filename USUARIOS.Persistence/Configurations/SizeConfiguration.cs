using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> entity)
        {
            entity.HasKey(e => e.IdSize);
            entity.ToTable("Size");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}