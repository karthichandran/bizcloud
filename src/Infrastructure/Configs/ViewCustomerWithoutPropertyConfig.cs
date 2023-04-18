
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewCustomerWithoutPropertyConfig : IEntityTypeConfiguration<ViewCustomerWithoutProperty>
    {
        public void Configure(EntityTypeBuilder<ViewCustomerWithoutProperty> entity)
        {
            entity.ToView("ViewCustomerWithoutProperty");
        }
    }
}
