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
            builder.HasKey(x => x.Id);

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
            builder
                  .HasOne(o => o.Courier)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CourierId);


            //  qaime unique olmalidi
            builder.HasIndex(x => x.InvoiceNo).IsUnique();

            //sifarisler unique olmalidi
            builder.HasIndex(x => x.OrderNo).IsUnique();
        }
    }
}
