using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Entities;

namespace NLayer.Repository.Seeds
{
    internal class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasData(
                new ProductFeature
                {
                    Id = 1,
                    Color = "Kırmızı",
                    Height = 100,
                    Width = 50,
                    ProductId = 1
                },
                   new ProductFeature
                   {
                       Id = 2,
                       Color = "Sarı",
                       Height = 20,
                       Width = 50,
                       ProductId = 2
                   });
        }
    }
}
