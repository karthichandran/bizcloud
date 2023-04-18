using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using ReProServices.Infrastructure.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReProServices.Infrastructure.Configs;

namespace ReProServices.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<States> StateList { get; set; }

        public DbSet<Property> Property { get; set; }

        public DbSet<SellerProperty> SellerProperty { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<CustomerProperty> CustomerProperty { get; set; }

        public DbSet<TaxCodes> TaxCodes { get; set; }

        public DbSet<TaxType> TaxType { get; set; }

        public DbSet<CustomerPropertyFile> CustomerPropertyFile { get; set; }

        public DbSet<Remark> Remark { get; set; }

        public DbSet<FileCategory>FileCategory { get; set; }

        public DbSet<CustomerBilling> CustomerBilling { get; set; }
        
        public DbSet<ClientPayment> ClientPayment { get; set; }

        public DbSet<NatureOfPayment> NatureOfPayment { get; set; }

        public DbSet<ClientPaymentTransaction> ClientPaymentTransaction { get; set; }
        
        public DbSet<StatusType> StatusType { get; set; }

        public DbSet<RemittanceStatus> RemittanceStatus { get; set; }
        public DbSet<Remittance> Remittance { get; set; }
        public DbSet<ModeOfReceipt> ModeOfReceipt { get; set; }
        public DbSet<Receipt> Receipt { get; set; }

        public DbSet<ClientPaymentRawImport> ClientPaymentRawImport { get; set; }

        public DbSet<ViewSellerPropertyBasic> ViewSellerPropertyBasic { get; set; }
        public DbSet<ViewCustomerPropertyBasic> ViewCustomerPropertyBasic { get; set; }
        public DbSet<ViewSellerPropertyExpanded> ViewSellerPropertyExpanded { get; set; }
        public DbSet<ViewCustomerPropertyExpanded> ViewCustomerPropertyExpanded { get; set; }
        public DbSet<ViewCustomerWithoutProperty> ViewCustomerWithoutProperty { get; set; }
        public DbSet<ViewClientPayment> ViewClientPayment { get; set; }
        public DbSet<ViewCustomerPropertyFile> ViewCustomerPropertyFile { get; set; }
        public DbSet<ViewClientPaymentImports> ViewClientPaymentImports { get; set; }
        public DbSet<ViewPendingServiceFee> ViewPendingServiceFee { get; set; }
        public DbSet<ViewReceipt> ViewReceipt { get; set; }

        public DbSet<ViewRemittance> ViewRemittance { get; set; }

        public DbSet<ViewLotSummary> ViewLotSummary { get; set; }

        public DbSet<ViewPayableClientPayments> ViewPayableClientPayments { get; set; }
        public DbSet<ViewClientPaymentReport> ViewClientPaymentReport { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<Segment> Segment { get; set; }

        public DbSet<SegmentRolePermissions> SegmentRolePermissions { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<Prospect> Prospect { get; set; }
        public DbSet<ProspectProperty> ProspectProperty { get; set; }
       public DbSet<ViewCustomerPropertyArchived> ViewCustomerPropertyArchived { get; set; }

        public DbSet<BankAccountDetails> BankAccountDetails { get; set; }
        public DbSet<ViewCustomerReport> ViewCustomerReports { get; set; }
        public DbSet<Message> Message { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreatedBy = _currentUserService.UserId;
            //            entry.Entity.Created = _dateTime.Now;
            //            break;
            //        case EntityState.Modified:
            //            entry.Entity.LastModifiedBy = _currentUserService.UserId;
            //            entry.Entity.LastModified = _dateTime.Now;
            //            break;
            //    }
            //}

            return base.SaveChangesAsync(cancellationToken);
        }


        public static readonly ILoggerFactory LoggerFactory
      = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
      {
          builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
            .AddConsole();
      });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfiguration(new CustomerPropertyConfig());
            builder.ApplyConfiguration(new ViewSellerPropertyBasicConfig());
            builder.ApplyConfiguration(new ViewSellerPropertyExpandedConfig());
            builder.ApplyConfiguration(new ViewCustomerPropertyBasicConfig());
            builder.ApplyConfiguration(new ViewCustomerWithoutPropertyConfig());
            builder.ApplyConfiguration(new ViewClientPaymentConfig());
            builder.ApplyConfiguration(new ViewCustomerPropertyFileConfig());
            builder.ApplyConfiguration(new ViewClientPaymentImportsConfig());
            builder.ApplyConfiguration(new ViewPendingServiceFeeConfig());
            builder.ApplyConfiguration(new ViewReceiptConfig());
            builder.ApplyConfiguration(new ViewRemittanceConfig());
            builder.ApplyConfiguration(new ViewLotSummaryConfig());
            builder.ApplyConfiguration(new ViewPayableClientPaymentsConfig());
            base.OnModelCreating(builder);
        }
    }
}
