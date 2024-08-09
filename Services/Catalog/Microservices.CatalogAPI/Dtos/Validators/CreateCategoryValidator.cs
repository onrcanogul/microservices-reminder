using FluentValidation;

namespace Microservices.CatalogAPI.Dtos.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("Name is required");
        }
    }
}
