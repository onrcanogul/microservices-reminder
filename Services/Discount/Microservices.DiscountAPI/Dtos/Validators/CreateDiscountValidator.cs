using FluentValidation;

namespace Microservices.DiscountAPI.Dtos.Validators
{
    public class CreateDiscountValidator : AbstractValidator<CreateDiscountDto>
    {
        public CreateDiscountValidator()
        {
            RuleFor(c => c.UserId).NotNull().NotEmpty();
            RuleFor(c => c.Code).NotNull().NotEmpty();
            RuleFor(c => c.Rate).GreaterThan(0).NotNull().NotEmpty();
        }
    }
}
