using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;
namespace ReProServices.Infrastructure.Configs
{
    public class ViewLotSummaryConfig : IEntityTypeConfiguration<ViewLotSummary>
    {
        public void Configure
           (EntityTypeBuilder<ViewLotSummary> entity)
        {
            entity.ToView("ViewLotSummary")
                .HasNoKey();
        }
    }
}
