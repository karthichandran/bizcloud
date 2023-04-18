
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class SellerPropertyConfig : IEntityTypeConfiguration<SellerProperty>
    {
        public void Configure
           (EntityTypeBuilder<SellerProperty> entity)
        {
            //Relationships

            entity.HasOne(pt => pt.Seller)
                .WithMany(p => p.SellerProperty)
                .HasForeignKey(pt => pt.SellerID);

            entity.HasOne(pt => pt.Property)
                .WithMany(t => t.SellerProperty)
                .HasForeignKey(pt => pt.PropertyID);
        }

    }
}
