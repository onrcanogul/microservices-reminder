using FluentValidation;

namespace Microservices.CatalogAPI.Dtos.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty().WithMessage("Id is required");
            RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Price is required").GreaterThanOrEqualTo(0).WithMessage("Price must have greater than 0");
            RuleFor(p => p.UserId).NotNull().NotEmpty().WithMessage("User Id is required");
            RuleFor(p => p.ImagePath).NotNull().NotEmpty().WithMessage("Image Path is required");
            RuleFor(p => p.Description).NotNull().NotEmpty().WithMessage("Description is required");
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage("Name is required");
        }
    }
}
