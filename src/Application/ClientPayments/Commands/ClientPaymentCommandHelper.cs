using ReProServices.Application.Common.Interfaces;
using System.Linq;


namespace ReProServices.Application.ClientPayments.Commands
{
    public  class ClientPaymentCommandHelper
    {
        private readonly IApplicationDbContext _context;

        public  ClientPaymentCommandHelper(IApplicationDbContext context)
        {
            _context = context;
        }
        public  bool CheckIfDuplicateReceipt(int sellerId, string receiptNo)
        {
            var receiptCount = (from pay in _context.ClientPayment
                                join cpt in _context.ClientPaymentTransaction on pay.InstallmentID equals cpt.InstallmentID
                                where cpt.SellerID == sellerId
                                      && pay.ReceiptNo == receiptNo
                                select 1)
                .ToList().Count;
            return receiptCount > 0;
        }

    }
}
