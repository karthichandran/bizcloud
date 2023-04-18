using System;
using FluentValidation;
using NodaTime;
namespace ReProServices.Application.ClientPayments.Commands
{
    public class CreateClientPaymentValidator : AbstractValidator<CreateClientPaymentCommand>
    {
        public CreateClientPaymentValidator()
        {
            //_ = RuleFor(s => s.ClientPaymentVM.InstallmentBaseObject)
            //    .Must(x => LocalDate.FromDateTime(x.DateOfPayment.Value.Date) <= LocalDate.FromDateTime(DateTime.Today.AddDays(1).Date))
            //    .WithMessage("Date Of Payment cannot be a future date");
            ////_ = RuleFor(s => s.ClientPaymentVM.InstallmentBaseObject)
            ////    .Must(x => LocalDate.FromDateTime(x.DateOfDeduction.Value) <= LocalDate.FromDateTime(DateTime.Today))
            ////    .WithMessage("Date Of Deduction cannot be a future date");
            //_ = RuleFor(s => s.ClientPaymentVM.InstallmentBaseObject)
            //    .Must(x => LocalDate.FromDateTime(x.RevisedDateOfPayment.Value.Date) <= LocalDate.FromDateTime(DateTime.Today.AddDays(1).Date))
            //    .WithMessage("Revised Date Of Payment cannot be a future date");
        }
    }
}
