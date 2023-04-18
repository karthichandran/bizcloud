
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewCustomerPropertyBasicConfig : IEntityTypeConfiguration<ViewCustomerPropertyBasic>
    {
        public void Configure(EntityTypeBuilder<ViewCustomerPropertyBasic> entity)
        {
            entity.ToView("ViewCustomerPropertyBasic");
        }
    }
}
