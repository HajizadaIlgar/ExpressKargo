using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.Configurations.Deliveries
{
    public class DeliveryTypeConfiguration : IEntityTypeConfiguration<DeliveryZoneType>
    {
        public void Configure(EntityTypeBuilder<DeliveryZoneType> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.TypeName)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .HasMany(x => x.Senders)
                .WithMany(x => x.DeliveryZones);
        }
    }
}
