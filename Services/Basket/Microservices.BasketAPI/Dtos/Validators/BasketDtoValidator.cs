using FluentValidation;

namespace Microservices.BasketAPI.Dtos.Validators
{
    public class BasketDtoValidator : AbstractValidator<BasketDto>
    {
        public BasketDtoValidator()
        {
            RuleFor(b => b.Id).NotNull().NotEmpty().WithMessage("Id is required");
            RuleFor(b => b.UserId).NotNull().NotEmpty().WithMessage("User Id is required");
            RuleFor(b => b.TotalPrice).GreaterThanOrEqualTo(0).WithMessage("Price must have greater than 0");
        }
    }
}
