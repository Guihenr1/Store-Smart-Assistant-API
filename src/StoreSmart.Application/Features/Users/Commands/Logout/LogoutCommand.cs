using Ardalis.Result;
using MediatR;

namespace StoreSmart.Application.Features.Users.Commands.Logout;

public sealed record LogoutCommand(Guid UserId) : IRequest<Result>;