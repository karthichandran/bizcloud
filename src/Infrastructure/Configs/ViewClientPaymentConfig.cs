
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewClientPaymentConfig : IEntityTypeConfiguration<ViewClientPayment>
    {
        public void Configure
           (EntityTypeBuilder<ViewClientPayment> entity)
        {
            entity.ToView("ViewClientPayment");
        }
    }
}
