using FluentValidation;

namespace Microservices.DiscountAPI.Dtos.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateDiscountDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
            RuleFor(c => c.UserId).NotNull().NotEmpty();
            RuleFor(c => c.Code).NotNull().NotEmpty();
            RuleFor(c => c.Rate).GreaterThan(0).NotNull().NotEmpty();
        }
    }
}
