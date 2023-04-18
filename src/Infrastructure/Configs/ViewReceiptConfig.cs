using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;
namespace ReProServices.Infrastructure.Configs
{
    public class ViewReceiptConfig : IEntityTypeConfiguration<ViewReceipt>
    {
        public void Configure
           (EntityTypeBuilder<ViewReceipt> entity)
        {
            entity.ToView("ViewReceipt")
                .HasNoKey();
        }
    }
}
