using FluentValidation;

namespace StoreSmart.Application.Features.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Request.SKU)
            .NotEmpty().WithMessage("SKU is required")
            .MaximumLength(50);

        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Request.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10)
            .MaximumLength(2000);

        RuleFor(x => x.Request.Brand)
            .NotEmpty().WithMessage("Brand is required")
            .MaximumLength(100);

        RuleFor(x => x.Request.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(x => x.Request.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(x => x.Request.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}