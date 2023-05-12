using System;
using ReProServices.Domain.Entities;
using NodaTime;

namespace ReProServices.Application.Common.Formulas
{
    ///two sections that are constantly used in TDS interest calculation:
    ///1. Section 201(1A)-Interest on failure to deduct tax and interest on failure to deposit deducted tax amount.

    ///provisions are required to be known for calculation of interest:
    ///2. Section 288B-Rounding off the tax payable to multiples of Rs.1 0 (similar to Elementary Maths Rounding).
    
    public static class TaxCalculatorByShare
    {
        const decimal TDS_DEDUCTION_DELAY_PERCENTAGE = 1;

        public static TaxesAndFees CalculateTaxAndFees(TaxesAndFees tfObj)
        {
            RunValidations(tfObj);
            tfObj.AmountShare = Math.Ceiling(tfObj.AmountPaid * (tfObj.Share / 100));
            
            tfObj.AmountValue = CalculateAmountValue(tfObj);
            
            tfObj.TdsAmount = CalculateTds(tfObj);
            tfObj.GSTAmount = CalculateGst(tfObj);
            tfObj.GrossAmount = CalculateGrossValue(tfObj);
            tfObj.DueDateOfPaymentOfTds = CalculateDueDate(tfObj);
            tfObj.NoOfDaysOfDelay = CalculateNoOfDaysDelay(tfObj);
            tfObj.NoOfMonthsOfDeductionDelay = CalculateDeductionLateFeeMonths(tfObj);
            tfObj.TdsInterestAmount = CalculateTdsInterest(tfObj);
            tfObj.LateFeeAmount = CalculateLateFee(tfObj);
            
            return tfObj;
        }

        private static void RunValidations(TaxesAndFees tf)
        {
            //if (LocalDate.FromDateTime(tf.DateOfPayment) < LocalDate.FromDateTime(tf.DateOfDeduction))
            //{
            //    throw  new ApplicationException("Date of Payment to Seller cannot be before Date of Deduction");
            //}
        }

        public static decimal CalculateAmountValue(TaxesAndFees tfObj)
        {
            return 100 + tfObj.GstPercentage + (tfObj.IsTdsDeductedBySeller ?  0 : -tfObj.TdsPercentage);
        }

        public static decimal CalculateTds(TaxesAndFees tf)
        {
            //truncate will ignore decimal values after 2 numbers
            var trunc = Math.Truncate(100* ((tf.AmountShare / tf.AmountValue) * tf.TdsPercentage))/100;
            return Math.Round(trunc,MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateGst(TaxesAndFees tfObj)
        {
            var trunc = Math.Truncate(100 * ((tfObj.AmountShare / tfObj.AmountValue) * tfObj.GstPercentage))/100;
            return Math.Ceiling(trunc);
        }
         
        public static decimal CalculateGrossValue(TaxesAndFees tf)
        {
            if (tf.IsTdsDeductedBySeller)
            {
                return tf.AmountShare - tf.GSTAmount;
            }
            else
            {
                return tf.AmountShare - tf.GSTAmount + tf.TdsAmount;
            }
        }

        ///Last day of next month
        public static DateTime CalculateDueDate(TaxesAndFees tf)
        {
            var date = LocalDateTime.FromDateTime(tf.DateOfPayment.AddMonths(1)); 
            
           return   new DateTime(date.Year, date.Month, 
                                          DateTime.DaysInMonth(date.Year, date.Month)); 
        }

        public static int CalculateNoOfDaysDelay(TaxesAndFees tf)
        {
          var days =   Period.Between(LocalDate.FromDateTime(tf.DueDateOfPaymentOfTds), 
                                          LocalDate.FromDateTime(tf.DateOfDeduction), PeriodUnits.Days).Days ;
            if (days <= 0)
                return 0;
            else
                return days;
        }

        public static int CalculateDeductionLateFeeMonths(TaxesAndFees tf)
        {
            if (tf.NoOfDaysOfDelay > 0)
            {
                //addition of 1 month to the period will account to include start month and end month too
               var months = 12 * (  tf.DateOfDeduction.Year - tf.DateOfPayment.Year) +
                         (tf.DateOfDeduction.Month - tf.DateOfPayment.Month) + 1; 
                Console.WriteLine("months : " + months);
                return Math.Abs(months);
            }
            else
            {
                return 0;
            }
        }

        // this only takes care of deduction delay interest
        //ignoring tds delay pay interest that is @  1.5%
        public static decimal CalculateTdsInterest(TaxesAndFees tf)
        {
            if (tf.NoOfDaysOfDelay > 0)
                return Math.Ceiling((TDS_DEDUCTION_DELAY_PERCENTAGE / 100) * tf.NoOfMonthsOfDeductionDelay * tf.TdsAmount);  
            else
                return 0;
        }

        public static decimal CalculateLateFee(TaxesAndFees tf)
        {
            var lateFee = tf.NoOfDaysOfDelay * tf.LateFeePerDay;

            if (lateFee == 0)
                return 0;

            if (lateFee > Math.Ceiling(tf.TdsAmount))
                return Math.Ceiling(tf.TdsAmount);
            else
                return Math.Ceiling(lateFee);

        }

    }
}
