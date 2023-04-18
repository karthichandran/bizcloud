using FluentValidation;

namespace ReProServices.Application.TaxCodes.Command.CreateTaxCodesCommand
{
    public class CreateTaxCodeCommandValidator : AbstractValidator<CreateTaxCodeCommand>
    {
        public CreateTaxCodeCommandValidator()
        {
            RuleFor(s => s.TaxCodeDtoObj)
                .Must(x => x.TaxValue >= 0)
                .WithMessage("Tax Value Cannot be a negative number");

        }
    }
}
