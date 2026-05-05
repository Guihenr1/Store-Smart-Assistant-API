using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Users.Commands.Login;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Features.Refresh;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly IUserRepository _userRepo;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshRepo, 
        IUserRepository userRepo, IJwtTokenService jwtTokenService)
    {
        _refreshRepo = refreshRepo;
        _userRepo = userRepo;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var oldToken = await _refreshRepo.GetByTokenAsync(request.RefreshToken, ct);
        if (oldToken == null || !oldToken.IsValid())
            return Result.Unauthorized("Invalid or expired refresh token.");

        var user = await _userRepo.GetByIdAsync(oldToken.UserId, ct);
        if (user == null) return Result.Unauthorized();

        oldToken.Revoke();
        await _refreshRepo.UpdateAsync(oldToken, ct);

        var accessToken = _jwtTokenService.GenerateToken(user);
        var newRefresh = RefreshToken.Create(user.Id, DateTime.UtcNow.AddDays(30));
        await _refreshRepo.AddAsync(newRefresh, ct);

        return Result.Success(new LoginResponse(accessToken, DateTime.UtcNow.AddHours(1), newRefresh.Token));
    }
}