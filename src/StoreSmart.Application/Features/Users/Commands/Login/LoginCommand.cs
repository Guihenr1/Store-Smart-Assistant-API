using Ardalis.Result;
using MediatR;

namespace StoreSmart.Application.Features.Users.Commands.Login;

public sealed record LoginCommand(string email, string password) : IRequest<Result<LoginResponse>>;

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt, string RefreshToken);