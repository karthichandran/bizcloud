using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;
namespace ReProServices.Infrastructure.Configs
{
    public class ViewPendingServiceFeeConfig : IEntityTypeConfiguration<ViewPendingServiceFee>
    {
        public void Configure
           (EntityTypeBuilder<ViewPendingServiceFee> entity)
        {
            entity.ToView("ViewPendingServiceFee");
        }
    }
}
