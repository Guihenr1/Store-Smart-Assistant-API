using Microsoft.AspNetCore.Identity;
using StoreSmart.Application.Interfaces;

namespace StoreSmart.Infrastructure.Auth;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly IPasswordHasher<IdentityUser> _hasher = new PasswordHasher<IdentityUser>();

    public string HashPassword(string password)
        => _hasher.HashPassword(null!, password);

    public bool VerifyPassword(string password, string hash)
        => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
}