using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Options;
using StoreSmart.Application.Common.DTOs;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Application.Settings;
using StoreSmart.Domain.Entities;
using StoreSmart.Domain.ValueObjects;

namespace StoreSmart.Application.Features.Users.Commands.Register;

public sealed class RegisterUserCommandHandler 
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettingsOptions)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettingsOptions.Value;
    }
    
    public async Task<Result<RegisterUserResponse>> Handle(
        RegisterUserCommand request, 
        CancellationToken ct)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, ct);
        
        if (existingUser != null)
        {
            return Result.Conflict(
                existingUser.IsActive ? "Email already registered" : "Email already registered, but inactive");
        }
        
        var user = User.Create(
            Email.From(request.Email), 
            request.Name, 
            _passwordHasher.HashPassword(request.Password));

        await _userRepository.AddAsync(user, ct);
        
        var accessToken = _jwtTokenService.GenerateToken(user);
        
        var refreshToken = RefreshToken.Create(user.Id, DateTime.UtcNow.AddDays(30));
        await _refreshTokenRepository.AddAsync(refreshToken, ct);

        return Result.Success(new RegisterUserResponse(
            AccessToken: accessToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes), 
            RefreshToken: refreshToken.Token));
    }
}