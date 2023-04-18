using FluentValidation;

namespace ReProServices.Application.CustomerProperty.Commands.CreateCustomerProperty
{
    public class CreateCustomerPropertyCommandValidator : AbstractValidator<CreateCustomerPropertyCommand>
    {
        public CreateCustomerPropertyCommandValidator()
        {
            _ = RuleForEach(s => s.CustomerVM.customers).ChildRules(x => x.RuleFor(d => d.CustomerProperty)
                   .ForEach(y => y.Must(z => z.UnitNo > 0).WithMessage("Unit No should not be empty or 0")));
            _ = RuleForEach(s => s.CustomerVM.customers).ChildRules(x => x.RuleFor(d => d.CustomerProperty)
                   .ForEach(y => y.Must(z => z.GstRateID > 0).WithMessage("GST Rate must be selected")));
            _ = RuleForEach(s => s.CustomerVM.customers).ChildRules(x => x.RuleFor(d => d.CustomerProperty)
                   .ForEach(y => y.Must(z => z.TdsRateID > 0).WithMessage("TDS Rate must be selected")));

        }
    }
}
