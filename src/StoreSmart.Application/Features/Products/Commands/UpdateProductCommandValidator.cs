using FluentValidation;

namespace StoreSmart.Application.Features.Products.Commands;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Request.Name)
            .MinimumLength(3).MaximumLength(200).When(x => x.Request.Name != null);

        RuleFor(x => x.Request.Description)
            .MinimumLength(10).MaximumLength(2000).When(x => x.Request.Description != null);

        RuleFor(x => x.Request.Price)
            .GreaterThan(0).When(x => x.Request.Price.HasValue);
    }
}