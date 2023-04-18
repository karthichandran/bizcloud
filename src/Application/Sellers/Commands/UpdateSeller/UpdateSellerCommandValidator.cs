using FluentValidation;

namespace ReProServices.Application.Sellers.Commands.UpdateSeller
{
    public class UpdateSellerCommandValidator : AbstractValidator<UpdateSellerCommand>
    {

        public UpdateSellerCommandValidator()
        {
            RuleFor(s => s.SellerDto.PAN)
     .Length(10)
     .NotEmpty();
        }
  
    }
}
