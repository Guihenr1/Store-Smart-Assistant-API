using MediatR;
using StoreSmart.Api.Common;
using StoreSmart.Application.Features.Users.Commands.Register;

namespace StoreSmart.Api.Endpoints;

public static class AuthEndpoints
{
    public static void RegisterAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        // ====================== REGISTER ======================
        group.MapPost("/register", async (
                RegisterUserCommand command, 
                IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToMinimalApiResult();
            })
            .AllowAnonymous()
            .WithName("RegisterUser")
            .WithDescription("Register a new user account");
    }
}