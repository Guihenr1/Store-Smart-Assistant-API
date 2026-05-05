using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Options;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Application.Settings;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Features.Users.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IOptions<JwtSettings> jwtSettingsOptions,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly JwtSettings _jwtSettings = jwtSettingsOptions.Value;

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.email, cancellationToken);
        if (user == null || !passwordHasher.VerifyPassword(request.password, user.PasswordHash))
        {
            return Result.Unauthorized("Invalid email or password");
        }

        if (!user.IsActive)
        {
            return Result.Forbidden("User is inactive");
        }

        var accessToken = jwtTokenService.GenerateToken(user);
        
        var refreshToken = RefreshToken.Create(user.Id, DateTime.UtcNow.AddDays(30));
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        
        return Result.Success(new LoginResponse(
            AccessToken: accessToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes), 
            RefreshToken: refreshToken.Token));
    }
}