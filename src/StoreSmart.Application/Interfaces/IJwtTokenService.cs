using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}