using FluentValidation;
using System.Text.RegularExpressions;

namespace ReProServices.Application.Property.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {

        public CreatePropertyCommandValidator()
        {
            //todo figure the need for discard option
            _ = RuleForEach(s => s.PropertyVM.sellerProperties)
                .Must(x => x.SellerShare > 0)
                .WithMessage("Share Value Cannot be 0");

            RuleFor(s => s.PropertyVM.propertyDto)
                .Must(x => Regex.IsMatch(x.PinCode.TrimEnd(), "^[0-9 ]+$"))
                 .WithMessage("Pin Code Should be digits only"); 
        }
    }
}

//todo refer https://stackoverflow.com/questions/30505937/validate-collection-using-sum-on-a-property and improve validation

