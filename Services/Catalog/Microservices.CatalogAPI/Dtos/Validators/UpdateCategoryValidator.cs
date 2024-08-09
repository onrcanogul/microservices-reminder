using FluentValidation;

namespace Microservices.CatalogAPI.Dtos.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("Id is required");
            RuleFor(c => c.Name).NotEmpty().NotNull().WithMessage("Name is required");
        }
    }
}
