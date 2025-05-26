using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.Configurations.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary key
            builder.HasKey(x => x.OrderNo);

            // note hissesidi() isteye bagli 
            builder.Property(x => x.Note)
                   .HasMaxLength(500);

            //Relations (Service)
            builder.HasOne(x => x.ServiceType)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ServiceId);

            //Relations(Receiver)
            //builder.HasOne(x => x.Receiver)
            //       .WithMany(x => x.Orders)
            //       .HasForeignKey(x => x.ReceiverId);

            // Relations (Sender)
            //builder.HasOne(x => x.Sender)
            //       .WithMany(x => x.Orders)
            //       .HasForeignKey(x => x.Senderid);

            // Relations (Courier)

            builder.HasOne(o => o.FromCourier)
                  .WithMany()
                  .HasForeignKey(o => o.FromCourierId)
                  .OnDelete(DeleteBehavior.NoAction); // <=== Əlavə olunur

            builder.HasOne(o => o.ToCourier)
                .WithMany()
                .HasForeignKey(o => o.ToCourierId)
                .OnDelete(DeleteBehavior.NoAction); // <=== Əlavə olunur

            //  qaime unique olmalidi
            builder.HasIndex(x => x.InvoiceNo).IsUnique();

            //sifarisler unique olmalidi
            builder.HasIndex(x => x.OrderNo).IsUnique();
        }
    }
}
