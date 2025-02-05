using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);

        builder.HasOne(oi => oi.Product)
       .WithMany()
       .HasForeignKey(oi => oi.ProductId)
       .OnDelete(DeleteBehavior.Restrict); //Якщо видалення продукту не повинно видаляти замовлення

        builder.Property(oi => oi.Price)
            .HasColumnType("decimal(18,2)");
    }
}

