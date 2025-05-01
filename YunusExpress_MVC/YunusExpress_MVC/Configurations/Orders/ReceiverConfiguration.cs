using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.Configurations.Orders
{
    public class ReceiverConfiguration : IEntityTypeConfiguration<Receiver>
    {
        public void Configure(EntityTypeBuilder<Receiver> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReceiverName)
                .IsRequired()
                .HasMaxLength(64);

        }
    }
}
