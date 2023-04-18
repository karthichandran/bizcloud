
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewSellerPropertyExpandedConfig : IEntityTypeConfiguration<ViewSellerPropertyExpanded>
    {
        public void Configure
           (EntityTypeBuilder<ViewSellerPropertyExpanded> entity)
        {
            entity.ToView("ViewSellerPropertyExpanded");
        }
    }
}
