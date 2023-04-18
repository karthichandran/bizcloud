
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewCustomerPropertyExpandedConfig : IEntityTypeConfiguration<ViewCustomerPropertyExpanded>
    {
        public void Configure(EntityTypeBuilder<ViewCustomerPropertyExpanded> entity)
        {
            entity
                .ToView("ViewCustomerPropertyExpanded");
        }
    }
}
