using System.Collections.Generic;

namespace ReProServices.Application.ClientPayments
{
    public class ClientPaymentVM
    {
        public ClientPaymentVM()
        {
            InstallmentBaseObject = new ClientPaymentDto();
            ExistingInstallments = new List<ClientPaymentDto>();
        }
        public ClientPaymentDto InstallmentBaseObject { get; set; }
        public IList<ClientPaymentDto> ExistingInstallments { get; set; }
    }
}
