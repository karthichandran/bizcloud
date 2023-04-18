
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewClientPaymentImportConfig : IEntityTypeConfiguration<ViewClientPaymentImports>
    {
        public void Configure
           (EntityTypeBuilder<ViewClientPaymentImports> entity)
        {
            entity.ToView("ViewClientPaymentImports");
        }
    }
}
