using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;
namespace ReProServices.Infrastructure.Configs
{
    public class ViewPayableClientPaymentsConfig : IEntityTypeConfiguration<ViewPayableClientPayments>
    {
        public void Configure
           (EntityTypeBuilder<ViewPayableClientPayments> entity)
        {
            entity.ToView("ViewPayableClientPayments")
                .HasNoKey();
        }
    }
}
