using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.Configurations.Deliveries
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<DeliveryZone>
    {
        public void Configure(EntityTypeBuilder<DeliveryZone> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Name)
                .HasMaxLength(64)
                .IsRequired();
            builder
              .HasOne(x => x.DeliveryZoneType)
              .WithMany(x => x.Zones)
              .HasForeignKey(x => x.DeliveryZoneTypeId);

        }
    }
}
