using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PRODUCTOS.Persistence.Configurations
{
    public class StateProductConfiguration : IEntityTypeConfiguration<StateProduct>
    {
        public void Configure(EntityTypeBuilder<StateProduct> entity)
        {
            entity.HasKey(e => e.IdState);
            entity.ToTable("StateProduct");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}