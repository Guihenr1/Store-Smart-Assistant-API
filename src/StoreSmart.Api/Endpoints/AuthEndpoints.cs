using System.Security.Claims;
using MediatR;
using StoreSmart.Api.Common;
using StoreSmart.Application.Features.Refresh;
using StoreSmart.Application.Features.Users.Commands.Login;
using StoreSmart.Application.Features.Users.Commands.Logout;
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
        
        // ====================== LOGIN ======================
        group.MapPost("/login", async (
                LoginCommand command, 
                IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToMinimalApiResult();
            })
            .AllowAnonymous()
            .WithName("LoginUser")
            .WithDescription("Login an user account");
        
        // ====================== LOGOUT ======================
        group.MapPost("/logout", async (
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                  ?? httpContext.User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                var command = new LogoutCommand(userId);
                var result = await mediator.Send(command);

                return result.ToMinimalApiResult();
            })
            .RequireAuthorization()
            .WithName("Logout")
            .WithDescription("Logout current user");
        
        // ====================== REFRESH TOKEN ======================
        group.MapPost("/refresh", async (
                RefreshTokenCommand command, 
                IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToMinimalApiResult();
            })
            .AllowAnonymous()
            .WithName("RefreshToken")
            .WithDescription("Refresh access token using refresh token");
    }
}