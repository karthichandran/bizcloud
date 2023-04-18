
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReProServices.Domain.Entities;

namespace ReProServices.Infrastructure.Configs
{
    public class ViewCustomerPropertyFileConfig : IEntityTypeConfiguration<ViewCustomerPropertyFile>
    {
        public void Configure
           (EntityTypeBuilder<ViewCustomerPropertyFile> entity)
        {
            entity.ToView("ViewCustomerPropertyFile");
        }
    }
}
