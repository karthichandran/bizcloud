using ReProServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReProServices.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        DbSet<Seller> Seller { get; set; }

        DbSet<Domain.Entities.States> StateList { get; set; }

        DbSet<Domain.Entities.Property> Property { get; set; }

        DbSet<Domain.Entities.SellerProperty> SellerProperty { get; set; }

        DbSet<Customer> Customer { get; set; }

        DbSet<Domain.Entities.CustomerProperty> CustomerProperty { get; set; }

        DbSet<Domain.Entities.TaxCodes> TaxCodes { get; set; }

        DbSet<TaxType> TaxType { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<CustomerPropertyFile> CustomerPropertyFile { get; set; }

        DbSet<Remark> Remark { get; set; }

        DbSet<FileCategory> FileCategory { get; set; }

        DbSet<CustomerBilling> CustomerBilling { get; set; }

        DbSet<ClientPayment> ClientPayment { get; set; }

        DbSet<NatureOfPayment> NatureOfPayment { get; set; }

        DbSet<ClientPaymentTransaction> ClientPaymentTransaction { get; set; }

        DbSet<StatusType> StatusType { get; set; }

        DbSet<RemittanceStatus> RemittanceStatus { get; set; }
        DbSet<Remittance> Remittance { get; set; }
        DbSet<Receipt> Receipt { get; set; }
        DbSet<Domain.Entities.ModeOfReceipt> ModeOfReceipt { get; set; }
        DbSet<ClientPaymentRawImport> ClientPaymentRawImport { get; set; }
        public DbSet<RemittanceRemark> RemittanceRemark { get; set; }
        public DbSet<ClientTransactionRemark> ClientTransactionRemark { get; set; }
        //public DbSet<Domain.Entities.DetailsSummaryReport> DetailsSummaryReports { get; set; }
        //Views
        DbSet<ViewSellerPropertyBasic> ViewSellerPropertyBasic { get; set; }
        DbSet<ViewSellerPropertyExpanded> ViewSellerPropertyExpanded { get; set; }

        DbSet<ViewCustomerPropertyBasic> ViewCustomerPropertyBasic { get; set; }
        DbSet<ViewCustomerPropertyExpanded> ViewCustomerPropertyExpanded { get; set; }

        DbSet<ViewCustomerWithoutProperty> ViewCustomerWithoutProperty { get; set; }
        DbSet<ViewCustomerPropertyFile> ViewCustomerPropertyFile { get; set; }

        DbSet<ViewClientPayment> ViewClientPayment { get; set; }
        DbSet<ViewClientPaymentImports> ViewClientPaymentImports { get; set; }
        DbSet<ViewPendingServiceFee> ViewPendingServiceFee { get; set; }
        DbSet<ViewReceipt> ViewReceipt { get; set; }
        DbSet<ViewRemittance> ViewRemittance { get; set; }
        DbSet<ViewLotSummary> ViewLotSummary { get; set; }

        DbSet<ViewPayableClientPayments> ViewPayableClientPayments { get; set; }
        DbSet<ViewClientPaymentReport> ViewClientPaymentReport { get; set; }


        DbSet<Domain.Entities.Users> Users { get; set; }
        DbSet<Domain.Entities.Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Domain.Entities.Segment> Segment { get; set; }
        public DbSet<SegmentRolePermissions> SegmentRolePermissions { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        // IEnumerable<object> Users { get; }
        public DbSet<Domain.Entities.Prospect> Prospect { get; set; }
        public DbSet<ProspectProperty> ProspectProperty { get; set; }

        DbSet<ViewCustomerPropertyArchived> ViewCustomerPropertyArchived { get; set; }
        DbSet<BankAccountDetails> BankAccountDetails { get; set; }
        DbSet<ViewCustomerReport> ViewCustomerReports { get; set; }
        DbSet<Domain.Entities.Message> Message { get; set; }
        DbSet<Domain.Entities.DebitAdvice> DebitAdvices { get; set; }
    }
}
