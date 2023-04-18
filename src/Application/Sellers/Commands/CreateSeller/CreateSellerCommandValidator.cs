using FluentValidation;

namespace ReProServices.Application.Sellers.Commands.CreateSeller
{
    public class CreateSellerCommandValidator : AbstractValidator<CreateSellerCommand>
    {
        public CreateSellerCommandValidator()
        {
            RuleFor(s => s.SellerDtoObj.PAN)
           .Length(10)
           .NotEmpty()
           .WithMessage("PAN ID cannot be empty");
        }
    }
}
