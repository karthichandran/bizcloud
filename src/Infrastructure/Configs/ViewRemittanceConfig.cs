using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;
namespace ReProServices.Infrastructure.Configs
{
    public class ViewRemittanceConfig : IEntityTypeConfiguration<ViewRemittance>
    {
        public void Configure
           (EntityTypeBuilder<ViewRemittance> entity)
        {
            entity.ToView("ViewRemittance")
                .HasNoKey();
        }
    }
}
