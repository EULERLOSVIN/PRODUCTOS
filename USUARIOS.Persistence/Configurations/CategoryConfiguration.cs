using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(e => e.IdCategory);
            entity.ToTable("Category");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}