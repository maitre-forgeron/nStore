using CatalogService.Domain;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Db.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(Constants.TextMaxLength).IsRequired();

            builder
                .HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            builder.OwnsOne(p => p.Price);
            builder.OwnsOne(p => p.Image);
        }
    }
}
