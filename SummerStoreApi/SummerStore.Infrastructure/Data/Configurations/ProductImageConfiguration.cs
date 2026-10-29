using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SummerStore.Core.Models.Entities;

namespace SummerStore.Infrastructure.Data.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        // builder.HasKey(x => x.Id);
        //
        // builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
    }
}
