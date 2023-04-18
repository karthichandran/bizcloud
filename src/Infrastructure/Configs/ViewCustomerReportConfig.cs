using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;


namespace ReProServices.Infrastructure.Configs
{
    public class ViewCustomerReportConfig : IEntityTypeConfiguration<ViewCustomerReport>
    {
        public void Configure
          (EntityTypeBuilder<ViewCustomerReport> entity)
        {
            entity.ToView("ViewCustomerReport")
                .HasNoKey();
        }
    }
}
