using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Interfaces.Repositories;

namespace StoreSmart.Application.Features.Users.Commands.Logout;

public sealed class LogoutCommandHander(IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenRepository.RevokeTokenAsync(request.UserId, cancellationToken);
        
        return Result.Success();
    }
}