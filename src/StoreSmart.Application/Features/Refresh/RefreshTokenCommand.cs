using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Users.Commands.Login;

namespace StoreSmart.Application.Features.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) 
    : IRequest<Result<LoginResponse>>;