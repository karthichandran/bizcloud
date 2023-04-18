using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewClientPaymentReportConfig : IEntityTypeConfiguration<ViewClientPaymentReport>
    {
        public void Configure
           (EntityTypeBuilder<ViewClientPaymentReport> entity)
        {
            entity.ToView("ViewClientPaymentReport")
                .HasNoKey();
        }
    }
}
