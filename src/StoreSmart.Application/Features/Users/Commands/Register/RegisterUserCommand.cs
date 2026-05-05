using Ardalis.Result;
using FluentValidation;
using MediatR;
using StoreSmart.Application.Common.DTOs;

namespace StoreSmart.Application.Features.Users.Commands.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Name,
    string Password) : IRequest<Result<RegisterUserResponse>>;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).MaximumLength(150).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8);
    }
}