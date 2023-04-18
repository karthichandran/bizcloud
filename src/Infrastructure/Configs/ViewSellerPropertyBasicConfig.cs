
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewSellerPropertyBasicConfig : IEntityTypeConfiguration<ViewSellerPropertyBasic>
    {
        public void Configure
           (EntityTypeBuilder<ViewSellerPropertyBasic> entity)
        {
            entity.ToView("ViewSellerPropertyBasic");
        }
    }
}
