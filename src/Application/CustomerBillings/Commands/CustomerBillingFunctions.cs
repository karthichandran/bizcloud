using System;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.CustomerBillings.Commands
{
    public class CustomerBillingFunctions
    {

        internal decimal CalculateAmount(CustomerBillingDto custBill)
        {
            if (custBill.PaymentMethodID == (int)EPaymentMethod.PerTransation)
            {
                if (!custBill.CostPerInstallment.HasValue)
                {
                    throw new ApplicationException("Charge Per Installment cannot be null");
                }

                if (!custBill.NoOfInstallments.HasValue)
                {
                    throw new ApplicationException("No of Installments cannot be null");
                }

                return custBill.CostPerInstallment.Value * custBill.NoOfInstallments.Value;
            }

            else
                return custBill.Amount ?? throw new ApplicationException("Amount cannot be empty");

        }
    }
}
